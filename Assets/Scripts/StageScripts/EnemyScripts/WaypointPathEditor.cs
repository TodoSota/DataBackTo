using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaypointPath))]
public class WaypointPathEditor : Editor
{
    private readonly Color _colorA = new Color(0.2f, 1f, 0.2f, 0.8f);
    private readonly Color _colorB = new Color(1f, 0.2f, 0.2f, 0.8f);

    protected virtual void OnSceneGUI()
{
    WaypointPath path = (WaypointPath)target;

    // 現在のワールド座標での A と B を取得
    Vector3 currentWorldA = path.GetWorldA();
    Vector3 currentWorldB = path.GetWorldB();

    EditorGUI.BeginChangeCheck();

    // ハンドルで新しい位置を取得
    Vector3 newWorldA = DrawPoint(currentWorldA, "Point A", _colorA);
    Vector3 newWorldB = DrawPoint(currentWorldB, "Point B", _colorB);

    if (EditorGUI.EndChangeCheck())
    {
        Undo.RecordObject(path.transform, "Move Waypoint Path");
        Undo.RecordObject(path, "Update Waypoint Offsets");

        // 真ん中への移動
        Vector3 newCenter = (newWorldA + newWorldB) / 2f;
        path.transform.position = newCenter;

        // pointA と pointB を「新しい本体の位置からの相対距離」に再計算
        path.pointA = newWorldA - newCenter;
        path.pointB = newWorldB - newCenter;

        // シーンを更新
        EditorUtility.SetDirty(path);
    }
}

    // 左右端を描画
    private Vector3 DrawPoint(Vector3 worldPos, string label, Color color)
    {
        Handles.color = color;
        
        GUIStyle style = new GUIStyle();
        style.normal.textColor = color;
        style.fontStyle = FontStyle.Bold;
        Handles.Label(worldPos + Vector3.up * 0.3f, label, style);

        float size = HandleUtility.GetHandleSize(worldPos) * 0.2f;
        
        EditorGUI.BeginChangeCheck();
        int id = GUIUtility.GetControlID(FocusType.Passive);
        var fmh_50_13_639051697652356287 = Quaternion.identity; Vector3 newPos = Handles.FreeMoveHandle(
            id,
            worldPos, 
            size, 
            Vector3.zero, 
            Handles.SphereHandleCap
        );

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Move Waypoint");
            return newPos;
        }
        return worldPos;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("usePath"));
        if (serializedObject.FindProperty("usePath").boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("pointA"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("pointB"));
        }
        serializedObject.ApplyModifiedProperties();
    }
}