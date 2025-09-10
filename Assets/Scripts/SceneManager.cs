using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class MenuInicial : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Sprites do Bot�o")]
    public Sprite spriteNormal;
    public Sprite spriteHover;

    [Header("Efeito Visual")]
    public float scaleMultiplier = 1.1f;  // Quanto o bot�o cresce
    public float scaleSpeed = 0.2f;       // Velocidade do efeito de scale

    private Image buttonImage;
    private Coroutine flickerRoutine;
    private Coroutine scaleRoutine;
    private Vector3 originalScale;

    private void Start()
    {
        // Pega a refer�ncia do componente Image do bot�o
        buttonImage = GetComponent<Image>();
        if (spriteNormal != null)
            buttonImage.sprite = spriteNormal;

        // Guarda escala original
        originalScale = transform.localScale;
    }

    // Quando o mouse entra no bot�o
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Para piscada anterior se existir
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        flickerRoutine = StartCoroutine(FlickerEffect());

        // Para scale anterior e inicia novo
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);

        scaleRoutine = StartCoroutine(ScaleTo(originalScale * scaleMultiplier));
    }

    // Quando o mouse sai do bot�o
    public void OnPointerExit(PointerEventData eventData)
    {
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        // Volta desligado
        if (spriteNormal != null)
            buttonImage.sprite = spriteNormal;

        // Volta ao tamanho original
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);

        scaleRoutine = StartCoroutine(ScaleTo(originalScale));
    }

    // Corrotina para piscada neon
    private IEnumerator FlickerEffect()
    {
        for (int i = 0; i < 2; i++)
        {
            buttonImage.sprite = spriteHover;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));

            buttonImage.sprite = spriteNormal;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
        }

        buttonImage.sprite = spriteHover; // acende de vez
    }

    // Corrotina de scale suave
    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float time = 0f;

        while (time < 1f)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, time);
            time += Time.deltaTime / scaleSpeed;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    // === A��es do menu ===
    public void IniciarJogo()
    {
        SceneManager.LoadScene("Fase01");
    }

    public void FecharJogo()
    {
        Application.Quit();
        Debug.Log("Fechou o Jogo");
    }
}