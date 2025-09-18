using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance;

    [Header("M�sicas")]
    public AudioClip normalMusic;
    public AudioClip muffledMusic;

    [Header("Configura��o")]
    public float fadeTime = 0.01f;

    private AudioSource normalSource;
    private AudioSource muffledSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Criar 2 AudioSources no mesmo objeto
        normalSource = gameObject.AddComponent<AudioSource>();
        muffledSource = gameObject.AddComponent<AudioSource>();

        // Configura��o b�sica
        normalSource.loop = true;
        muffledSource.loop = true;

        normalSource.clip = normalMusic;
        muffledSource.clip = muffledMusic;

        // Normal come�a tocando
        normalSource.volume = 1f;
        muffledSource.volume = 0f;

        normalSource.Play();
        muffledSource.Play();
    }

    public void PlayNormalMusic()
    {
        StopAllCoroutines();
        StartCoroutine(Fade(muffledSource, normalSource));
    }

    public void PlayMuffledMusic()
    {
        StopAllCoroutines();
        StartCoroutine(Fade(normalSource, muffledSource));
    }

    private System.Collections.IEnumerator Fade(AudioSource from, AudioSource to)
    {
        float t = 1;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float lerp = t / fadeTime;

            from.volume = Mathf.Lerp(1f, 0f, lerp);
            to.volume = Mathf.Lerp(0f, 1f, lerp);

            yield return null;
        }

        from.volume = 0f;
        to.volume = 1f;
    }
}
