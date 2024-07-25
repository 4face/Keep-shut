using Unity.VisualScripting;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private float distance = 1.0f; // Расстояние перед камерой, на котором будет находиться объект

    private void LateUpdate()
    {
        // Получаем позицию и направление взгляда камеры
        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3 cameraForward = mainCamera.transform.forward;

        // Вычисляем позицию объекта перед камерой
        Vector3 objectPosition = cameraPosition + cameraForward * distance;

        // Устанавливаем позицию объекта
        transform.position = objectPosition;

        // Выравниваем объект так, чтобы он смотрел в сторону камеры
        transform.LookAt(cameraPosition);
    }
}
