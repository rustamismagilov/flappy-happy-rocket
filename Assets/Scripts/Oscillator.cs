using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = Vector3.up;
    [SerializeField] float speed;
    [SerializeField] float distance;

    Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newPos = Mathf.PingPong(Time.time * speed, distance);
        transform.position = startingPosition + movementVector.normalized * newPos;
    }
}
