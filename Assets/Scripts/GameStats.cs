using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStats", menuName = "Scriptable Objects/GameStats")]
public class GameStats : ScriptableObject
{
    [field: SerializeField] public PlaneStats Plane { get; private set; }
    [field: SerializeField] public SmokeDropStats SmokeDrop { get; private set; }
    [field: SerializeField] public ScoreStats Score{ get; private set; }

    public float smokeDropDistance { get => Plane.moveSpeed / SmokeDrop.reserveDropPerSecond; }


    [Serializable]
    public class PlaneStats
    {
        [field: SerializeField] public float moveSpeed { get; private set; }
        [field: SerializeField] public float turnSpeedMax { get; private set; }
    }

    [Serializable]
    public class SmokeDropStats
    {
        [field: SerializeField] public float reserveMaxAmount { get; private set; }
        [field: SerializeField] public float reserveDropAmount { get; private set; }
        [field: SerializeField] public float reserveDropPerSecond { get; private set; }
    }

    [Serializable]
    public class ScoreStats
    {
        [field: SerializeField] public float smokeValidRange { get; private set; }
        [field: SerializeField] public float smokePerfectRange { get; private set; }
    }
}
