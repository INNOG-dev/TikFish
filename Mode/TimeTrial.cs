using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrial : GameMode, RankingGameMode
{

    private List<Fish> ranking = new List<Fish>();

    private float rankingUpdateTime;


    protected override GameMode copy(GameMode gamemode)
    {
        if (gamemode is TimeTrial)
        {
            TimeTrial battleRoyal = (TimeTrial)gamemode;

            time = battleRoyal.time;

            rankingUpdateTime = updateRankingTime();
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
        List<Fish> fishs = GameLogic.instance.getPlayers();

        List<Tuple<string, int>> winners = new List<Tuple<string, int>>();

        fishs.Sort((fish1, fish2) => fish1.getScore().CompareTo(fish2.getScore()));

        for (int i = 0; i < fishs.Count && i <= 2; i++)
        {
            Fish winner = fishs[fishs.Count - (i + 1)];

            winners.Add(new Tuple<string, int>(winner.getUsername(), 3 - i));
        }

        return winners;
    }

    public override void onGameStart() {

        base.onGameStart();
    }

    public override void Update()
    {
        if((rankingUpdateTime += Time.deltaTime) >= updateRankingTime())
        {
            updateRanking();
            rankingUpdateTime = 0F;
        }
    }

    public override string getDisplayName()
    {
        return "TimeTrial";
    }

    public override bool canFishEatFish(Fish fish, Fish targetFish)
    {
        return true;
    }

    public override float getGameDuration()
    {
        return 60 * 5F;
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

    public int updateRankingTime()
    {
        return 5;
    }

    public void updateRanking()
    {
        ranking = GameLogic.instance.getPlayers();
        
        ranking.Sort((fish1, fish2) => fish2.getScore().CompareTo(fish1.getScore()));
    }

    public List<Fish> getRanking()
    {
        return ranking;
    }
}
