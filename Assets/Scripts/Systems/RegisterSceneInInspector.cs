using System;
using UnityEngine;

/// <summary>
/// Inspectorでシーン情報を設定するクラス
/// <br>- 必ず変数をシリアライズ化すること</br>
/// </summary>
[Serializable]
public class RegisterSceneInInspector
{
    // Editorからのみアクセスさせる
    [SerializeField] private string name;
    [SerializeField] private int selectedIndex;

    /// <summary>
    /// シーン名
    /// <br>- BuildSettingsに登録されているシーン名をプルダウンで登録</br>
    /// </summary>
    public string Name => name;
}
