using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

public class HomeController : MonoBehaviour
{
    public int level { get; private set; }
    public bool isActive;
    public GameObject humanPrefab;
    public Sprite HouseSpriteLevel0;
    public Sprite HouseSpriteLevel1;
    public Sprite HouseSpriteLevel2;

    private List<Sprite> HouseSprites;
    private SpriteRenderer spriteRenderer;
    private List<HumanController> occupants = new();
    private float lastPolledTime;
    private List<int> homeCapacityForLevel;
    private float timeSinceLastUpdate;
    private float levelTime;
    private int pairs = 0;
    private bool isSingleHumanLeftOutInPairing = false;
    private float humanCreationProbabilityThreshold = 0.9f;

    private void UpdateSprite()
    {
        spriteRenderer.sprite = HouseSprites[level];
    }

    void AddHuman()
    {
        var human1 = Instantiate(humanPrefab);
        var component = human1.GetComponent<HumanController>();
        occupants.Add(component);
        Debug.Log("Occupant Count > " + occupants.Count());
    }

    void IncreaseLevel() {
        level++;
        levelTime = Time.time;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Start");
        level = 0;
        lastPolledTime = Time.time;
        levelTime = Time.time;
        homeCapacityForLevel = new() { GameManager.GetInstance().homeCapacityLevel1, GameManager.GetInstance().homeCapacityLevel2, GameManager.GetInstance().homeCapacityLevel3 };
        HouseSprites = new() { HouseSpriteLevel0, HouseSpriteLevel1, HouseSpriteLevel2 };
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        UpdateSprite();
        AddHuman();
        AddHuman();
    }

    // Update is called once per frame
    void Update()
    {
        RandomProbabilisticStrategy(occupants.Count(), homeCapacityForLevel[level]);
        //OnePairEachStrategy();
    }

    void RandomProbabilisticStrategy(int populationCount, int maxPopulation)
    {
        float currentTime = Time.time;
        timeSinceLastUpdate = currentTime - lastPolledTime;
        if (timeSinceLastUpdate >= 3)
        {
            float elapsedTime = currentTime - levelTime;
            humanCreationProbabilityThreshold = (float)Math.Max(0.3, 0.8 - (0.01 * elapsedTime));
            Debug.Log("Occupant Count > " + occupants.Count());
            timeSinceLastUpdate = 0;
            lastPolledTime = Time.time;

            int pairs = populationCount / 2; // Number of possible pairs

            for (int i = 0; i < pairs; i++)
            {
                if (UnityEngine.Random.insideUnitCircle.magnitude > humanCreationProbabilityThreshold) // 50% chance for reproduction
                {
                    var human = Instantiate(humanPrefab);
                    var humanController = human.GetComponent<HumanController>();

                    if (occupants.Count > homeCapacityForLevel[level])
                    {
                        humanController.KickMe();
                        Debug.Log("Maximum Occupancy, Kicking out 1");
                    }
                    else
                    {
                        humanController.AssignMe(this);
                        occupants.Add(humanController);
                        // if (isSingleHumanLeftOutInPairing && occupants.Count() % 2 == 0 && (Time.time - occupants.Last().birthTime) > 10) {
                        //     pairs++;
                        // }
                        if (isSingleHumanLeftOutInPairing)
                        {
                            pairs += 1;
                            isSingleHumanLeftOutInPairing = false;
                        }
                        else if (occupants.Count() % 2 == 1)
                        {
                            isSingleHumanLeftOutInPairing = true;
                        }
                    }
                    GameManager.GetInstance().RegisterBirthForHuman(humanController);
                }
            }
        }
    }

    void OnePairEachStrategy()
    {
        float currentTime = Time.time;
        timeSinceLastUpdate = currentTime - lastPolledTime;
        if (timeSinceLastUpdate >= 3)
        {
            Debug.Log("Occupant Count > " + occupants.Count());
            timeSinceLastUpdate = 0;
            lastPolledTime = Time.time;
            //var rateOfRepro = (int)Mathf.Pow(1.1f, levelTime);
            if (pairs == 0)
            {
                pairs = occupants.Count() / 2;
            }

            if (pairs-- > 0)
            {
                var human = Instantiate(humanPrefab);
                var humanController = human.GetComponent<HumanController>();
                // Home decides if this new human can belong here
                Debug.Log($"Occupants: {occupants.Count()} | Max: {homeCapacityForLevel[level]}");
                if (occupants.Count > homeCapacityForLevel[level])
                {
                    humanController.KickMe();
                    Debug.Log("Maximum Occupancy, Kicking out 1");
                }
                else
                {
                    humanController.AssignMe(this);
                    occupants.Add(humanController);
                    // if (isSingleHumanLeftOutInPairing && occupants.Count() % 2 == 0 && (Time.time - occupants.Last().birthTime) > 10) {
                    //     pairs++;
                    // }
                    if (isSingleHumanLeftOutInPairing)
                    {
                        pairs += 1;
                        isSingleHumanLeftOutInPairing = false;
                    }
                    else if (occupants.Count() % 2 == 1)
                    {
                        isSingleHumanLeftOutInPairing = true;
                    }
                }
                // Sending ahead for birth registration
                GameManager.GetInstance().RegisterBirthForHuman(humanController);
            }
        }
    }
}
