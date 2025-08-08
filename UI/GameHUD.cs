using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameHUD : MonoBehaviour
{ 
    [SerializeField]
    public List<UIGameModeObj> gameModes = new List<UIGameModeObj>();

    [SerializeField]
    public Dictionary<Type, GamemodeHUD> huds = new Dictionary<Type, GamemodeHUD>();

    public static GameHUD HUD;

    [SerializeField]
    public WaitingUI waitingUI;

    [SerializeField]
    public EndUI endUI;

    [SerializeField]
    public GamemodeSelectionUI gamemodeSelectorUI;

    [SerializeField]
    private AudioSource selectionSource;

    [SerializeField]
    private AudioSource selectionFinishSource;

    [SerializeField]
    private AudioClip selectionClip;


    public GameObject GamemodeObjPrefab;

    private int previousSelectionIndex;

    public Transform GamemodesContainer;

    private int selectionIndex;

    private float timer = 0;

    private float textAlpha = 0.5F;

    void Awake()
    {
        HUD = this;

        GameObject GameHUD = GameObject.Find("GameHUD");

        huds.Add(typeof(BattleRoyal), new BattleRoyalHUD(GameHUD.transform.GetChild(2).gameObject));
        huds.Add(typeof(TeamFight), new TeamFightHUD(GameHUD.transform.GetChild(3).gameObject));
        huds.Add(typeof(DefaultHUD), new DefaultHUD(GameHUD.transform.GetChild(4).gameObject));

        GamemodeObjPrefab = Resources.Load<GameObject>("UI/HUD/GMSelection/GameModeObject");
    }


    void addGamemodes()
    {
        List<GameMode> gameModes = GameLogic.instance.getPartyManager().GetGameModeSelector().getPlayeableGameModes();

        foreach(GameMode mode in gameModes)
        {
            this.gameModes.Add(new UIGameModeObj(Instantiate(GamemodeObjPrefab,GamemodesContainer), mode.getDisplayName(), mode.GetType()));
        }
    }


    // Update is called once per frame
    void Update()
    {
        PartyManager manager = GameLogic.instance.getPartyManager();

        timer += Time.deltaTime;


        if (manager.getGameState() == GameState.WAITING)
        {
            if (endUI.ContainerObj.activeInHierarchy)
            {
                endUI.ContainerObj.SetActive(false);
            }

            waitingUI.ContainerObj.SetActive(true);

            waitingUI.PlayersCountTxt.text = "JOUEURS : " + GameLogic.instance.getPlayerCount() + " / 100";

            TimeSpan from = TimeSpan.FromSeconds(GameLogic.instance.getPartyManager().getWaitingTimer());

            waitingUI.GameWaitingTxt.text = $"LA PARTIE COMMENCE DANS { from.ToString(@"mm\:ss") }";
        }
        else if(manager.getGameState() == GameState.STARTING)
        {
            if(waitingUI.ContainerObj.activeInHierarchy) waitingUI.ContainerObj.SetActive(false);

            if (!gamemodeSelectorUI.ContainerObj.activeInHierarchy)
            {
                addGamemodes();
                gamemodeSelectorUI.ContainerObj.SetActive(true);
            }

            float f = Mathf.Max(0.1F, 1.0F - (manager.getSelectionTimer() / (PartyManager.SELECTION_TIME)));

            if(manager.getSelectionTimer() <= 2)
            {
                if (gameModes[selectionIndex].gamemodeType == manager.getCurrentGameMode().GetType())
                {
                    selectionSource.Stop();

                    gameModes[selectionIndex].gamemodeName.alpha = Mathf.Sin(textAlpha * 5F);

                    textAlpha += Time.deltaTime;

                    if(!manager.getCanStart())
                    {
                        manager.setReadyToStart();

                        selectionFinishSource.Play();
                    }
                }
                else
                {
                    selectAnimation(f);
                }
            }
            else
            {
                selectAnimation(f);
            }
        }
        else if (manager.getGameState() == GameState.STARTED)
        {

            GamemodeHUD hud = GetGamemodeHUD();

            hud.Update(manager);

            if (!hud.isActive()) hud.setActive(true);

            if (manager.getStartTimer() <= 0)
            {
                if (gamemodeSelectorUI.ContainerObj.activeInHierarchy) gamemodeSelectorUI.ContainerObj.SetActive(false);
            }
        }
        else
        {
            GamemodeHUD hud = GetGamemodeHUD();

            if (hud.isActive()) hud.setActive(false);
        }
    }

    public void displayEndTxt(string txt)
    {
        endUI.EndTxt.text = txt;

        DelayedAction.InvokeDelayed(delegate
        {
            endUI.ContainerObj.SetActive(true);
        }, 1F);
    }

    public GamemodeHUD GetGamemodeHUD()
    {
        PartyManager manager = GameLogic.instance.getPartyManager();

        Type currentGamemodeType = manager.getCurrentGameMode().GetType();

        if (huds.ContainsKey(currentGamemodeType))
        {
            return huds[currentGamemodeType];
        }

        return huds[typeof(DefaultHUD)];
    }

    void selectAnimation(float t)
    {
        if(timer > t)
        {
            timer = 0;

            selectionSource.PlayOneShot(selectionClip);

            selectNextGameMode();
        }
    }

    void selectNextGameMode()
    {

        previousSelectionIndex = selectionIndex;

        if (++selectionIndex > gameModes.Count-1)
        {
            selectionIndex = 0;
        }

        gameModes[previousSelectionIndex].setUnselected();
        gameModes[selectionIndex].setSelected();

    }


}
public class UIGameModeObj
{
    [SerializeField]
    public Image image;

    [SerializeField]
    public TextMeshProUGUI gamemodeName;

    public GameObject obj;

    public Type gamemodeType;
  

    public UIGameModeObj(GameObject obj, string displayName, Type gamemodeType)
    {
        this.obj = obj;

        gamemodeName = obj.GetComponentInChildren<TextMeshProUGUI>();

        image = obj.GetComponent<Image>();

        gamemodeName.text = displayName;

        this.gamemodeType = gamemodeType;
    }

    public void setSelected()
    {
        image.color = new Color(141 / 255F, 204 / 255F, 1F);
    }

    public void setUnselected()
    {
        image.color = new Color(169 / 255F, 194 / 255F, 214 / 255F);
    }
}

[System.Serializable]
public class WaitingUI
{
    [SerializeField]
    public GameObject ContainerObj;

    [SerializeField]
    public TextMeshProUGUI GameWaitingTxt;

    [SerializeField]
    public TextMeshProUGUI PlayersCountTxt;
}

[System.Serializable]
public class GamemodeSelectionUI
{
    [SerializeField]
    public GameObject ContainerObj;
}

[System.Serializable]
public class EndUI
{
    [SerializeField]
    public GameObject ContainerObj;

    [SerializeField]
    public TextMeshProUGUI EndTxt;
}