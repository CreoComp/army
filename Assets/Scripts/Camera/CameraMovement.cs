using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject target;
    public float smooth = 5.0f;
    public Vector3 offset = new Vector3(0, 2, -5);

    private void Start()
    {
        transform.position = offset;
    }
    void FixedUpdate()
    {
        transform.position = target.transform.position + offset;
       /* transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, Time.fixedDeltaTime * smooth);*/
        transform.LookAt(target.transform.position);
    }
}
