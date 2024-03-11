using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

public class InGameProgresser : MonoBehaviour
{
    [SerializeField, Min(0f)] private float resultTransitionWaitTime = default;
    [SerializeField] private InputAction pauseInputAction = default;

    private IGameProgressRegistrable gameProgressRegistrable = default;
    private IGameProgressManagementAuxiliaryable gameProgressManagementAuxiliaryable;


    [Inject]
    public void Inject(IGameProgressRegistrable gameProgressRegistrable, IGameProgressManagementAuxiliaryable gameProgressManagementAuxiliaryable)
    {
        this.gameProgressRegistrable = gameProgressRegistrable;
        this.gameProgressManagementAuxiliaryable = gameProgressManagementAuxiliaryable;

        Subscription();
    }

    private void Subscription()
    {
        pauseInputAction.Enable();
        pauseInputAction.performed += _ => gameProgressManagementAuxiliaryable.Pause();

        gameProgressRegistrable.OnFinish += FinishHandler;
    }

    private void FinishHandler(EndEventArgs endEventArgs)
    {
        ResultManager resultManager = FindObjectOfType<ResultManager>();
        // Debug
        resultManager.ResultData = new ResultData();
        resultManager.DisplayResult();
    }
}
