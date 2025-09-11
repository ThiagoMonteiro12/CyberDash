using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogoAnimacao : MonoBehaviour
{
    [Header("Sprites do Logo")]
    public Sprite logoOff;   // logo apagado
    public Sprite logoOn;    // logo aceso

    [Header("Sons")]
    public AudioClip neonSound;   // som do neon acendendo
    public AudioClip musicaLoop;  // m�sica de fundo em loop

    [Header("Configura��o")]
    public float delayInicial = 0.5f;   // tempo antes da anima��o come�ar
    public float delaySom = 0.3f;       // quanto antes o som toca em rela��o � primeira piscada
    public int quantidadePiscadas = 3;  // quantas vezes pisca
    public Vector2 tempoPiscada = new Vector2(0.05f, 0.15f); // intervalo entre piscadas
    public float delayMusica = 1f;      // tempo depois de acender o neon para come�ar a m�sica

    private Image logoImage;

    private void Start()
    {
        logoImage = GetComponent<Image>();

        // come�a apagado
        if (logoOff != null)
            logoImage.sprite = logoOff;

        // inicia a corrotina da anima��o
        StartCoroutine(AnimarLogo());
    }

    private IEnumerator AnimarLogo()
    {
        // espera antes de tudo
        yield return new WaitForSeconds(delayInicial);

        // toca o som ANTES da primeira piscada
        if (neonSound != null)
            AudioSource.PlayClipAtPoint(neonSound, Camera.main.transform.position);

        // espera um pouquinho para o som come�ar antes da piscada
        yield return new WaitForSeconds(delaySom);

        // piscadas
        for (int i = 0; i < quantidadePiscadas; i++)
        {
            logoImage.sprite = logoOn;
            yield return new WaitForSeconds(Random.Range(tempoPiscada.x, tempoPiscada.y));

            logoImage.sprite = logoOff;
            yield return new WaitForSeconds(Random.Range(tempoPiscada.x, tempoPiscada.y));
        }

        // acende de vez
        logoImage.sprite = logoOn;

        // espera antes de iniciar m�sica
        yield return new WaitForSeconds(delayMusica);

        // inicia m�sica em loop
        if (musicaLoop != null)
        {
            GameObject musicaObj = new GameObject("MusicaLoop");
            AudioSource musicaSource = musicaObj.AddComponent<AudioSource>();
            musicaSource.clip = musicaLoop;
            musicaSource.loop = true;
            musicaSource.Play();
        }
    }
}
