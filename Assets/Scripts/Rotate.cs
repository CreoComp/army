using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    void Update()
    {
        transform.Rotate(speed * Time.deltaTime, speed * Time.deltaTime, speed * Time.deltaTime);
    }
}
