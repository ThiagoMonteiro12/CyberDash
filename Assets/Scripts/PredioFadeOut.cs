using UnityEngine;

public class FadeTransparency : MonoBehaviour
{
    [Header("Configura��es de Transpar�ncia")]
    [Range(0f, 1f)] public float targetAlpha = 0.2f; // qu�o transparente vai ficar
    public float fadeSpeed = 2f;                     // velocidade da transi��o

    private SpriteRenderer sr;
    private float initialAlpha;
    private Coroutine fadeCoroutine;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        initialAlpha = sr.color.a; // guarda a transpar�ncia original
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeTo(targetAlpha));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeTo(initialAlpha));
        }
    }

    private System.Collections.IEnumerator FadeTo(float alphaTarget)
    {
        Color color = sr.color;
        while (Mathf.Abs(color.a - alphaTarget) > 0.01f)
        {
            color.a = Mathf.MoveTowards(color.a, alphaTarget, fadeSpeed * Time.deltaTime);
            sr.color = color;
            yield return null;
        }

        // garante que chegue exatamente no valor final
        color.a = alphaTarget;
        sr.color = color;
    }
}
