using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager GetInstance() => instance;
    public float initialOxygen = 100;

    [Header("Human Controls")]
    public GameObject humanPrefab;
    public float homedOxygenConsumptionRate = 100;
    public float homelessOxygenConsumptionRate = 100;

    [Header("Home Controls")]
    public GameObject homeFab;
    public int homeCapacityLevel1 = 10;
    public int homeCapacityLevel2 = 5;
    public int homeCapacityLevel3 = 20;
    public float woodForHomeLevel1 = 1;
    public float woodForHomeLevel2 = 5;
    public float woodForHomeLevel3 = 20;

    [Header("Tree Controls")]
    public GameObject treeFab;
    public float oxygenFromTreeLevel1 = 1;
    public float oxygenFromTreeLevel2 = 5;
    public float oxygenFromTreeLevel3 = 20;
    public float woodFromTreeLevel1 = 1;
    public float woodFromTreeLevel2 = 5;
    public float woodFromTreeLevel3 = 20;

    public Boolean isHoveredGridEmpty = false;

    public Boolean isHoverActive =  false;

    public TileController hoveredGrid;

    // [Header("UI")]
    // public TextMeshProUGUI oxygenText;
    // public TextMeshProUGUI homedHumansText;
    // public TextMeshProUGUI homelessHumansText;

    private float totalOxygen;
    public float GetCurrentOxygen() => totalOxygen;
    private List<TreeController> trees = new();
    private List<HomeController> homes = new();
    private List<HumanController> people = new();

    private int totalHumans;
    private int homedHumans;
    private int homelessHumans;

    private float timeSinceLastUpdate;
    private float levelTime;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        totalOxygen = initialOxygen;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var home = Instantiate(homeFab, new Vector2(0, 0), Quaternion.identity);
        var homeController = home.GetComponent<HomeController>();
        homes.Add(homeController);
        homeController.AddNewHuman();
        homeController.AddNewHuman();
    }

    // Update is called once per frame
    void Update()
    {
        if (totalOxygen <= 0)
        {
            Debug.Log("Game Over");
            return;
        }
        // totalOxygen += 1000*Time.deltaTime; 

        var deltaTime = Time.deltaTime;

        var totalOxygenProduced = 0f;
        foreach (TreeController tree in trees)
        {
            switch (tree.level)
            {
                case TreeLevel.SAPLING:
                    totalOxygenProduced += oxygenFromTreeLevel1;
                    break;
                case TreeLevel.TEEN:
                    totalOxygenProduced += oxygenFromTreeLevel2;
                    break;
                case TreeLevel.ADULT:
                    totalOxygenProduced += oxygenFromTreeLevel3;
                    break;
            }
        }
        totalOxygenProduced = totalOxygenProduced * deltaTime;

        var totalOxygenConsumed = 0f;
        totalOxygenConsumed += homedHumans * homedOxygenConsumptionRate;
        totalOxygenConsumed += homelessHumans * homelessOxygenConsumptionRate;
        totalOxygenConsumed = totalOxygenConsumed * deltaTime;

        totalOxygen += totalOxygenProduced - totalOxygenConsumed;

        var totalPlace = 0;
        foreach (HomeController home in homes)
        {
            if (!home.isActive) continue;
            switch (home.level)
            {
                case 1:
                    totalPlace += homeCapacityLevel1;
                    break;
                case 2:
                    totalPlace += homeCapacityLevel2;
                    break;
                case 3:
                    totalPlace += homeCapacityLevel3;
                    break;
            }
        }

        var remainingPlace = totalPlace - homedHumans;
        if (remainingPlace > 0)
        {
            homedHumans += remainingPlace;
            homelessHumans -= remainingPlace;
        }
        else
        {
            homedHumans -= totalPlace;
            homelessHumans += totalPlace;
        }

        timeSinceLastUpdate += Time.deltaTime;
        levelTime += Time.deltaTime;

        CheckTrees();
        CheckHumans();
        // oxygenText.text = "Oxygen: " + totalOxygen.ToString();
        // homedHumansText.text = "Homed Humans: " + homedHumans.ToString();
        // homelessHumansText.text = "Homeless Humans: " + homelessHumans.ToString();
    }

    public void RegisterBirthForHuman(HumanController newHuman)
    {
        newHuman.birthTime = Time.time;
        people.Add(newHuman);
    }

    public void RegisterTreePlaced(TreeController tree) {
        trees.Add(tree);
    }

    public void RegisteredHomePlaced(HomeController home) {
        homes.Add(home);
    }

    // #region Tree Queue Mechanism
    public void CheckTrees()
    {
        foreach (var tree in trees.Where(t => t.level == TreeLevel.AWAITING_PLANTATION))
        {
            FindHumanNearestToTree(tree);
        }
    }

    public bool FindHumanNearestToTree(TreeController tree)
    {
        var availableHouses = homes.Where(h => h.occupants.Count > 0);
        if (availableHouses.Count() == 0) return false;
        var treeVector = new Vector2(tree.transform.position.x, tree.transform.position.y);

        var availableHousesSortedByDistance = availableHouses.OrderBy(house => {
            var houseVector = new Vector2(house.transform.position.x, house.transform.position.y);
            var distance = (houseVector - treeVector).magnitude;
            return distance;
        });

        var theChosenHouse = availableHousesSortedByDistance.First();
        theChosenHouse.SendHumanToWork(tree, HumanState.MOVING_FOR_PLANTING);
        return true;
    }
    // #endregion

    public void CheckHumans() {
        // Debug.Log("Humans now: " + people.Count());
        foreach (var human in people.Where(p => p.state == HumanState.HOMELESS || p.state == HumanState.TREE_DONE)) {
            Debug.Log("Find House Near to Human: " + human.state.ToString());
            FindHouseNearestToHuman(human);
        }
    }

    public bool FindHouseNearestToHuman(HumanController human) {
                var availableHouses = homes.Where(h => h.occupants.Count > 0);
        if (availableHouses.Count() == 0) return false;
        var treeVector = new Vector2(human.transform.position.x, human.transform.position.y);

        var availableHousesSortedByDistance = availableHouses.OrderBy(house => {
            var houseVector = new Vector2(house.transform.position.x, house.transform.position.y);
            var distance = (houseVector - treeVector).magnitude;
            return distance;
        });

        var theChosenHouse = availableHousesSortedByDistance.First();
        theChosenHouse.AddHuman(human);
        return true;
    }

}