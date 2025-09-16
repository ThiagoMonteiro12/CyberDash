using UnityEngine;

public class GameOverTester : MonoBehaviour
{
    void Update()
    {
        // Pressione G para simular o Game Over
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameOverManager.Instance.TriggerGameOver();
        }
    }
}
