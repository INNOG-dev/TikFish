using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BattleRoyalHUD : GamemodeHUD
{

    private TextMeshProUGUI AliveFishTxt;

    private TextMeshProUGUI TimerTxt;


    public BattleRoyalHUD(GameObject HudObject) : base(HudObject)
    {

    }

    public override void InitHUD()
    {
        AliveFishTxt = hudObject.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();

        TimerTxt = hudObject.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
   
    }

    public override void Update(PartyManager manager)
    {
        AliveFishTxt.text = GameLogic.instance.getAlivePlayerCount() + "";

        float leftTime = manager.getCurrentGameMode().getGameDuration() - manager.getCurrentGameMode().getTime();

        TimeSpan timespan = TimeSpan.FromSeconds(leftTime);

        string resultat = timespan.ToString(@"mm\:ss");

        TimerTxt.text = resultat;

    }
}
