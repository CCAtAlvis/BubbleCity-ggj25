using UnityEngine;

public class BackgroundGridMaker : MonoBehaviour
{
    public GameObject spriteRenderer;
    public int gridSize = 200;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = -gridSize; i < gridSize; i++)
        {
            for (int j = -gridSize; j < gridSize; j++)
            {
                var xOffset = 0f;
                if (Mathf.Abs(j)%2==1) {
                    xOffset = 0.9f;
                }
                var sprite = Instantiate(spriteRenderer,new Vector3(xOffset + i * 1.8f, j*0.45f, j + -1),Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
