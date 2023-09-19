using TMPro;

struct CurrentScores
{
    private int _currentScores;
    public TextMeshProUGUI scoresText;
    public int Scores
    {
        get { return _currentScores; }
        set
        {
            _currentScores = value;
            scoresText.text = _currentScores.ToString();
        }
    }
}
