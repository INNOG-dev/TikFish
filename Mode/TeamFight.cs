using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamFight : GameMode
{
    private List<Fish> teamRed = new List<Fish>();

    private List<Fish> teamBlue = new List<Fish>();


    protected override GameMode copy(GameMode gamemode)
    {
        if (gamemode is TeamFight)
        {
            TeamFight battleRoyal = (TeamFight)gamemode;

            time = battleRoyal.time;
        }
        return this;
    }

    public override bool gameFinished()
    {
        return time >= getGameDuration();
    }

    public override int needPlayerCount()
    {
        return 2;
    }

    public override List<Tuple<string, int>> onGameFinished() 
    {
        List<string> winners = new List<string>();

        if (getScoreTeam("red") > getScoreTeam("blue"))
        {
            Debug.Log("Victoire equipe rouge");
            foreach(Fish fish in teamRed)
            {
                winners.Add(fish.getUsername());
            }
        }
        else if(getScoreTeam("red") < getScoreTeam("blue"))
        {
            Debug.Log("Victoire equipe bleu");
            foreach (Fish fish in teamBlue)
            {
                winners.Add(fish.getUsername());
            }
        }
        else 
        {
            Debug.Log("Egalité");
        }

        List<Tuple<string, int>> winnersWithPoint = new List<Tuple<string, int>>();

        foreach (string winner in winners)
        {
            winnersWithPoint.Add(new Tuple<string, int>(winner, 1));
        }

        return winnersWithPoint;
    }

    public int getScoreTeam(string team)
    {
        float score = 0;

        if (team == "red")
        {
            teamRed.ForEach(x => score += x.getScore());
        }
        else
        {
            teamBlue.ForEach(x => score += x.getScore());
        }

        return (int) score;
    }

    public override void onGameStart() 
    {
        base.onGameStart();

        makeTeam();
    }

    private void makeTeam()
    {

        GameLogic gameLogic = GameLogic.instance;

        List<Fish> players = gameLogic.getPlayers();

        int count = players.Count;
        for(int i = 0; i < count; i++)
        {
            Fish fish = players[UnityEngine.Random.Range(0, players.Count)];

            if (i % 2 == 0)
            {
                fish.setTagName("<color=red>" + fish.getUsername() + "</color>");
                teamRed.Add(fish);
            }
            else
            {
                fish.setTagName("<color=#036ffc>" + fish.getUsername() + "</color>");
                teamBlue.Add(fish);
            }

            players.Remove(fish);
        }
    }

    public override void Update()
    {

    }

    public override string getDisplayName()
    {
        return "TeamFight";
    }

    public override bool canFishEatFish(Fish fish, Fish targetFish)
    {
        if (teamRed.Contains(fish) && teamRed.Contains(targetFish)) return false;

        if (teamBlue.Contains(fish) && teamBlue.Contains(targetFish)) return false;

        return true;
    }

    public override float getGameDuration()
    {
        return 60 * 5;
    }

    public override bool playerRespawnAfterDeath()
    {
        return true;
    }

    public override bool deactivateAllBonus()
    {
        return false;
    }

    public override List<int> deactiveBonus()
    {
        return new List<int>();
    }
}
