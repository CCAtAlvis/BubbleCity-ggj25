using UnityEngine;

public class SpriteController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = -20; i < 20; i++)
        {
            for (int j = -20; j < 20; j++)
            {
                var xOffset = 0f;
                if (Mathf.Abs(j)%2==1) {
                    xOffset = 0.9f;
                }

                var sprite = Instantiate(spriteRenderer);
                sprite.transform.position = new Vector3(xOffset + i * 1.8f, j*0.45f, 0);
                sprite.sortingOrder = j * -1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
