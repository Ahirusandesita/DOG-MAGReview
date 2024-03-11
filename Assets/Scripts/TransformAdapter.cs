using UnityEngine;
using UnityEngine.Internal;

public class TransformAdapter : MonoBehaviour, IPositionAdapter, IRotationAdapter, ITransformAdapter, IReadOnlyPositionAdapter, IReadOnlyRotationAdapter
{
    public Vector3 Position
    {
        set
        {
            this.transform.position = value;
        }
        get
        {
            return this.transform.position;
        }
    }


    public Quaternion Rotation
    {
        set
        {
            this.transform.rotation = value;
        }
        get
        {
            return this.transform.rotation;
        }
    }

    public void LookAt(Transform target)
    {
        this.transform.LookAt(target);
    }
    public void LookAt(Transform target, [DefaultValue("Vector3.up")] Vector3 worldUp)
    {
        this.transform.LookAt(target, worldUp);
    }
    public void LookAt(Vector3 worldPosition)
    {
        this.transform.LookAt(worldPosition);
    }
    public void LookAt(Vector3 worldPosition, [DefaultValue("Vector3.up")] Vector3 worldUp)
    {
        this.transform.LookAt(worldPosition, worldUp);
    }

    public Transform Transform
    {
        get
        {
            return this.transform;
        }
    }

}
