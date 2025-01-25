using System;
using Unity.VisualScripting;
using UnityEngine;

public class TileController : MonoBehaviour
{
    private bool isWithinDiameter;
    public bool IsWithinDiameter => isWithinDiameter;
    public void SetWithinDiameter(bool active) => isWithinDiameter = active;

    public GameObject structureType;
    public GameObject highlighter;

    private UnityEngine.Color[] HighlightColor = {new Color( 0f , 1f , 0f , 0.3f ),new Color( 1f , 0 , 0 , 0.3f )};
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver()
    {
        if(GameManager.GetInstance().isHoverActive){
            highlighter.SetActive(true);
            GameManager.GetInstance().isHoveredGridEmpty = structureType.IsUnityNull();
            GameManager.GetInstance().hoveredGrid = this;
            if(structureType.IsUnityNull()){
                highlighter.GetComponent<SpriteRenderer>().color = HighlightColor[0];
            }else{
                highlighter.GetComponent<SpriteRenderer>().color = HighlightColor[1];
            }
        }
    } 

    void OnMouseExit()
    {
        highlighter.SetActive(false);
    }

}
