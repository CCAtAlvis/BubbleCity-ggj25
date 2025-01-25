using UnityEngine;

public class ToggleShop : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject Shop;
    public GameObject Button;
    public void ToggleShopStatus()
    {
        if(Shop.activeInHierarchy){
            Button.transform.position = new Vector3(Button.transform.position.x, -170, Button.transform.position.z);            
        }else{
            Button.transform.position = new Vector3(Button.transform.position.x, 140, Button.transform.position.z);            
        }
        Shop.SetActive(!Shop.activeInHierarchy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
