using UnityEngine;

public class SpriteRandomizer : MonoBehaviour
{
    public Sprite[] spriteList;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpriteRenderer spriteRenderer;
        spriteRenderer = GetComponent<SpriteRenderer>();
        int choice = Random.Range( 0, spriteList.Length);
        spriteRenderer.sprite = spriteList[choice];
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
