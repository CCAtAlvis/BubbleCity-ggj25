using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class HomeController : MonoBehaviour
{
    public int level;
    public bool isActive;
    public List<HumanController> occupants;
    public float lastPolledTime;
    public List<int> HomeCapacityForLevel = new() { GameManager.GetInstance().homeCapacityLevel1, GameManager.GetInstance().homeCapacityLevel2, GameManager.GetInstance().homeCapacityLevel3 };
    public void KickOccupant()
    {
        var occupant = occupants.Where(occu => occu.IsJobLess()).Last();
        occupant.KickMe();
        occupants.RemoveAt(-1);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        level = 0;
        lastPolledTime = Time.time;
    }
    private float timeSinceLastUpdate;
    private float levelTime;
    public GameObject humanPrefab;

    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time;
        float difference = lastPolledTime - currentTime;
        if (timeSinceLastUpdate > 1)
        {
            timeSinceLastUpdate = 0;
            var newHumans = (int)Mathf.Pow(1.1f, levelTime);
            for (int i = 0; i < newHumans; i++)
            {
                var human = Instantiate(humanPrefab);
                var humanController = human.GetComponent<HumanController>();
                // Home decides if this new human can belong here
                if (occupants.Count > HomeCapacityForLevel[level])
                    humanController.KickMe();
                else
                    humanController.AssignMe(this);
                // Sending ahead for birth registration
                GameManager.GetInstance().RegisterBirthForHuman(humanController);
            }
        }
    }
}
