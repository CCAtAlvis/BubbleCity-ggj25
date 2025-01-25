using UnityEngine;

public class HumanController : MonoBehaviour
{
    public float birthTime;
    public int serialNumber;
    public HomeController assignedHouse;
    public TreeController assignedTree;

    public bool IsHomeLess() => assignedHouse != null;
    public bool IsJobLess() => assignedTree != null;

    public void KickMe() {
        assignedHouse = null;
    }

    public void AssignMe(HomeController newHome) {
        assignedHouse = newHome;
    }

    public void CreateWorkForMe(TreeController newTree) {
        assignedTree = newTree;
    }

    public void StopWorkForMe() {
        assignedTree = null;
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
