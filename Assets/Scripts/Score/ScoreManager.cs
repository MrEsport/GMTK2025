using NaughtyAttributes;
using System.Threading.Tasks;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance { get => instance; }

    [SerializeField, Required] GameStats stats; 
    [SerializeField, Required] ScoreUIHandler uiHandler;

    [SerializeField] int startScore;

    private int score = 0;

    public int Score { get => score; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        score = startScore;
        uiHandler.SetScoreText(score);
    }

    public async Task RegisterPointsScore(SmokePointTarget[] points)
    {
        int patternScore = 0;
        for (int i = 0; i < points.Length; ++i)
        {
            await Task.Delay(50);

            patternScore += ComputePointScore(points[i]);

            uiHandler.SetScoreText(score + patternScore, patternScore);
        }

        score += patternScore;
        uiHandler.HidePopup();
    }

    private int ComputePointScore(SmokePointTarget point)
    {
        DebugExtension.DrawCircle(point.position, .33f, 12, point.isPerfect ? ColorExtension.lime : ColorExtension.darkRed, .25f);
        int scr = point.isPerfect ? stats.Score.perfectBonusScore : 0;
        return scr + Mathf.FloorToInt(Vector2.SqrMagnitude(point.position - point.validatingPoint.position) / Mathf.Pow(stats.Score.smokeValidRange / 2f, 2) * stats.Score.distanceScoreRate);
    }
}
