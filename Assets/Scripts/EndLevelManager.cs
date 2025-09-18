using System.Collections;
using UnityEngine;
using TMPro;

public class EndLevelManager : MonoBehaviour
{
    public static EndLevelManager Instance;

    [Header("UI refs")]
    public GameObject endLevelPanel;
    public TextMeshProUGUI consoleText;
    public TextMeshProUGUI pressEnterText;

    [Header("Config")]
    public float charDelay = 0.03f;
    public float lineDelay = 0.6f;
    public float blinkInterval = 0.6f;

    private bool isEnding = false;
    private float levelStartTime;
    private Coroutine blinkRoutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (endLevelPanel != null) endLevelPanel.SetActive(false);
        if (pressEnterText != null) pressEnterText.enabled = false;

        levelStartTime = Time.time; // marca o tempo de início do nível
    }

    void Update()
    {
        if (!isEnding) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            QuitGame();
        }
    }

    // Chamar isso quando o jogador terminar o nível
    public void TriggerEndLevel()
    {
        if (isEnding) return;
        isEnding = true;

        endLevelPanel.SetActive(true);
        consoleText.text = "";

        Time.timeScale = 0f; // pausa o jogo

        MusicController.Instance.PlayMuffledMusic();

        float elapsed = Time.time - levelStartTime;
        string timeString = $"Level completed in {elapsed:F2} seconds";

        StartCoroutine(PrintSequence(timeString));
    }

    IEnumerator PrintSequence(string timeString)
    {
        yield return StartCoroutine(TypeLine(timeString, "#2ecc71")); // verde
        yield return new WaitForSecondsRealtime(lineDelay);

        yield return StartCoroutine(TypeLine("Thanks For Playing the demo", "#f1c40f")); // amarelo
        yield return new WaitForSecondsRealtime(lineDelay);

        yield return StartCoroutine(TypeLine("The escape continues in the complete version", "#3498db")); // azul
        yield return new WaitForSecondsRealtime(1f);

        pressEnterText.enabled = true;
        blinkRoutine = StartCoroutine(BlinkPressText());
    }

    IEnumerator TypeLine(string text, string hexColor)
    {
        string openTag = $"<color={hexColor}>";
        string closeTag = "</color>";

        for (int i = 0; i < text.Length; i++)
        {
            // Vai adicionando caractere por caractere
            consoleText.text += openTag + text[i] + closeTag;
            yield return new WaitForSecondsRealtime(charDelay);
        }

        consoleText.text += "\n"; // quebra de linha no final
    }

    IEnumerator BlinkPressText()
    {
        while (true)
        {
            pressEnterText.enabled = !pressEnterText.enabled;
            yield return new WaitForSecondsRealtime(blinkInterval);
        }
    }

    void QuitGame()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
