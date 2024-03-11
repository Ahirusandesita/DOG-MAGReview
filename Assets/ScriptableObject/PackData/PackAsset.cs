using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PackData", menuName = "ScriptableObjects/CreatePackData")]
public class PackAsset : ScriptableObject
{
    [Space]
    [SerializeField, Min(0f)] private float baseMoveSpeed = 10f;
    [Tooltip("�e�����������Ƃ��̉����{��")]
    [SerializeField, Min(0f)] private float speedupScaleOnHit = 1.2f;
    [Tooltip("�e��AttackPower�ɂ���ď�悹��������{��")]
    [SerializeField, Min(0f)] private float speedupScaleOnBulletPower = 1.1f;
    [Tooltip("�e�����������Ƃ��̉�������")]
    [SerializeField, Min(0f)] private float timeToSpeedupOnHit = 0.5f;
    [Space, Space]
    [Tooltip("���˂ɂ������{��")]
    [SerializeField, Min(0f)] private float speedupScaleOnReflection = 1.1f;
    [Tooltip("���˂ɂ���������������A���ˉ�")]
    [SerializeField, Min(0)] private int reflectionRequiredTimesForSpeedup = 50;
    [Space, Space]
    [SerializeField] private List<ParticleSystem> onGoalParticles = default;

    public float BaseMoveSpeed => baseMoveSpeed;
    public float SpeedupScaleOnHit => speedupScaleOnHit;
    public float SpeedupScaleOnBulletPower => speedupScaleOnBulletPower;
    public float TimeToSpeedupOnHit => timeToSpeedupOnHit;
    public float SpeedupScaleOnReflection => speedupScaleOnReflection;
    public int ReflectionRequiredTimesForSpeedup => reflectionRequiredTimesForSpeedup;
    public IReadOnlyList<ParticleSystem> OnGoalParticles => onGoalParticles;
}
