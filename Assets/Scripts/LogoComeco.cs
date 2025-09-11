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
    public AudioClip musicaLoop;  // música de fundo em loop

    [Header("Configuração")]
    public float delayInicial = 0.5f;   // tempo antes da animação começar
    public float delaySom = 0.3f;       // quanto antes o som toca em relação à primeira piscada
    public int quantidadePiscadas = 3;  // quantas vezes pisca
    public Vector2 tempoPiscada = new Vector2(0.05f, 0.15f); // intervalo entre piscadas
    public float delayMusica = 1f;      // tempo depois de acender o neon para começar a música

    private Image logoImage;

    private void Start()
    {
        logoImage = GetComponent<Image>();

        // começa apagado
        if (logoOff != null)
            logoImage.sprite = logoOff;

        // inicia a corrotina da animação
        StartCoroutine(AnimarLogo());
    }

    private IEnumerator AnimarLogo()
    {
        // espera antes de tudo
        yield return new WaitForSeconds(delayInicial);

        // toca o som ANTES da primeira piscada
        if (neonSound != null)
            AudioSource.PlayClipAtPoint(neonSound, Camera.main.transform.position);

        // espera um pouquinho para o som começar antes da piscada
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

        // espera antes de iniciar música
        yield return new WaitForSeconds(delayMusica);

        // inicia música em loop
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
