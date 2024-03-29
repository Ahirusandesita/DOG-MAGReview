using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PackData", menuName = "ScriptableObjects/CreatePackData")]
public class PackAsset : ScriptableObject
{
    [Space]
    [SerializeField, Min(0f)] private float baseMoveSpeed = 10f;
    [Tooltip("弾が当たったときの加速倍率")]
    [SerializeField, Min(0f)] private float speedupScaleOnHit = 1.2f;
    [Tooltip("弾のAttackPowerによって上乗せする加速倍率")]
    [SerializeField, Min(0f)] private float speedupScaleOnBulletPower = 1.1f;
    [Tooltip("弾が当たったときの加速時間")]
    [SerializeField, Min(0f)] private float timeToSpeedupOnHit = 0.5f;
    [Space, Space]
    [Tooltip("反射による加速倍率")]
    [SerializeField, Min(0f)] private float speedupScaleOnReflection = 1.1f;
    [Tooltip("反射による加速が発生する、反射回数")]
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
