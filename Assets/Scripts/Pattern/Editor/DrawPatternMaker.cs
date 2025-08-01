using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

/*[CustomEditor(typeof(PatternMaker))]
public class DrawPatternMaker : Editor
{
    const int bezierDivisionCount = 5;

    private void OnSceneGUI()
    {
        PatternMaker source = target as PatternMaker;

        source.startHandle.position = Handles.PositionHandle(source.startHandle.position, Quaternion.identity);
        source.endHandle.position = Handles.PositionHandle(source.endHandle.position, Quaternion.identity);
        source.startTangent.position = Handles.PositionHandle(source.startTangent.position, Quaternion.identity);
        source.endTangent.position = Handles.PositionHandle(source.endTangent.position, Quaternion.identity);

        // Visualize the tangent lines
        Handles.DrawDottedLine(source.startHandle.position, source.startTangent.position, bezierDivisionCount);
        Handles.DrawDottedLine(source.endHandle.position, source.endTangent.position, bezierDivisionCount);

        Handles.DrawBezier(source.startHandle.position, source.endHandle.position, source.startTangent.position, source.endTangent.position, Color.red, null, 5f);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("A"));
        if (GUILayout.Button("DD"));
    }
}*/
