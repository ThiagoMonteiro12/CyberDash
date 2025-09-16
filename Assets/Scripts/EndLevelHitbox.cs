using UnityEngine;

public class EndLevelHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EndLevelManager.Instance.TriggerEndLevel();
        }   
    }
}
