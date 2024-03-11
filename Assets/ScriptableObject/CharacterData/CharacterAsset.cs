using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// キャラクターデータ
/// </summary>
[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CreateCharacterData")]
public class CharacterAsset : ScriptableObject
{
    [Space]
    [SerializeField] private string characterName = default;
    [SerializeField] private GameObject characterPrefab = default;
    [Space, Space]
    [SerializeField] private OutGameData characterUI = default;
    [Space, Space]
    [SerializeField] private CharacterParamater paramater = default;


    public string CharacterName => characterName;
    public GameObject CharacterPrefab => characterPrefab;
    public OutGameData CharacterUI => characterUI;
    public CharacterParamater Paramater => paramater;
}

/// <summary>
/// アウトゲームで使用するデータ
/// </summary>
[Serializable]
public class OutGameData
{
    [SerializeField]
    private Image characterUI = default;

    public Image CharacterUI => characterUI;
}

/// <summary>
/// キャラクターが使用する共通パラメータ
/// </summary>
[Serializable]
public class CharacterParamater
{
    [SerializeField, Min(1)] private int life = 1;
    [SerializeField, Min(1)] private int bulletMaxCount = 5;
    [SerializeField, Min(0f)] private float bulletCooltime = 0.5f;
    [SerializeField, Min(0f)] private float moveSpeed = 7f;
    [SerializeField, Min(0f)] private float chargeTime = 0.4f;
    [SerializeField, Min(0f)] private float deathPenalty = 1.5f;
    [Space]
    [SerializeField] private BulletParamater bulletParamater = default;

    public int Life => life;
    public int BulletMaxCount => bulletMaxCount;
    public float BulletCooltime => bulletCooltime;
    public float MoveSpeed => moveSpeed;
    public float ChargeTime => chargeTime;
    public float DeathPenalty => deathPenalty;
    public BulletParamater BulletParamater => bulletParamater;
}

/// <summary>
/// 弾が使用する共通パラメータ
/// </summary>
[Serializable]
public class BulletParamater
{
    [SerializeField, Min(0f)] private float bulletSpeed = 10f;
    [SerializeField, Min(0f)] private float bulletLifeTime = 1.5f;
    [SerializeField, Min(0)] private int attackPower = 1;


    public float BulletSpeed => bulletSpeed;
    public float BulletLifeTime => bulletLifeTime;
    public int AttackPower => attackPower;
}
