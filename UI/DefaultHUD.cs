using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DefaultHUD : GamemodeHUD
{
    private TextMeshProUGUI TimerTxt;


    private List<TextMeshProUGUI> ranking;

    private Transform RankingTransform;


    public DefaultHUD(GameObject HudObject) : base(HudObject)
    {
    }

    public override void InitHUD()
    {
        TimerTxt = hudObject.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();

        PartyManager manager = GameLogic.instance.getPartyManager();

        RankingTransform = hudObject.transform.GetChild(1);

        ranking = new List<TextMeshProUGUI>();

        TextMeshProUGUI userRankingTxt = Resources.Load<TextMeshProUGUI>("UI/HUD/Ranking/UserRankingTxt");

        for (int i = 0; i < 5; i++)
        {
            ranking.Add(GameObject.Instantiate(userRankingTxt, RankingTransform));
        }

    }

    public override void Update(PartyManager manager)
    {
        float leftTime = manager.getCurrentGameMode().getGameDuration() - manager.getCurrentGameMode().getTime();

        TimeSpan timespan = TimeSpan.FromSeconds(leftTime);

        string resultat = timespan.ToString(@"mm\:ss");

        TimerTxt.text = resultat;

        if(manager.getCurrentGameMode() is RankingGameMode)
        {
            List<Fish> fishs = ((RankingGameMode)manager.getCurrentGameMode()).getRanking();

          
            for (int i = 0; i < ranking.Count; i++)
            {
                    if(i < fishs.Count)
                    {
                        if(!ranking[i].gameObject.activeInHierarchy)
                        {
                            ranking[i].gameObject.SetActive(true);
                        }
                        ranking[i].text = (i + 1) + "- " + fishs[i].getUsername() + " " + fishs[i].getScore();
                    }
                    else
                    {
                        ranking[i].gameObject.SetActive(false);
                    }
            }
            
        }
        else
        {
            if(RankingTransform.gameObject.activeInHierarchy) RankingTransform.gameObject.SetActive(false);
        }
    }
}