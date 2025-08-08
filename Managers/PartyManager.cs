using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager
{
    private static readonly int WAITING_TIME = 10; //seconds

    public static readonly int SELECTION_TIME = 10; //seconds

    public static readonly int START_TIME = 3; //seconds

    public static readonly int RESTART_TIME = 10; //seconds


    [SerializeField]
    private GameMode currentGamemode;

    private GameModeSelector selector = new GameModeSelector();

    private GameState currentState = GameState.FINISHED;

    private float timer;

    private float startTimer;

    private bool canStart = false;

    public PartyManager()
    {
        selector.registerGameModes();
    }

    public GameState getGameState()
    {
        return currentState;
    }

    public void startParty()
    {
        if (currentState == GameState.FINISHED)
        {
            GameLogic.instance.addBots();
            currentState = GameState.WAITING;
            timer = WAITING_TIME;
        }
    }

    public void Update()
    {
        if(currentState == GameState.STARTED)
        {
            
            currentGamemode.Update();
            currentGamemode.updateTime();

            timer += Time.deltaTime;
            if(timer >= 2)
            {
                timer = 0;

                if (currentGamemode.playerRespawnAfterDeath())
                {
                    foreach (Fish fish in GameLogic.instance.getPlayers())
                    {
                        if (fish.isAlive()) continue;

                       
                        fish.setScore(fish.getScore() / 2);
                        fish.setDead(false);

                        DelayedAction.InvokeDelayed(delegate
                        {
                            fish.setPosition(GameLogic.instance.getMapData().getRandomMapPos());

                            fish.GetGameObject().SetActive(true);

                            fish.setEatOffCooldown(5F);
                            fish.AddEffect(new Effect(Effects.INVINCIBLE_EFFECT), 5F);
                        }, 5F);
                    }
                }
            }
                   
            if(currentGamemode.gameFinished())
            {
                finishGame();
            }

            //Debug.Log(currentGamemode.getGameDuration() - currentGamemode.getTime() + " s");
        }
        else if(currentState == GameState.WAITING)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if(selector.getPlayeableGameModes().Count > 0)
                {
                    timer = SELECTION_TIME;
                    startTimer = START_TIME;
                    currentGamemode = selector.selectGameMode();
                    currentState = GameState.STARTING;
                }
                else
                {
                    timer = SELECTION_TIME;
                }
            }
        }
        else if(currentState == GameState.STARTING)
        {
            timer -= Time.deltaTime;
            if(timer <= 0 && canStart)
            {
                startTimer -= Time.deltaTime;
                if (startTimer <= 0)
                {
                    currentState = GameState.STARTED;
                    currentGamemode.onGameStart();
                }
            }
        }
        else if(currentState == GameState.FINISHED)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                startParty();
            }
        }
    }

    private void finishGame()
    {
        List<Tuple<string, int>> winners = currentGamemode.onGameFinished();

        string endTxt = "Partie terminé \n\nGagnants :\n\n";

        foreach(Tuple<string, int> winner in winners)
        {
            endTxt += winner.Item1 + " (+" + winner.Item2 + ")" + ", ";
        }

        endTxt = endTxt.Trim(' ');
        endTxt = endTxt.Trim(',');

        GameHUD.HUD.displayEndTxt(endTxt);

        GameLogic.instance.getWebService().addPoints(winners);

        timer = RESTART_TIME;

        startTimer = 0F;

        canStart = false;

        currentState = GameState.FINISHED;

        GameLogic.instance.clearMap();
    }

    public float getWaitingTimer()
    {
        return timer;
    }

    public float getSelectionTimer()
    {
        return timer;
    }

    public float getStartTimer()
    {
        return startTimer;
    }

    public GameModeSelector GetGameModeSelector()
    {
        return selector;
    }

    public GameMode getCurrentGameMode()
    {
        return currentGamemode;
    }

    public void setReadyToStart()
    {
        canStart = true;
    }

    public bool getCanStart()
    {
        return canStart;
    }

    

}
