using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    int state = 0;

    public GameObject slide1;
    public GameObject slide2;
    public GameObject slide3;
    public GameObject slide4;
    public GameObject slide5;
    public GameObject slide6;
    public GameObject slide7;
    public GameObject slide8;

    GameObject slideToMove = null;

    private float startTime;
    private float duration = 10f;

    void Update() {
        if (slideToMove != null) {
            float elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            var endPos = new Vector3(0,0,-state*0.1f);
            slideToMove.transform.position = Vector3.Lerp(slideToMove.transform.position, endPos, Mathf.SmoothStep(0f, 1f, t));
        }
    }

    void OnMouseUp()
    {
        var endPos = new Vector3(0,0,(-state)*0.1f);

        if (state == 0) {
            slideToMove = slide1;
            this.transform.position = new Vector3(7.9f,-4f,-1f);
        } else if (state == 1) {
            slideToMove.transform.position = endPos;
            slideToMove = slide2;
        } else if (state == 2) {
            slideToMove.transform.position = endPos;
            slideToMove = slide3;
        } else if (state == 3) {
            slideToMove.transform.position = endPos;
            slideToMove = slide4;
        } else if (state == 4) {
            slideToMove.transform.position = endPos;
            slideToMove = slide5;
        } else if (state == 5) {
            slideToMove.transform.position = endPos;
            slideToMove = slide6;
        } else if (state == 6) {
            slideToMove.transform.position = endPos;
            slideToMove = slide7;
        } else if (state == 7) {
            slideToMove.transform.position = endPos;
            slideToMove = slide8;
        } else {
            SceneManager.LoadScene("Main");
        }

        startTime = Time.time;
        state++;
    }
}
