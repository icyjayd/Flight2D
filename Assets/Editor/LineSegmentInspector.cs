using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(LineSegment))]
public class LineSegmentInspector : Editor
{
    private void OnInspectorGui()
    {
        LineSegment lineSegment = target as LineSegment;
        lineSegment.CheckLengths();
        EditorUtility.SetDirty(lineSegment);
    }
}
