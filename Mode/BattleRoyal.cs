using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoyal : GameMode
{
    protected override GameMode copy(GameMode gamemode)
    {
        if (gamemode is BattleRoyal)
        {
            BattleRoyal battleRoyal = (BattleRoyal)gamemode;

            time = battleRoyal.time;
        }
        return this;
    }

    public override bool gameFinished()
    {
        return GameLogic.instance.getAlivePlayerCount() == 1 || time >= getGameDuration();
    }

    public override int needPlayerCount()
    {
        return 5;
    }

    public override List<Tuple<string, int>> onGameFinished() 
    {
        List<Fish> fishs = GameLogic.instance.getPlayers();

        List<Fish> winners = fishs.FindAll(x => x.isAlive());

        List<Tuple<string, int>> winnersWithPoint = new List<Tuple<string, int>>();

        foreach(Fish winner in winners)
        {
            winnersWithPoint.Add(new Tuple<string, int>(winner.getUsername(), 1));
        }

        return winnersWithPoint;
    }

    public override void onGameStart()
    {
        base.onGameStart();
    }

    public override void Update()
    {

    }

    public override string getDisplayName()
    {
        return "BattleRoyal";
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
        return false;
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
