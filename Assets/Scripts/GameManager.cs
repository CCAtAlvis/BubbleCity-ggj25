using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public  GameObject endScreen;
    public  TextMeshProUGUI endScreenText;

    private static GameManager instance;
    public static GameManager GetInstance() => instance;
    public float initialOxygen = 100;

    [Header("Human Controls")]
    public GameObject humanPrefab;
    public float homedOxygenConsumptionRate = 5;
    public float homelessOxygenConsumptionRate = 7;

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
    public float oxygenFromTreeLevel2 = 1000;
    public float oxygenFromTreeLevel3 = 20;
    public float woodFromTreeLevel1 = 1;
    public float woodFromTreeLevel2 = 5;
    public float woodFromTreeLevel3 = 20;

    public Boolean isHoveredGridEmpty = false;

    public Boolean isHoverActive =  false;

    public TileController hoveredGrid;

    // [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bubblesText;
    // public TextMeshProUGUI homelessHumansText;

    private float totalOxygen;
    public float GetCurrentOxygen() => totalOxygen;
    public float UpdateOxygen(float delta) => totalOxygen += delta;
    private List<TreeController> trees = new();
    private List<HomeController> homes = new();
    private List<HumanController> people = new();

    private int totalHumans;
    private int homedHumans = 0;
    private int homelessHumans;

    private float timeSinceLastUpdate;
    private float levelTime;

    // Score is the number of humans that have been homed
    private int score = 0;

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
        // homeController.OnPlaced();
        homeController.AddNewHuman();
        homeController.AddNewHuman();
        RegisterHome(homeController);
    }

    // Update is called once per frame
    void Update()
    {
        if (totalOxygen <= 0)
        {
            endScreen.SetActive(true);
            endScreenText.text = "Score: " + score.ToString();
        }
        // totalOxygen += 1000*Time.deltaTime; 

        var deltaTime = Time.deltaTime;

        // var totalOxygenProduced = 0f;
        // foreach (TreeController tree in trees)
        // {
        //     switch (tree.level)
        //     {
        //         case TreeLevel.SAPLING:
        //             totalOxygenProduced += oxygenFromTreeLevel1;
        //             break;
        //         case TreeLevel.TEEN:
        //             totalOxygenProduced += oxygenFromTreeLevel2;
        //             break;
        //         case TreeLevel.ADULT:
        //             totalOxygenProduced += oxygenFromTreeLevel3;
        //             break;
        //     }
        // }
        // totalOxygenProduced = totalOxygenProduced * deltaTime;

        var totalOxygenConsumed = 0f;
        totalOxygenConsumed += homedHumans * homedOxygenConsumptionRate;
        totalOxygenConsumed += homelessHumans * homelessOxygenConsumptionRate;
        totalOxygenConsumed = totalOxygenConsumed * deltaTime;


        if (homedHumans > score) {
            score = homedHumans;
        }

        scoreText.text = "Score: " + score.ToString();
        bubblesText.text = ((int)totalOxygen).ToString();

        totalOxygen = totalOxygen - totalOxygenConsumed;

        var totalPlace = 0;
        foreach (HomeController home in homes)
        {
            switch (home.level)
            {
                case HomeLevel.SMALL:
                    totalPlace += homeCapacityLevel1;
                    break;
                case HomeLevel.MEDIUM:
                    totalPlace += homeCapacityLevel2;
                    break;
                case HomeLevel.LARGE:
                    totalPlace += homeCapacityLevel3;
                    break;
            }
        }


        homedHumans = people.Where(p => p.state != HumanState.HOMELESS).Count();
        // var remainingPlace = totalPlace - homedHumans;
        // if (remainingPlace > 0)
        // {
        //     homedHumans += remainingPlace;
        //     homelessHumans -= remainingPlace;
        // }
        // else
        // {
        //     homedHumans -= totalPlace;
        //     homelessHumans += totalPlace;
        // }

        timeSinceLastUpdate += Time.deltaTime;
        levelTime += Time.deltaTime;

        CheckTrees();
        CheckHumans();
        CheckHomes();
    }

    public void RegisterBirthForHuman(HumanController newHuman)
    {
        newHuman.birthTime = Time.time;
        people.Add(newHuman);
    }

    public void RegisterTreePlaced(TreeController tree) {
        trees.Add(tree);
        UpdateOxygen(-2);
    }

    public void RegisterHome(HomeController home) {
        homes.Add(home);
        UpdateOxygen(-10);
    }

    // #region Tree Queue Mechanism
    public void CheckTrees()
    {
        var treesRequiringPlantation = trees.Where(t => t.level == TreeLevel.AWAITING_PLANTATION); 
        Debug.Log($"Trees Requiring Plantation: {treesRequiringPlantation.Count()}");
        foreach (var tree in treesRequiringPlantation)
        {
            FindHumanNearestToTree(tree);
        }
    }

    public bool FindHumanNearestToTree(TreeController tree)
    {
        var availableHouses = homes.Where(h => h.occupants.Count > 0 && h.level != HomeLevel.AWAITING_BUILD);
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
        Debug.Log($"Available Humans : {people.Count()}");
        var availableHumans = people.Where(p => p.state == HumanState.HOMELESS || p.state == HumanState.TREE_DONE || p.state == HumanState.HOUSE_DONE);
        foreach (var human in availableHumans) {
            FindHouseNearestToHuman(human);
        }
    }

    public bool FindHouseNearestToHuman(HumanController human) {
        var availableHouses = homes.Where(h => h.occupants.Count < h.homeCapacityForLevel[(int)h.level]);
        if (availableHouses.Count() == 0) {
            Debug.Log($"Human is now: Homeless");
            human.SetState(HumanState.HOMELESS);
            return false;
        }

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

    public void CheckHomes() {
        Debug.Log($"Available Homes : {homes.Count()}");
        var homesForUpgrade = homes.Where(p => p.level == HomeLevel.AWAITING_BUILD || p.level == HomeLevel.AWAITING_UPGRADE || p.level == HomeLevel.BUILDING_WIP || p.level == HomeLevel.UPGRADING_WIP);
        Debug.Log($"Upgrade/Build Candidates : {homes.Count()}");
        foreach (var home in homesForUpgrade) {
            SendNearestHumanToHome(home);
        }
    }

    public bool SendNearestHumanToHome(HomeController home) {
        if (home.occupants.Count > 0) {
            Debug.Log($"Sending Worker from own home");
            home.SendHumanToWork(home, HumanState.MOVING_FOR_HOMEUPGRADE);
            return true;
        }

        var availableHouses = homes.Where(h => h.occupants.Count > 0);
            
        if (availableHouses.Count() == 0) return false;

        var treeVector = new Vector2(home.transform.position.x, home.transform.position.y);

        var availableHousesSortedByDistance = availableHouses.OrderBy(house => {
            var houseVector = new Vector2(house.transform.position.x, house.transform.position.y);
            var distance = (houseVector - treeVector).magnitude;
            return distance;
        });

        var theChosenHouse = availableHousesSortedByDistance.First();
        Debug.Log($"Sending worker from diff house");
        theChosenHouse.SendHumanToWork(home, HumanState.MOVING_FOR_HOMEUPGRADE);
        return true;
    }
}