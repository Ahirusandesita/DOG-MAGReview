using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(TeamColorAsset))]
public class TeamColorAssetEditor : Editor
{
    private TeamColorAsset teamColorAsset = default;


    private void OnEnable()
    {
        teamColorAsset = target as TeamColorAsset;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        if (GUILayout.Button("Change Apply"))
        {
            teamColorAsset.ChangeColor();
        }
    }
}
