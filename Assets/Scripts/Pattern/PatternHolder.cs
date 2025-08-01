using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PatternHolder", menuName = "Scriptable Objects/PatternHolder")]
public class PatternHolder : ScriptableObject
{
    [field: SerializeField] public List<KeyPattern> Library { get; private set; }

    [Serializable]
    public class KeyPattern
    {
        [field: SerializeField] public string key { get; private set; }
        [field: SerializeField] public Vector2[] patternPositions { get; private set; }

        public KeyPattern(string key, Vector2[] patternPositions)
        {
            this.key = key;
            this.patternPositions = patternPositions;
        }
    }

    public void AddPattern(Vector2[] positions)
    {
        Library.Add(new("", positions));
    }
}
