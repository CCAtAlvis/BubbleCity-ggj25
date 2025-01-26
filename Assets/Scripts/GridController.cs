using UnityEngine;
using System.Collections.Generic;

public class GridController : MonoBehaviour
{

    private static GridController instance;
    public static GridController GetInstance() => instance;

    public int initialGridDiameter = 3;
    public int bufferGridDiameter = 1;
    public float diameterVolumeMultiplier = 0.65f;
    public GameObject tilePrefab;

    private int maxGridDiameter = 0;
    private Dictionary<Vector2, TileController> coordinateToTile = new Dictionary<Vector2, TileController>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setGridSize(initialGridDiameter);
    }

    void Update()
    {
        float currentOxygen = GameManager.GetInstance().GetCurrentOxygen();
        int diameter = Mathf.FloorToInt(Mathf.Pow(currentOxygen, 1f / 3) * diameterVolumeMultiplier);
        if (diameter % 2 == 0)
        {
            return;
        }

        if (diameter > maxGridDiameter)
        {
            setGridSize(diameter);
        }
    }

    public void setGridSize(int diameter)
    {
        maxGridDiameter = diameter;

        var totalSize = (diameter - 1) / 2 + bufferGridDiameter;
        for (int i = -totalSize; i <= totalSize; i++)
        {
            for (int j = -totalSize; j <= totalSize; j++)
            {
                var xOffset = 0f;
                if (Mathf.Abs(j) % 2 == 1)
                {
                    xOffset = 0.9f;
                }

                float jFloat = j;
                float iFloat = i;
                if (xOffset > 0)
                {
                    jFloat = j - 1 + 0.5f;
                    iFloat = i - 1 + 0.5f;
                }
                Vector2 coord = new Vector2(iFloat, jFloat);


                // Skip if we already have a tile at this coordinate
                if (coordinateToTile.ContainsKey(coord))
                {
                    // Update tile active state based on whether it's within bounds
                    bool isWithinBounds = IsWithinGridBounds(iFloat, jFloat, (diameter - 1) / 2);
                    coordinateToTile[coord].SetWithinDiameter(isWithinBounds);
                    continue;
                }

                var tile = Instantiate(tilePrefab);
                tile.transform.position = new Vector3(xOffset + i * 1.8f, j * 0.45f, j * -1);
                // tile.sortingOrder = j * -1;

                // Add TileController component and configure it
                var tileController = tile.gameObject.GetComponentInChildren<TileController>();
                bool isWithinMainGrid = IsWithinGridBounds(iFloat, jFloat, (diameter - 1) / 2);
                tileController.SetWithinDiameter(isWithinMainGrid);

                // Store in our coordinate mapping
                coordinateToTile[coord] = tileController;
            }
        }
    }

    private bool IsWithinGridBounds(float x, float y, int radius)
    {
        return Mathf.Abs(x) <= radius && Mathf.Abs(y) <= radius;
    }

    public TileController GetTileAt(Vector2Int coordinate)
    {
        return coordinateToTile.TryGetValue(coordinate, out var tile) ? tile : null;
    }
}
