using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameMode
{
    protected float time;

    public abstract void Update();

    public abstract bool gameFinished();

    public abstract int needPlayerCount();

    public abstract bool playerRespawnAfterDeath();

    public abstract bool deactivateAllBonus();

    public abstract List<int> deactiveBonus();

    public virtual void onGameStart()
    {
        GameLogic gameLogic = GameLogic.instance;

        foreach (Fish fish in gameLogic.getPlayers())
        {
            fish.setPosition(gameLogic.getMapData().getRandomMapPos());
            fish.AddEffect(new Effect(Effects.INVINCIBLE_EFFECT), 5F);
        }
    }

    public abstract List<Tuple<string, int>> onGameFinished();

    public abstract string getDisplayName();

    public abstract bool canFishEatFish(Fish fish, Fish targetFish);

    //-1 if infinite time
    public abstract float getGameDuration();

    public void updateTime()
    {
        time += Time.deltaTime;
    }

    protected abstract GameMode copy(GameMode gamemode);

    public float getTime()
    {
        return time;
    }

    public GameMode Clone()
    {
        return copy(this);
    }

}
