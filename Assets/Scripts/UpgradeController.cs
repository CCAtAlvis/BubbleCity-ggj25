using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    public float startTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
    }

    private HomeController homeController;
    public void SetHomeController(HomeController home) {
        homeController = home;
    }

    void Update() {
        if (Time.time - startTime > 10)
            Destroy(this);
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        if (homeController != null) {
            homeController.SetLevel(HomeLevel.AWAITING_UPGRADE);
        }
        Destroy(this);
    }
}
