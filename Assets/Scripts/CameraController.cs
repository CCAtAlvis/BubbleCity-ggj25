using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 10f;
    public float zoomSpeed = 10f;

    private Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, speed*Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(0, speed*Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(speed*Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(speed*Time.deltaTime, 0, 0);
        }

        float ScrollWheelChange = Input.GetAxisRaw("Mouse ScrollWheel");
        cam.orthographicSize -= ScrollWheelChange * zoomSpeed;
    }
}
