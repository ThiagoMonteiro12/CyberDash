using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.10f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;
    private PlayerManager playerScript;

    [Header("Look Ahead")]
    public float lookAheadDistance = 2f;  // Distância que a câmera olha para frente
    public float lookAheadSmooth = 0.2f;  // Suavidade da transição
    private float currentLookAhead = 0f;
    private float targetLookAhead = 0f;

    void Start()
    {
        if (target != null)
            playerScript = target.GetComponent<PlayerManager>();
    }

    void Update()
    {
        if (target == null) return;

        // Se o player estiver correndo, aplica look ahead baseado na direção
        if (playerScript != null && playerScript.IsRunning)
        {
            targetLookAhead = (playerScript.isFacingRight ? 1f : -1f) * lookAheadDistance;
        }
        else
        {
            targetLookAhead = 0f;
        }

        // Suaviza a interpolação
        currentLookAhead = Mathf.Lerp(currentLookAhead, targetLookAhead, lookAheadSmooth);

        // Calcula posição final
        Vector3 targetPosition = target.position + offset + new Vector3(currentLookAhead, 0f, 0f);

        // Move suavemente
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
