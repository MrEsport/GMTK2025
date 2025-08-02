using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance { get => instance; }

    [SerializeField, Required] GameStats stats; 

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public async Task RegisterPointsScore(SmokePointTarget[] points)
    {
        int patternScore = 0;
        for (int i = 0; i < points.Length; ++i)
        {
            await Task.Delay(50);
            patternScore += ComputePointScore(points[i]);
        }

        Debug.Log($"Score: {patternScore} = " + points.ContentToString(p => $"+{ComputePointScore(p)}"));
    }

    private int ComputePointScore(SmokePointTarget point)
    {
        DebugExtension.DrawCircle(point.position, .33f, 12, point.isPerfect ? ColorExtension.lime : ColorExtension.darkRed, .25f);
        int scr = point.isPerfect ? stats.Score.perfectBonusScore : 0;
        return scr + Mathf.FloorToInt(Vector2.SqrMagnitude(point.position - point.validatingPoint.position) / Mathf.Pow(stats.Score.smokeValidRange / 2f, 2) * stats.Score.distanceScoreRate);
    }
}
