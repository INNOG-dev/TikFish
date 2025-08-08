using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBlitz : TimeTrial
{
    protected override GameMode copy(GameMode gamemode)
    {
        if (gamemode is SpeedBlitz)
        {
            SpeedBlitz battleRoyal = (SpeedBlitz)gamemode;

            time = battleRoyal.time;
        }
        return this;
    }

    public override bool gameFinished()
    {
        return base.gameFinished();
    }

    public override int needPlayerCount()
    {
        return 2;
    }

    public override List<Tuple<string, int>> onGameFinished()
    {
       return base.onGameFinished();
    }

    public override void onGameStart()
    {
        base.onGameStart();

        foreach(Fish fish in GameLogic.instance.getPlayers())
        {
            fish.AddEffect(new Effect(Effects.STAR_EFFECT), float.MaxValue);
        }
    }

    public override void Update()
    {
        base.Update();
    }

    public override string getDisplayName()
    {
        return "SpeedBlitz";
    }

    public override bool canFishEatFish(Fish fish, Fish targetFish)
    {
        return true;
    }

    public override float getGameDuration()
    {
        return 60 * 2F;
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
        return new List<int>() { EntityType.ENTITY_STARBONUS };
    }
}
