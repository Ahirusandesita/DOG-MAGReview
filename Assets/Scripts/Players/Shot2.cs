using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot2 : Shot
{
    protected override void NormalShot(int chargePower)
    {
        if (bulletCountProperty.Value <= 0)
        {
            return;
        }
        bulletCountProperty.Value--;
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z -= 45f;
        PoolObject obj = pool.Get(transform.position, Quaternion.Euler(rotation));

        if (bulletCountProperty.Value <= 0)
        {
            return;
        }
        bulletCountProperty.Value--;
        rotation = transform.rotation.eulerAngles;
        rotation.z += 45f;
        obj = pool.Get(transform.position, Quaternion.Euler(rotation));
        obj.GetComponent<Bullet>().AttackPower = bulletParamater.AttackPower;
    }

    protected override void ChargeShot(int chargePower)
    {
        float z = 45f;
        for (int i = 0; i < chargePower; i += 2)
        {
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.z -= z;
            PoolObject obj = pool.Get(transform.position, Quaternion.Euler(rotation));
            obj.GetComponent<Bullet>().AttackPower = bulletParamater.AttackPower;
            rotation = transform.rotation.eulerAngles;
            rotation.z += z;
            obj = pool.Get(transform.position, Quaternion.Euler(rotation));
            obj.GetComponent<Bullet>().AttackPower = bulletParamater.AttackPower;

            z -= 15f;
        }
    }

    protected override void BulletCharge()
    {
        int bulletCount = bulletCountProperty.Value + 2;
        if (bulletCount > characterParamater.BulletMaxCount)
        {
            bulletCount = characterParamater.BulletMaxCount;
        }

        bulletCountProperty.Value = bulletCount;
    }
}
