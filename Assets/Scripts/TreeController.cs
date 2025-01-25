using UnityEngine;
using System.Collections.Generic;

public class TreeController : MonoBehaviour
{
    public int TREE_FIRST_THRESHOLD_DURATION = 5;
    public int TREE_SECOND_THRESHOLD_DURATION = 10;
    float startTime;
    public int level;
    public Sprite TreeSpriteLevel0;
    public Sprite TreeSpriteLevel1;
    public Sprite TreeSpriteLevel2;
    private List<Sprite> TreeSprites;
    private SpriteRenderer spriteRenderer;

    private void UpdateSprite() {
        spriteRenderer.sprite = TreeSprites[level];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
        level = 0;
        TreeSprites = new() { TreeSpriteLevel0, TreeSpriteLevel1, TreeSpriteLevel2 };
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        var currentTime = Time.time;
        var difference = currentTime - startTime;

        if (difference > TREE_FIRST_THRESHOLD_DURATION)
        {
            level = 1;
            UpdateSprite();
        }
        if (difference > TREE_SECOND_THRESHOLD_DURATION)
        {
            level = 2;
            UpdateSprite();
        }
    }
}