using UnityEngine;
using System.Collections;

public class CutsceneInicial : MonoBehaviour
{
    [Header("Referências")]
    public GameObject player;   // Arraste o objeto Player
    public GameObject cameraObj; // Arraste a Camera

    [Header("Config Cutscene")]
    public float forcaEmpurrar = 5f;
    public float forcaPulo = 8f;
    public float tempoCutscene = 2f;

    private Rigidbody2D rb;
    private Animator animator;
    private PlayerManager playerScript;
    private CameraFollow cameraScript;

    private void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        animator = player.GetComponent<Animator>();

        // Pega os scripts automaticamente
        playerScript = player.GetComponent<PlayerManager>();
        cameraScript = cameraObj.GetComponent<CameraFollow>();

        // Desativa os controles
        if (playerScript != null) playerScript.enabled = false;
        if (cameraScript != null) cameraScript.enabled = false;

        rb.simulated = true;

        StartCoroutine(Cutscene());
    }

    private IEnumerator Cutscene()
    {
        rb.linearVelocity = new Vector2(forcaEmpurrar, forcaPulo);

        if (animator != null)
            animator.SetTrigger("Jump");

        yield return new WaitForSeconds(tempoCutscene);

        rb.linearVelocity = Vector2.zero;

        if (playerScript != null) playerScript.enabled = true;
        if (cameraScript != null) cameraScript.enabled = true;
    }
}
