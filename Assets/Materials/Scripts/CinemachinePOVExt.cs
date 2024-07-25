using Cinemachine;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class CinemachinePOVExt : CinemachineExtension
{
    [SerializeField] private float clampAngle = 80f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private DialogueController dialogueController;
    private InputManager inputManager;
    private Vector3 startingRotation;
    protected override void Awake()
    {
        inputManager = InputManager.Instance;
        base.Awake();

    }
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float DeltaTime)
    {
        if (vcam.Follow)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                if (!dialogueController.InProgress)
                {
                    if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
                    Vector2 deltaInput = inputManager.GetMouseDelta();
                    startingRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                    startingRotation.y += deltaInput.y * horizontalSpeed * Time.deltaTime;
                    startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                    state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f); 
                }
            }
        }
    }
}
