using UnityEngine;
using System.Collections.Generic;

public class TreeSpawnController : MonoBehaviour
{
    public GameObject Tree;
    public List<GameObject> Trees;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0)) {

            Vector3 point = new Vector3();
            // declare a new variable to be my spawn Point

            Vector2 mousePos = new Vector2();
            // declaring the variable for the mouse click

            mousePos = Input.mousePosition;
            // set the mousePos variable to the position of the mouse click (screen space)

            point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));
            // set my spawn point variable by converting mousePos from screen space into world space
            
            GameObject g = Instantiate(Tree);
            g.transform.position = point;
            Trees.Add(g);
        }        
    }
}
