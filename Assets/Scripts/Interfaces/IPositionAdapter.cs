using UnityEngine;

public interface IPositionAdapter
{
    Vector3 Position { get; set; }
}
public interface IReadOnlyPositionAdapter
{
    Vector3 Position { get; }
}
public interface IRotationAdapter
{
    Quaternion Rotation { get; set; }
    void LookAt(Transform target);
    void LookAt(Transform target, Vector3 worldUp);
    void LookAt(Vector3 worldPosition);
    void LookAt(Vector3 worldPosition, Vector3 worldUp);
}
public interface IReadOnlyRotationAdapter
{
    Quaternion Rotation { get; }
}
public interface ITransformAdapter
{
    Transform Transform { get; }
}
