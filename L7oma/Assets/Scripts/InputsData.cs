using UnityEngine;

public class InputsData : MonoBehaviour
{
    public static ActionAsset actionsAsset;

    void Awake()
    {
        actionsAsset = new ActionAsset();
        actionsAsset.Enable();
    }
    
    public static Vector2 MoveDirection()
    {
        if (actionsAsset.Player.Move.IsPressed())
        {
            return actionsAsset.Player.Move.ReadValue<Vector2>();
        }
        return Vector2.zero;
    }
    public static Vector2 TurnDirection()
    {
        return actionsAsset.Player.Turn.ReadValue<Vector2>();
    }
    public static bool IsJumping()
    {
        return actionsAsset.Player.Jump.WasPerformedThisFrame();
    }
    public static bool IsInteracting()
    {
        return actionsAsset.Player.Interact.WasPerformedThisFrame();
    }
    public static bool IsShootingStarted()
    {
        return InputsData.actionsAsset.Player.Test.IsPressed();
    }
    public static bool IsShootingReleased()
    {
        return InputsData.actionsAsset.Player.Test.WasReleasedThisFrame();
    }
    void OnDisable()
    {
        actionsAsset.Disable();
    }
}
