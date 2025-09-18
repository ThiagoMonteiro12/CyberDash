using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    [Header("UI refs")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI pressAnyKeyText;
    public TextMeshProUGUI consoleText;

    [Header("Config")]
    public float blinkInterval = 0.6f;
    public float charDelay = 0.03f;     // tempo entre caracteres
    public float lineDelay = 0.6f;      // tempo entre linhas

    private Coroutine blinkRoutine;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (pressAnyKeyText != null)
            pressAnyKeyText.enabled = false;
    }

    void Update()
    {
        if (!isGameOver) return;

        if (Input.anyKeyDown)
        {
            Restart();
        }
    }

    IEnumerator PrintConsoleSequence()
    {
        yield return StartCoroutine(TypeLine("Trying to get access to location...", "#f1c40f")); // amarelo
        yield return new WaitForSecondsRealtime(lineDelay);

        yield return StartCoroutine(TypeLine("Location not Found", "#e74c3c")); // vermelho
        yield return new WaitForSecondsRealtime(lineDelay);

        yield return StartCoroutine(TypeLine("Model 21 Status - Deactivated", "#8e44ad")); // roxo
        yield return new WaitForSecondsRealtime(lineDelay);

        yield return StartCoroutine(TypeLine("GAME OVER", "#ff0000")); // vermelho forte
        yield return new WaitForSecondsRealtime(0.5f);

        // mostra o "[Press Any Key to Restart]"
        pressAnyKeyText.enabled = true;
        blinkRoutine = StartCoroutine(BlinkPressText());
    }

    IEnumerator TypeLine(string text, string hexColor)
    {
        string openTag = $"<color={hexColor}>";
        string closeTag = "</color>";

        string current = "";
        for (int i = 0; i < text.Length; i++)
        {
            current = text.Substring(0, i + 1);
            consoleText.text += openTag + current + closeTag;
            yield return new WaitForSecondsRealtime(charDelay);
            consoleText.text = consoleText.text.Remove(consoleText.text.Length - (openTag.Length + closeTag.Length + current.Length));
        }

        consoleText.text += openTag + text + closeTag + "\n";
    }

    IEnumerator BlinkPressText()
    {
        while (isGameOver)
        {
            pressAnyKeyText.enabled = !pressAnyKeyText.enabled;
            yield return new WaitForSecondsRealtime(blinkInterval);
        }
    }
    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        gameOverPanel.SetActive(true);
        consoleText.text = "";

        Time.timeScale = 0f;

        MusicController.Instance.PlayMuffledMusic();

        // começa a corrotina que imprime as linhas
        StartCoroutine(PrintConsoleSequence());
    }

    public void Restart()
    {
        isGameOver = false;
        if (blinkRoutine != null) StopCoroutine(blinkRoutine);

        Time.timeScale = 1f;

        
            MusicController.Instance.PlayNormalMusic();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
