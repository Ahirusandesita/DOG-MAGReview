using System.IO;
using UnityEngine;
using UnityEditor;

/// <summary>
/// RegisterSceneInInspectorクラスのSerializedPropertyを拡張するクラス
/// </summary>
[CustomPropertyDrawer(typeof(RegisterSceneInInspector))]
public class RegisterSceneDrawer : PropertyDrawer
{
    private string[] sceneNames = default;
    private bool isExecuted = false;


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 初回のみ実行
        if (!isExecuted)
        {
            isExecuted = true;

            // 表示する文字列リストを初期化する
            UpdateSceneNames();
            // BuildSettingsのシーンリスト更新時に再び実行されるよう、イベントに登録
            EditorBuildSettings.sceneListChanged += UpdateSceneNames;
        }

        // SerializedPorpertyを取得
        var nameProperty = property.FindPropertyRelative("name");
        var seletedIndexProperty = property.FindPropertyRelative("selectedIndex");

        // Inspectorの表示を拡張
        seletedIndexProperty.intValue = EditorGUI.Popup(position, "Next Scene Name", seletedIndexProperty.intValue, sceneNames);
        nameProperty.stringValue = sceneNames[seletedIndexProperty.intValue];
    }

    /// <summary>
    /// シーン一覧を更新する
    /// </summary>
    public void UpdateSceneNames()
    {
        sceneNames = new string[EditorBuildSettings.scenes.Length];

        for (int i = 0; i < sceneNames.Length; i++)
        {
            // BuildSettingsからシーン一覧を取得し、パスから文字列を取得
            var scene = EditorBuildSettings.scenes[i];
            sceneNames[i] = Path.GetFileNameWithoutExtension(scene.path);
        }
    }
}