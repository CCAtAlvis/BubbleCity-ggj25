using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using System.Collections;

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
    public Sprite TreeSpriteLevel1Popped;
    public Sprite TreeSpriteLevel2;
    public Sprite TreeSpriteLevel2Popped;
    private List<Sprite> TreeSprites;
    private SpriteRenderer spriteRenderer;
    private bool isPlaced = false;
    private bool isPopped = false;

    private Animator animator;
    private void UpdateSprite()
    {
        if(!isPopped){
            animator.SetInteger("State",1);
        }else{
            animator.SetInteger("State",0);
        }
        
        if (level == TreeLevel.AWAITING_PLANTATION || level == TreeLevel.HUMAN_ALLOCATED_FOR_PLANTATION || level == TreeLevel.PLANTATION_IN_PROGRESS) {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        } else {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public void SetLevel(TreeLevel level)
    {
        if(!animator.IsUnityNull()){
            if (level == TreeLevel.ADULT){  
                animator.SetInteger("Age", 2);
            }
            if (level == TreeLevel.TEEN){
                animator.SetInteger("Age", 1);
            }
        }
        this.level = level;
        startTime = Time.time;
        UpdateSprite();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = this.GetComponent<Animator>();
        startTime = Time.time;
        TreeSprites = new() { TreeSpriteLevel0, TreeSpriteLevel0, TreeSpriteLevel0, TreeSpriteLevel0, TreeSpriteLevel1, TreeSpriteLevel2 };
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaced && (level == TreeLevel.SAPLING || level == TreeLevel.TEEN || level == TreeLevel.ADULT))
        {
            var currentTime = Time.time;
            var difference = currentTime - startTime;

            if ((level == TreeLevel.SAPLING || (level == TreeLevel.TEEN && isPopped == true)) && difference > TREE_FIRST_THRESHOLD_DURATION)
            {
                isPopped = false;
                SetLevel(TreeLevel.TEEN);
            }
            if ((level == TreeLevel.TEEN || (level == TreeLevel.ADULT && isPopped == true)) && difference > TREE_SECOND_THRESHOLD_DURATION)
            {
                isPopped = false;
                SetLevel(TreeLevel.ADULT);
            }
        }
    }

    public void OnPlaced()
    {
        this.SetLevel(TreeLevel.AWAITING_PLANTATION);
        GameManager.GetInstance().RegisterTreePlaced(this);
        isPlaced = true;
    }

    public IEnumerator Pop(){
        if (!isPopped) {
            animator.SetInteger("State",2);
            Debug.LogError("here");
            if (level == TreeLevel.ADULT) {
                Debug.Log("Adult Popped");
                isPopped = true;
                startTime = Time.time;
                yield return new WaitForSeconds(1);
                GameManager.GetInstance().UpdateOxygen(GameManager.GetInstance().oxygenFromTreeLevel3);
                UpdateSprite();
            } else if (level == TreeLevel.TEEN) {
                Debug.Log("Teen Popped");
                
                isPopped = true;
                startTime = Time.time;
                GameManager.GetInstance().UpdateOxygen(GameManager.GetInstance().oxygenFromTreeLevel2);
                yield return new WaitForSeconds(1);
                UpdateSprite();
            } else if (level == TreeLevel.SAPLING) {
                // Nope, nothing here!
            }
        }
    }
    void OnMouseDown()
    {
         StartCoroutine(Pop());
    }
}
