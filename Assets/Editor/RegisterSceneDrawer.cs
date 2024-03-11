using System.IO;
using UnityEngine;
using UnityEditor;

/// <summary>
/// RegisterSceneInInspector�N���X��SerializedProperty���g������N���X
/// </summary>
[CustomPropertyDrawer(typeof(RegisterSceneInInspector))]
public class RegisterSceneDrawer : PropertyDrawer
{
    private string[] sceneNames = default;
    private bool isExecuted = false;


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // ����̂ݎ��s
        if (!isExecuted)
        {
            isExecuted = true;

            // �\�����镶���񃊃X�g������������
            UpdateSceneNames();
            // BuildSettings�̃V�[�����X�g�X�V���ɍĂю��s�����悤�A�C�x���g�ɓo�^
            EditorBuildSettings.sceneListChanged += UpdateSceneNames;
        }

        // SerializedPorperty���擾
        var nameProperty = property.FindPropertyRelative("name");
        var seletedIndexProperty = property.FindPropertyRelative("selectedIndex");

        // Inspector�̕\�����g��
        seletedIndexProperty.intValue = EditorGUI.Popup(position, "Next Scene Name", seletedIndexProperty.intValue, sceneNames);
        nameProperty.stringValue = sceneNames[seletedIndexProperty.intValue];
    }

    /// <summary>
    /// �V�[���ꗗ���X�V����
    /// </summary>
    public void UpdateSceneNames()
    {
        sceneNames = new string[EditorBuildSettings.scenes.Length];

        for (int i = 0; i < sceneNames.Length; i++)
        {
            // BuildSettings����V�[���ꗗ���擾���A�p�X���當������擾
            var scene = EditorBuildSettings.scenes[i];
            sceneNames[i] = Path.GetFileNameWithoutExtension(scene.path);
        }
    }
}