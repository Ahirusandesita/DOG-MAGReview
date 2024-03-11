using System;
using UnityEngine;
using UniRx;
using UnityEditor;


/// <summary>
/// チームごとのカラーデータ
/// </summary>
[CreateAssetMenu(fileName = "ColorData", menuName = "ScriptableObjects/CreateColorData")]
public class TeamColorAsset : ScriptableObject
{
    [Space]
    [SerializeField] private Color characterColor = Color.white;
    [SerializeField] private Color bulletColor = Color.white;
    [SerializeField] private Color packColor = Color.white;
    [SerializeField] private Color stageColor = Color.white;
    [SerializeField] private Color uIColor = Color.white;
    [Space, Space]
    [SerializeField] private Color allColor = Color.white;

    public Color CharacterColor => characterColor;
    public Color BulletColor => bulletColor;
    public Color PackColor => packColor;
    public Color StageColor => stageColor;
    public Color UIColor => uIColor;


    public void ChangeColor()
    {
        characterColor = allColor;
        bulletColor = allColor;
        packColor = allColor;
        stageColor = allColor;
        uIColor = allColor;
    }
}