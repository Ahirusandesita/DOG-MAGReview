using System;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputSystemUseObject
{
    private PlayerInput playerInput;

    public InputSystemUseObject()
    {

    }
    public InputSystemUseObject(PlayerInput playerInput)
    {
        this.playerInput = playerInput;
    }


    public virtual void Performed(string actionNameOrId, Action<CallbackContext> action)
    {
        playerInput.actions[actionNameOrId].performed += action;
    }
    public virtual void Started(string actionNameOrId, Action<CallbackContext> action)
    {
        playerInput.actions[actionNameOrId].started += action;
    }
    public virtual void Canceled(string actionNameOrId, Action<CallbackContext> action)
    {
        playerInput.actions[actionNameOrId].canceled += action;
    }

    public virtual void PerformedCancellation(string actionNameOrId, Action<CallbackContext> action)
    {
        playerInput.actions[actionNameOrId].performed -= action;
    }
    public virtual void StartedCancellation(string actionNameOrId, Action<CallbackContext> action)
    {
        playerInput.actions[actionNameOrId].started -= action;
    }
    public virtual void CanceledCancellation(string actionNameOrId, Action<CallbackContext> action)
    {
        playerInput.actions[actionNameOrId].canceled -= action;
    }
}
public class InputSystemUseNullObject : InputSystemUseObject
{
    public override void Performed(string actionNameOrId, Action<CallbackContext> action)
    {

    }
    public override void Started(string actionNameOrId, Action<CallbackContext> action)
    {

    }
    public override void Canceled(string actionNameOrId, Action<CallbackContext> action)
    {

    }
    public override void PerformedCancellation(string actionNameOrId, Action<CallbackContext> action)
    {

    }
    public override void StartedCancellation(string actionNameOrId, Action<CallbackContext> action)
    {

    }
    public override void CanceledCancellation(string actionNameOrId, Action<CallbackContext> action)
    {

    }
}