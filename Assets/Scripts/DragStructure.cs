using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragStructure : MonoBehaviour  , IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public GameObject gameObject;
    private GameObject newInstance;
    private Camera cam;
    public void OnBeginDrag(PointerEventData eventData)
    {
        GameManager.GetInstance().isHoverActive = true;
        newInstance = Instantiate(gameObject,Input.mousePosition,Quaternion.identity);
        print(newInstance);
        cam = Camera.main;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var pos = cam.ScreenToWorldPoint((Input.mousePosition));
        pos.z = 0;
        newInstance.transform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.GetInstance().isHoverActive = false;
        if(GameManager.GetInstance().isHoveredGridEmpty){
            if(!newInstance.GetComponent<TreeController>().IsUnityNull()){
                newInstance.GetComponent<TreeController>().isPlaced = true;
            }
            if(!newInstance.GetComponent<HomeController>().IsUnityNull()){
                newInstance.GetComponent<HomeController>().isPlaced = true;
            }
            GameManager.GetInstance().hoveredGrid.structureType = newInstance;
        }else{
            Destroy(newInstance);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
