using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private CharacterControls characterControls;
    private static InputManager _instance;

    public static InputManager Instance
    {
        get{ return _instance; }
    }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
        characterControls = new CharacterControls();
        Cursor.visible = false;
    }
    private void OnEnable()
    {
        characterControls.Enable();
    }
    private void OnDisable()
    {
        characterControls.Disable();
    }
    public Vector2 GetPlayerMovement()
    {
        return characterControls.Character.Movement.ReadValue<Vector2>();
    }
    public Vector2 GetMouseDelta()
    {
        return characterControls.Character.Look.ReadValue<Vector2>();
    }
    public bool PlayerJumpedThisFrame()
    {
        return characterControls.Character.Jump.triggered;    
    }
    public bool PlayerIntectedThisFrame()
    {
        return characterControls.Character.Interact.triggered;
    }
}
