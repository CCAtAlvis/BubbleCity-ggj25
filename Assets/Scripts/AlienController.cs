using UnityEngine;
using System.Collections;

public class AlienController : MonoBehaviour
{
    private Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        this.transform.position = cam.ScreenToWorldPoint(new Vector3(0, Screen.height - Screen.height/10, 10));
        StartMovement();
    }

    IEnumerator StartMovement()
    {
        transform.position = Vector3.Lerp(this.transform.position, cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, this.transform.position.y,this.transform.position.z)), Mathf.SmoothStep(0f, 1f, 20f));
        yield return new WaitForSeconds(10);
        transform.position = Vector3.Lerp(this.transform.position, cam.ScreenToWorldPoint(new Vector3(Screen.width, this.transform.position.y,this.transform.position.z)), Mathf.SmoothStep(0f, 1f, 20f));
    }
    // Update is called once per frame
    void Update()
    {

      
    }
}
