using UnityEngine;

public class BubbleScaler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private GridController gridController;

    private Transform transform;

    private float growthSpeed = 0.4f;
    void Start()
    {
        gridController = GridController.GetInstance();
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float diameter = gridController.getDiameter();

        if (float.IsNaN(diameter)) 
        {
            return;
        }

        float targetScale = diameter * 0.22f;

        float scale = transform.localScale.x;

        float scaleDiff = targetScale - scale;

        scale += scaleDiff * Time.deltaTime * growthSpeed;

        Debug.Log($"Diameter: {diameter}, scale: {scale}");

        transform.localScale = new Vector3(scale,scale,scale);

    }
}
