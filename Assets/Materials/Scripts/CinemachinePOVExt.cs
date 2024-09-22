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
    [SerializeField] private CharacterControl characterControl;
    private CinemachineVirtualCamera virtualCamera;
    private InputManager inputManager;
    private Vector3 startingRotation;
    private CinemachineBasicMultiChannelPerlin noise;
    protected override void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        inputManager = InputManager.Instance;
        base.Awake();
        if (virtualCamera != null)
        {
            noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (noise == null)
            {
                Debug.LogError("CinemachineBasicMultiChannelPerlin component not found on the virtual camera.");
            }
        }
    }
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float DeltaTime)
    {
        if (vcam.Follow)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                if (!dialogueController.isPlaying)
                {
                    if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
                    Vector2 deltaInput = inputManager.GetMouseDelta();
                    startingRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                    startingRotation.y += deltaInput.y * horizontalSpeed * Time.deltaTime;
                    startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                    state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
                    switch (characterControl.playerStatements)
                    {
                        case 0:
                            Debug.Log(characterControl.playerStatements);
                            noise.m_FrequencyGain = Mathf.Lerp(noise.m_FrequencyGain, 0f, Time.deltaTime / 0.3f);
                            break;
                        case 1:
                            Debug.Log(characterControl.playerStatements);
                            noise.m_FrequencyGain = Mathf.Lerp(noise.m_FrequencyGain, 0.5f, Time.deltaTime / 0.45f);
                            break;
                        case 2:
                            Debug.Log(characterControl.playerStatements);
                            noise.m_FrequencyGain = Mathf.Lerp(noise.m_FrequencyGain, 2.5f, Time.deltaTime / 0.6f);
                            break;
                        default:
                            Debug.LogError(characterControl.playerStatements);
                                break;
                    }
                }
            }
        }
    }
}
