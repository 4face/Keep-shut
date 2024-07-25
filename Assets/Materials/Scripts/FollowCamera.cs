using Unity.VisualScripting;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private float distance = 1.0f; // ���������� ����� �������, �� ������� ����� ���������� ������

    private void LateUpdate()
    {
        // �������� ������� � ����������� ������� ������
        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3 cameraForward = mainCamera.transform.forward;

        // ��������� ������� ������� ����� �������
        Vector3 objectPosition = cameraPosition + cameraForward * distance;

        // ������������� ������� �������
        transform.position = objectPosition;

        // ����������� ������ ���, ����� �� ������� � ������� ������
        transform.LookAt(cameraPosition);
    }
}
