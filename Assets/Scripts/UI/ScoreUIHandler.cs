using TMPro;
using UnityEngine;

public class ScoreUIHandler : MonoBehaviour
{
    [SerializeField] GameObject addedScorePopup;
    [SerializeField] TMP_Text scoreTextUI;
    [SerializeField] int startScore;

    private void Start()
    {
        SetScoreText(startScore);
    }

    public void SetScoreText(int score)
    {
        scoreTextUI.text = score.ToString();
    }

    public void SetScoreText(int score, int addedScore)
    {
        SetScoreText(score);
        HandleScorePopup(addedScore);
    }

    private void HandleScorePopup(int score)
    {

    }
}
