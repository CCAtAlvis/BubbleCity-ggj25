using UnityEngine;
using System.Collections.Generic;

public class TreeGrowthController : MonoBehaviour
{
    public int TREE_FIRST_THRESHOLD_DURATION = 5;
    public int TREE_SECOND_THRESHOLD_DURATION = 10;
    float startTime;
    public int Level;
    public Sprite TreeSpriteLevel0;
    public Sprite TreeSpriteLevel1;
    public Sprite TreeSpriteLevel2;
    private List<Sprite> TreeSprites;
    private SpriteRenderer spriteRenderer;
    private void UpdateSprite() {        
        spriteRenderer.sprite = TreeSprites[Level];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
        TreeSprites = new() { TreeSpriteLevel0, TreeSpriteLevel1, TreeSpriteLevel2 };
        Level = 0;
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time;
        float difference = currentTime - startTime;
        
        if (difference > TREE_FIRST_THRESHOLD_DURATION)
        {
            Level = 1;
            UpdateSprite();
        }
        if (difference > TREE_SECOND_THRESHOLD_DURATION)
        {
            Level = 2;
            UpdateSprite();
        }
    }
}