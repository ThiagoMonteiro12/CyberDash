using UnityEngine;

public class MoveSide : MonoBehaviour
{
    public float speed = 2f;       // Velocidade do movimento
    public float distance = 3f;    // Distância para cada lado
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float x = Mathf.PingPong(Time.time * speed, distance * 2) - distance;
        transform.position = new Vector3(startPos.x + x, startPos.y, startPos.z);
    }
}
