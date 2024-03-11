using UnityEngine;
using VContainer;

public class MoveLogic : IMovable
{
    private IPositionAdapter positionAdapter;
    private IRotationAdapter rotationAdapter;

    [Inject]
    public MoveLogic(IPositionAdapter positionAdapter, IRotationAdapter rotationAdapter)
    {
        this.positionAdapter = positionAdapter;
        this.rotationAdapter = rotationAdapter;
    }

    public void Move(Vector3 moveDir, float moveSpeed)
    {
        if (moveDir == Vector3.zero)
        {
            return;
        }

        positionAdapter.Position += moveDir * moveSpeed * Time.deltaTime;
    }
}
