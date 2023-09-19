using Leopotam.Ecs;
using UnityEngine;

sealed class ScoreUpdater : IEcsRunSystem, IEcsInitSystem
{
    private EcsEntity scoresCounter;
    private EcsWorld world = null;
    private StaticData staticData;
    private EcsFilter<EndGameMarker> endGameFilter;

    public void Init()
    {
        scoresCounter = world.NewEntity();
        scoresCounter.Get<CurrentScores>().scoresText = staticData.currentScore;
        scoresCounter.Get<BestScores>().bestScoresText = staticData.bestScore;
        scoresCounter.Get<BestScores>().Scores = PlayerPrefs.GetInt("Best");
        scoresCounter.Get<UpdateBestMarker>();
    }

    void IEcsRunSystem.Run()
    {
        if (scoresCounter.Has<UpdateScoresMarker>())
        {
            scoresCounter.Get<CurrentScores>().Scores++;
        }
        if (!endGameFilter.IsEmpty())
        {
            if (scoresCounter.Get<CurrentScores>().Scores > PlayerPrefs.GetInt("Best"))
            {
                PlayerPrefs.SetInt("Best", scoresCounter.Get<CurrentScores>().Scores);
                PlayerPrefs.Save();
            }
        }
    }
}
