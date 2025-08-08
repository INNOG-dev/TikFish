using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StreamManager : MonoBehaviour
{

    public static StreamManager instance;

    private List<Streamer> streamers = new List<Streamer>();

    [SerializeField]
    private TMP_InputField StreamerNameInput;

    [SerializeField]
    private Button AddStreamerBtn;

    [SerializeField]
    private Button RemoveStreamerBtn;

    [SerializeField]
    private Button StartLiveBtn;

    [SerializeField]
    private TMP_Dropdown SelectStreamerDropdown;

    [SerializeField]
    private DialogBox DialogBox;

    [SerializeField]
    private GameObject UIObject;



    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        AddStreamerBtn.onClick.AddListener(addStreamer);
        RemoveStreamerBtn.onClick.AddListener(removeStreamer);
        StartLiveBtn.onClick.AddListener(startLive);

        int count = PlayerPrefs.GetInt("StreamerCount");

        for(int i = 0; i < count; i++)
        {
            string tiktokName = PlayerPrefs.GetString("TiktokUsername_" + i);
            Streamer user = new Streamer(tiktokName, "", "");
            streamers.Add(user);

            addUserToDropdown(user);
        }

        

    }

 

    private void addUserToDropdown(Streamer user)
    {
        TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData();
        newOption.text = user.getTiktokName();
        SelectStreamerDropdown.options.Add(newOption);
    }

    private void removeUserFromDropdown(Streamer user)
    {
        SelectStreamerDropdown.options.RemoveAll(x => x.text == user.getTiktokName());
    }

    private void addStreamer()
    {

        if(StreamerNameInput.text.Length == 0)
        {
            DialogBox.DisplayDialogBox("<color=red>Vous devez entrer un nom d'utilisateur</color>");
            return;
        }

        Streamer user = new Streamer(StreamerNameInput.text, "", "");
        streamers.Add(new Streamer(user.getTiktokName(), "", ""));
        StreamerNameInput.text = "";

        addUserToDropdown(user);

        Save();
    }

    private void Save()
    {
        PlayerPrefs.DeleteAll();
        for(int i = 0; i < streamers.Count; i++)
        {
            Streamer user = streamers[i];
            PlayerPrefs.SetString("TiktokUsername_" + (streamers.Count - 1), user.getTiktokName());
            PlayerPrefs.SetInt("StreamerCount", streamers.Count);
        }
        PlayerPrefs.Save();
    }

    public Streamer findStreamer(string username)
    {
        foreach(Streamer user in streamers)
        {
            if(user.getTiktokName().ToLower() == username.ToLower())
            {
                return user;
            }
        }
        return null;
    }

    private void removeStreamer()
    {
        if (SelectStreamerDropdown.value == 0)
        {
            DialogBox.DisplayDialogBox("<color=red>Veuillez selectionner un streamer</color>");

            return;
        }

        Streamer user = findStreamer(SelectStreamerDropdown.options[SelectStreamerDropdown.value].text);

        if (user == null)
        {
            DialogBox.DisplayDialogBox("<color=red>Cette utilisateur n'existe pas</color>");
            return;
        }

        streamers.Remove(user);


        StreamerNameInput.text = "";

        removeUserFromDropdown(user);

        Save();

    }

    private void startLive()
    {
        if(SelectStreamerDropdown.value == 0)
        {
            DialogBox.DisplayDialogBox("<color=red>Vous devez selectionner un streamer!</color>");
            return;
        }

        Streamer user = findStreamer(SelectStreamerDropdown.options[SelectStreamerDropdown.value].text);

        if (user != null)
        {
            GameLogic.instance.initializeLive(user);
        }
    }

    public void onLiveStart()
    {
        UIObject.gameObject.SetActive(false);
        StartCoroutine(startLiveTasks());
    }

    IEnumerator startLiveTasks()
    {
        for (;;)
        {
            yield return new WaitForSeconds(0.1f);

            GameLogic.instance.getWebService().getGifts();
            GameLogic.instance.getWebService().getLiveMessages();
            GameLogic.instance.getWebService().getLikes();
            GameLogic.instance.getWebService().getNewFollowers();
        }
    }


}
