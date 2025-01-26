using System;
using Unity.VisualScripting;
using UnityEngine;

public class TileController : MonoBehaviour
{
    private bool isWithinDiameter;
    public bool IsWithinDiameter => isWithinDiameter;

    public GameObject structureType;
    public GameObject highlighter;

    public float i = 0f;
    public float j = 0f;

    private Color[] HighlightColor = { new Color(0f, 1f, 0f, 0.7f), new Color(1f, 0, 0, 0.7f), new Color(0, 0, 0, 0.3f) };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetWithinDiameter(bool isWithinDiameter)
    {
        this.isWithinDiameter = isWithinDiameter;
        if (!isWithinDiameter)
        {
            highlighter.SetActive(true);
            highlighter.GetComponent<SpriteRenderer>().color = HighlightColor[2];
        }
        else
        {
            highlighter.SetActive(false);
        }
    }


    void OnMouseOver()
    {
        if (GameManager.GetInstance().isHoverActive && isWithinDiameter)
        {
            highlighter.SetActive(true);
            GameManager.GetInstance().isHoveredGridEmpty = structureType.IsUnityNull();
            GameManager.GetInstance().hoveredGrid = this;
            if (structureType.IsUnityNull())
            {
                highlighter.GetComponent<SpriteRenderer>().color = HighlightColor[0];
            }
            else
            {
                highlighter.GetComponent<SpriteRenderer>().color = HighlightColor[1];
            }
        }
    }

    void OnMouseExit()
    {
        if (GameManager.GetInstance().isHoverActive && isWithinDiameter)
        {

            highlighter.SetActive(false);
            GameManager.GetInstance().isHoveredGridEmpty = false;
        }
    }

}
