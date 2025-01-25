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
    public List<HumanController> occupants = new();
    private float lastPolledTime;
    private List<int> homeCapacityForLevel;
    private float timeSinceLastUpdate;
    private float levelTime;
    private int pairs = 0;
    private bool isSingleHumanLeftOutInPairing = false;
    private float humanCreationProbabilityThreshold = 0.9f;

    public bool isPlaced = false;
    private GameManager gameManager;

    private void UpdateSprite()
    {
        spriteRenderer.sprite = HouseSprites[level];
    }

    public void AddHuman(HumanController human)
    {
        occupants.Add(human);
        human.MoveMe(human.transform.position, this.transform.position, HumanState.MOVING_BACK_TO_HOUSE);
    }

    public void SendHumanToWork(TreeController tree, HumanState state)
    {
        var randomHumanIndex = new System.Random().Next(0, occupants.Count() - 1);
        occupants[randomHumanIndex].CreateWorkForMe(tree, state);
    }

    void IncreaseLevel()
    {
        level++;
        levelTime = Time.time;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.GetInstance();

        level = 0;
        lastPolledTime = Time.time;
        levelTime = Time.time;
        homeCapacityForLevel = new() { gameManager.homeCapacityLevel1, gameManager.homeCapacityLevel2, gameManager.homeCapacityLevel3 };
        HouseSprites = new() { HouseSpriteLevel0, HouseSpriteLevel1, HouseSpriteLevel2 };
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaced)
        {
            RandomProbabilisticStrategy(occupants.Count());
        }
        //OnePairEachStrategy();
    }

    void RandomProbabilisticStrategy(int populationCount)
    {
        float currentTime = Time.time;
        timeSinceLastUpdate = currentTime - lastPolledTime;
        if (timeSinceLastUpdate >= 100)
        {
            float elapsedTime = currentTime - levelTime;
            humanCreationProbabilityThreshold = (float)Math.Max(0.3, 0.8 - (0.01 * elapsedTime));
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
                    gameManager.RegisterBirthForHuman(humanController);
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
                if (occupants.Count > homeCapacityForLevel[level])
                {
                    humanController.KickMe();
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
                gameManager.RegisterBirthForHuman(humanController);
            }
        }
    }
}
