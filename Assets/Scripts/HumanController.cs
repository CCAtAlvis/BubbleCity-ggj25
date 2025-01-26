using System.IO.Compression;
using UnityEngine;
using UnityEngine.UI;

public enum HumanState
{
    HOMELESS = 0,
    AT_HOME = 1,
    MOVING_BACK_TO_HOUSE = 9,
    MOVING_FOR_PLANTING = 7,
    PLANTING_TREE = 3,
    TREE_DONE = 4,
    BUILDING_HOUSE = 5,
    HOUSE_DONE = 6
}

public class HumanController : MonoBehaviour
{
    public float birthTime;
    public int serialNumber;
    public HomeController assignedHouse;
    public TreeController assignedTree;
    public HumanState state { get; private set; }

    public void KickMe() {
        assignedHouse = null;
        state = HumanState.HOMELESS;
    }

    public void AssignMe(HomeController newHome) {
        assignedHouse = newHome;
        StopWorkForMe();
        state = HumanState.AT_HOME;
    }

    public void CreateWorkForMe(TreeController newTree, HumanState state) {
        assignedTree = newTree;
        assignedTree.SetLevel(TreeLevel.HUMAN_ALLOCATED_FOR_PLANTATION);
        KickMe();
        MoveMe(this.transform.position, newTree.transform.position, state);
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
        Debug.Log("State > " + state.ToString());
        if (state == HumanState.AT_HOME) {
            this.transform.localScale = new Vector3(0, 0, 0);
        } else {
            this.transform.localScale = new Vector3(1, 1, 1);
        }

        var text = this.GetComponent<Text>();
        text.text = $"H: {birthTime}";
    
        if (state == HumanState.HOMELESS) {
            // TODO Move Randomly!
        }

        //#region Human Movement from own position to another (Started Moving, and Stoppage)
        if (state == HumanState.MOVING_FOR_PLANTING)
        {
            float elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.SmoothStep(0f, 1f, t));

            if (elapsedTime >= duration)
            {
                isMoving = false;
                transform.position = endPosition;
                state = HumanState.PLANTING_TREE;
                startTime = Time.time;
                assignedTree.SetLevel(TreeLevel.PLANTATION_IN_PROGRESS);
            }
        }

        if (state == HumanState.PLANTING_TREE)
        {
            float currentTime = Time.time;
            float difference = currentTime - startTime;
            if (difference >= 5) {
                Debug.Log("Planting Done");
                state = HumanState.TREE_DONE;
                assignedTree.SetLevel(TreeLevel.SAPLING);
            }
        }

        if (state == HumanState.MOVING_BACK_TO_HOUSE)
        {
            float elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.SmoothStep(0f, 1f, t));

            if (elapsedTime >= duration)
            {
                isMoving = false;
                transform.position = endPosition;
                state = HumanState.AT_HOME;
            }
        }
    }

    //#region Human Movement from own position to another (Commencing)
    public float duration = 3.0f;
    public Vector3 startPosition;
    public Vector3 endPosition;

    private float startTime;
    private bool isMoving = false;
    public void MoveMe(Vector3 aa, Vector3 zz, HumanState movingState)
    {
        if (movingState != HumanState.MOVING_FOR_PLANTING && movingState != HumanState.MOVING_BACK_TO_HOUSE)
            throw new System.Exception("Wat");
        startPosition = aa;
        endPosition = zz;
        startTime = Time.time;
        state = movingState;
    }
    //#endregion


}
