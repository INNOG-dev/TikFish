using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TeamFightHUD : GamemodeHUD
{
    private TextMeshProUGUI RedTeamScoreTxt;

    private TextMeshProUGUI BlueTeamScoreTxt;

    private TextMeshProUGUI TimerTxt;


    public TeamFightHUD(GameObject HudObject) : base(HudObject)
    {

    }

    public override void InitHUD()
    {
        RedTeamScoreTxt = hudObject.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        BlueTeamScoreTxt = hudObject.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        TimerTxt = hudObject.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void Update(PartyManager manager)
    {
        float leftTime = manager.getCurrentGameMode().getGameDuration() - manager.getCurrentGameMode().getTime();

        TimeSpan timespan = TimeSpan.FromSeconds(leftTime);

        string resultat = timespan.ToString(@"mm\:ss");

        TimerTxt.text = resultat;

        TeamFight teamFight = (TeamFight) manager.getCurrentGameMode();

        RedTeamScoreTxt.text = "" + teamFight.getScoreTeam("red");
        BlueTeamScoreTxt.text = "" + teamFight.getScoreTeam("blue");

    }
}
