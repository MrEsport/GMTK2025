using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "PatternHolder", menuName = "Scriptable Objects/PatternHolder")]
public class PatternHolder : ScriptableObject
{
    [field: SerializeField] public List<KeyPattern> Library { get; private set; }

    public IEnumerable<string> GetKeys()
    {
        return Library.Select(kp => kp.key);
    }

    public Vector2[] this[string key] { get => Library.First(kp => kp.key == key).patternPositions; }

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

#if UNITY_EDITOR
    public bool AddPattern(string name, Vector2[] positions)
    {
        if (Library.Any(kp => kp.key == name))
        {
            Debug.LogError($"Pattern Library already contains a \"{name}\" entry... Change Name");
            return false;
        }
        Library.Add(new(name, positions));
        EditorUtility.SetDirty(this);
        Debug.Log($"\"{name}\" Pattern Succesfully added to Library");
        return true;
    }
#endif
}
