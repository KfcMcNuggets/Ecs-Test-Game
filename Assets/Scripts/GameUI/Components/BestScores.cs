using Leopotam.Ecs;
using TMPro;
using UnityEngine;

struct BestScores
{
    private int _currentBestScores;
    public TextMeshProUGUI bestScoresText;
    public int Scores
    {
        get { return _currentBestScores; }
        set
        {
            _currentBestScores = value;
            bestScoresText.text = _currentBestScores.ToString();
        }
    }
}
