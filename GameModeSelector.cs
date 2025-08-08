using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSelector
{
    public List<GameMode> gameModes = new List<GameMode>();

    public List<GameMode> GetGameModes()
    {
        return gameModes;
    }

    public GameMode selectGameMode()
    {
        List<GameMode> gamemodes = getPlayeableGameModes();

        GameMode mode = gamemodes[Random.Range(0, gamemodes.Count)];
        
        Debug.Log($"selected gamemode : " + mode.GetType().ToString());

        return mode.Clone();
    }

    public List<GameMode> getPlayeableGameModes()
    {
        List<GameMode> gamemodes = new List<GameMode>();
        GameLogic logic = GameLogic.instance;
        foreach(GameMode gamemode in gameModes)
        {
            if(logic.getPlayerCount() >= gamemode.needPlayerCount())
            {
                gamemodes.Add(gamemode);
            }
        }

        return gamemodes;   
    }

    public void registerGameModes()
    {
        gameModes.Add(new BattleRoyal());
        gameModes.Add(new TimeTrial());
        gameModes.Add(new SpeedBlitz());
        gameModes.Add(new TeamFight());
    }
}
