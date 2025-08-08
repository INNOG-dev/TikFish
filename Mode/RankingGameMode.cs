using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface RankingGameMode 
{

    public abstract void updateRanking();

    public abstract int updateRankingTime();

    public abstract List<Fish> getRanking();
}
