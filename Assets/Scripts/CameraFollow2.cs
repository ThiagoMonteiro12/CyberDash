using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Transform target;       // Player
    private PlayerManager playerScript;              // Script do player
    public EdgeCollider2D mapBounds;                 // Collider do mapa

    [Header("Configuração de câmera")]
    private Vector3 velocity = Vector3.zero;
    public Vector3 offset = new Vector3(0f, 0f, -20f);
    public float smoothTime = 0.10f;

    private float minX, maxX, minY, maxY;
    private float halfWidth, halfHeight;

    [Header("Look Ahead")]
    public float lookAheadDistance = 2f;  // Distância que a câmera olha para frente
    public float lookAheadSmooth = 0.2f;  // Suavidade da transição
    private float currentLookAhead = 0f;
    private float targetLookAhead = 0f;

    void Start()
    {
        if (target != null)
            playerScript = target.GetComponent<PlayerManager>();

        // Calcula metade do tamanho da câmera
        Camera cam = Camera.main;
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;

        // Pega os limites do collider do mapa
        if (mapBounds != null)
        {
            Bounds bounds = mapBounds.bounds;
            minX = bounds.min.x + halfWidth;
            maxX = bounds.max.x - halfWidth;
            minY = bounds.min.y + halfHeight;
            maxY = bounds.max.y - halfHeight;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // ==== LOOK AHEAD ====
        if (playerScript != null && playerScript.IsRunning)
        {
            targetLookAhead = (playerScript.isFacingRight ? 1f : -1f) * lookAheadDistance;
        }
        else
        {
            targetLookAhead = 0f;
        }

        currentLookAhead = Mathf.Lerp(currentLookAhead, targetLookAhead, lookAheadSmooth);

        // ==== POSIÇÃO ALVO ====
        Vector3 targetPosition = target.position + offset + new Vector3(currentLookAhead, 0f, 0f);

        // Suaviza o movimento da câmera
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // ==== LIMITES DO MAPA ====
        float clampedX = Mathf.Clamp(smoothPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(smoothPos.y, minY, maxY);

        // Aplica a posição final
        transform.position = new Vector3(clampedX, clampedY, smoothPos.z);
    }
}
