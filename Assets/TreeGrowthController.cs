using UnityEngine;

public class TreeGrowthController : MonoBehaviour
{
    public int TREE_FIRST_THRESHOLD_DURATION = 5;
    public int TREE_SECOND_THRESHOLD_DURATION = 10;
    float startTime;
    public int Level;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
        Level = 0;
    }

    // Update is called once per frame
    void Update()
    {
        var spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        var currentTime = Time.time;
        var difference = currentTime - startTime;
        if (difference > TREE_FIRST_THRESHOLD_DURATION)
        {
            Level = 1;
            spriteRenderer.color = new Color(0, 0, 1);

        }
        if (difference > TREE_SECOND_THRESHOLD_DURATION)
        {
            Level = 2;
            spriteRenderer.color = new Color(0, 1, 0);
        }
    }
}