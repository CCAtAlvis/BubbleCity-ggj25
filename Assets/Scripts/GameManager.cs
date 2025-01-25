using System.Collections.Generic;
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
    public float oxygenFromTreeLevel1 = 1;
    public float oxygenFromTreeLevel2 = 5;
    public float oxygenFromTreeLevel3 = 20;
    public float woodFromTreeLevel1 = 1;
    public float woodFromTreeLevel2 = 5;
    public float woodFromTreeLevel3 = 20;

    // [Header("UI")]
    // public TextMeshProUGUI oxygenText;
    // public TextMeshProUGUI homedHumansText;
    // public TextMeshProUGUI homelessHumansText;

    private float totalOxygen;
    private List<TreeController> trees = new List<TreeController>();
    private List<HomeController> homes = new List<HomeController>();
    private List<HumanController> people = new List<HumanController>();

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
            Destroy(instance);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalOxygen = initialOxygen;
    }

    // Update is called once per frame
    void Update()
    {
        if (totalOxygen <= 0)
        {
            Debug.Log("Game Over");
            return;
        }

        // DEBUG POPULATION INCREASE
        if (Input.GetMouseButtonDown(0))
        {            
            Vector2 mousePos = Input.mousePosition;
            // set the mousePos variable to the position of the mouse click (screen space)

            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));
            // set my spawn point variable by converting mousePos from screen space into world space
            
            GameObject g = Instantiate(homeFab);
            g.transform.position = point;
            homes.Add(g.GetComponent<HomeController>());
        }

        var deltaTime = Time.deltaTime;

        var totalOxygenProduced = 0f;
        foreach (TreeController tree in trees)
        {
            switch (tree.level)
            {
                case 1:
                    totalOxygenProduced += oxygenFromTreeLevel1;
                    break;
                case 2:
                    totalOxygenProduced += oxygenFromTreeLevel2;
                    break;
                case 3:
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

        // oxygenText.text = "Oxygen: " + totalOxygen.ToString();
        // homedHumansText.text = "Homed Humans: " + homedHumans.ToString();
        // homelessHumansText.text = "Homeless Humans: " + homelessHumans.ToString();
    }

    public void RegisterBirthForHuman(HumanController newHuman)
    {
        newHuman.birthTime = Time.time;
        people.Add(newHuman);
    }
}
