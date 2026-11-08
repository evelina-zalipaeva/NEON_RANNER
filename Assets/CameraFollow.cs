using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    // Изменено для вида как в Subway Surfers: выше (Y=5.5) и дальше (Z=-8)
    public Vector3 offset = new Vector3(0f, 5.5f, -8f);
    public float smoothSpeed = 10f; // Увеличили для более четкого следования

    [Header("Rotation")]
    public float tiltAngle = 25f; // Увеличили наклон вниз

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 desiredPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(tiltAngle, 0f, 0f);
    }
}