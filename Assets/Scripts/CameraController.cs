using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 8f;
    public float mouseSensitivity = 2f;

    [Header("Modo Normal")]
    public float normalDistance = 25f;
    public float normalHeight = 6f;

    [Header("Modo Guardia")]
    public float guardDistance = 15f;
    public float guardHeight = 3f;

    [Header("Colisión de Cámara")]
    public LayerMask collisionLayers;
    public float cameraRadius = 0.3f;
    public float minDistance = 1.5f;

    [Header("Suavizado de Target")]
    public float targetSmoothSpeed = 5f; // más bajo = más suave, menos rebote

    private float currentYaw = 0f;
    private float currentDistance;
    private float currentHeight;
    private Vector3 smoothedTargetPos; // posición estable, ignora sacudidas de animación

    void Start()
    {
        currentDistance = normalDistance;
        currentHeight = normalHeight;
        smoothedTargetPos = target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Suavizar la posición del target para ignorar movimientos bruscos de animación
        Vector3 targetFlat = new Vector3(target.position.x, smoothedTargetPos.y, target.position.z);
        smoothedTargetPos = Vector3.Lerp(smoothedTargetPos, targetFlat, targetSmoothSpeed * Time.deltaTime);

        if (PlayerController.IsGuard)
        {
            currentDistance = Mathf.Lerp(currentDistance, guardDistance, smoothSpeed * Time.deltaTime);
            currentHeight = Mathf.Lerp(currentHeight, guardHeight, smoothSpeed * Time.deltaTime);

            float targetYaw = Mathf.Atan2(PlayerController.LastMoveDirection.x, PlayerController.LastMoveDirection.z) * Mathf.Rad2Deg;
            currentYaw = targetYaw;

            Quaternion rotation = Quaternion.Euler(0, targetYaw, 0);
            Vector3 desiredPosition = GetDesiredPosition(rotation);

            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.LookAt(smoothedTargetPos + Vector3.up * 1.5f);
        }
        else
        {
            currentDistance = Mathf.Lerp(currentDistance, normalDistance, smoothSpeed * Time.deltaTime);
            currentHeight = Mathf.Lerp(currentHeight, normalHeight, smoothSpeed * Time.deltaTime);

            currentYaw += Input.GetAxis("Mouse X") * mouseSensitivity;

            Quaternion rotation = Quaternion.Euler(0, currentYaw, 0);
            Vector3 desiredPosition = GetDesiredPosition(rotation);

            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.LookAt(smoothedTargetPos + Vector3.up * 1.5f);
        }
    }

    Vector3 GetDesiredPosition(Quaternion rotation)
    {
        Vector3 offset = rotation * new Vector3(0, currentHeight, -currentDistance);
        Vector3 desiredPosition = smoothedTargetPos + offset; // usa posición suavizada

        Vector3 origin = smoothedTargetPos + Vector3.up * 1.5f;
        Vector3 direction = desiredPosition - origin;
        float desiredDist = direction.magnitude;

        if (Physics.SphereCast(origin, cameraRadius, direction.normalized, out RaycastHit hit, desiredDist, collisionLayers))
        {
            float safeDistance = Mathf.Max(hit.distance - cameraRadius, minDistance);
            desiredPosition = origin + direction.normalized * safeDistance;
        }

        return desiredPosition;
    }
}