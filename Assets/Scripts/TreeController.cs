using UnityEngine;
using System.Collections.Generic;
using System;

public enum TreeLevel
{
    AWAITING_PLANTATION = 0,
    HUMAN_ALLOCATED_FOR_PLANTATION = 1,
    PLANTATION_IN_PROGRESS = 2,
    SAPLING = 3,
    TEEN = 4,
    ADULT = 5
}
public class TreeController : MonoBehaviour
{
    public int TREE_FIRST_THRESHOLD_DURATION = 5;
    public int TREE_SECOND_THRESHOLD_DURATION = 10;
    float startTime;
    public TreeLevel level { get; private set; }
    public Sprite TreeSpriteLevel0;
    public Sprite TreeSpriteLevel1;
    public Sprite TreeSpriteLevel2;
    private List<Sprite> TreeSprites;
    private SpriteRenderer spriteRenderer;
    public Boolean isPlaced = false;
    private void UpdateSprite()
    {
        spriteRenderer.sprite = TreeSprites[(int)level];
    }
    public void SetLevel(TreeLevel level)
    {
        this.level = level;
        UpdateSprite();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
        TreeSprites = new() { TreeSpriteLevel0, TreeSpriteLevel0, TreeSpriteLevel0, TreeSpriteLevel0, TreeSpriteLevel1, TreeSpriteLevel2 };
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        this.SetLevel(TreeLevel.AWAITING_PLANTATION);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaced && (level == TreeLevel.SAPLING || level == TreeLevel.TEEN || level == TreeLevel.ADULT))
        {
            var currentTime = Time.time;
            var difference = currentTime - startTime;

            if (difference > TREE_FIRST_THRESHOLD_DURATION)
            {
                SetLevel(TreeLevel.TEEN);
            }
            if (difference > TREE_SECOND_THRESHOLD_DURATION)
            {
                SetLevel(TreeLevel.ADULT);
            }
        }
    }
}