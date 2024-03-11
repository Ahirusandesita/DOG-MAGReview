using UnityEngine;
using VContainer;

public class TransformAdapterInject : MonoBehaviour
{
    protected IReadOnlyPositionAdapter positionAdapter;
    protected IReadOnlyRotationAdapter rotationAdapter;
    public IReadOnlyPositionAdapter ReadOnlyPositionAdapter
    {
        get
        {
            return positionAdapter;
        }
    }
    [Inject]
    public void Inject(IReadOnlyPositionAdapter positionAdapter,IReadOnlyRotationAdapter rotationAdapter)
    {
        this.positionAdapter = positionAdapter;
        this.rotationAdapter = rotationAdapter;
    }


}
