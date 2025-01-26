using UnityEngine;
using System.Collections.Generic;

public class GridController : MonoBehaviour
{

    private static GridController instance;
    public static GridController GetInstance() => instance;

    public int initialGridDiameter = 5;
    public int bufferGridDiameter = 1;
    public float diameterVolumeMultiplier = 0.65f;
    public GameObject tilePrefab;

    private int maxGridDiameter = 0;
    private Dictionary<Vector2, TileController> coordinateToTile = new Dictionary<Vector2, TileController>();

    private static float gridXOffset = 0.4075f;

    private static float gridYOffset = 0.21f;

    private Vector2 gridXBasis = new Vector2(gridXOffset, -gridYOffset);
    private Vector2 gridYBasis = new Vector2(gridXOffset, gridYOffset);

    private float diameterFloat;

    public float getDiameter() => diameterFloat;

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
        diameterFloat = Mathf.Pow(currentOxygen, 1f / 3) * diameterVolumeMultiplier;
        int diameter = Mathf.FloorToInt(diameterFloat);
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
                // var xOffset = 0f;
                // if (Mathf.Abs(j) % 2 == 1)
                // {
                //     xOffset = 0.9f;
                // }

                // float jFloat = j;
                // float iFloat = i;
                // if (xOffset > 0)
                // {
                //     jFloat = j - 1 + 0.5f;
                //     iFloat = i - 1 + 0.5f;
                // }
                Vector2 coord = i * gridXBasis + j * gridYBasis;


                // Skip if we already have a tile at this coordinate
                if (coordinateToTile.ContainsKey(coord))
                {
                    // Update tile active state based on whether it's within bounds
                    var isWithinBounds = IsWithinGridBounds(i, j, (diameter - 1) / 2);
                    coordinateToTile[coord].SetWithinDiameter(isWithinBounds);
                }
                else
                {
                    var tile = Instantiate(tilePrefab);
                    tile.transform.position = new Vector3(coord.x, coord.y, -1);
                    // tile.sortingOrder = j * -1;

                    // Add TileController component and configure it
                    var tileController = tile.gameObject.GetComponentInChildren<TileController>();
                    bool isWithinMainGrid = IsWithinGridBounds(i, j, (diameter - 1) / 2);
                    tileController.SetWithinDiameter(isWithinMainGrid);

                    // Store in our coordinate mapping
                    coordinateToTile[coord] = tileController;
                }
            }
        }
    }

    // private bool IsWithinGridBounds(float x, float y, int radius)
    // {
    //     return Mathf.Abs(x) <= radius && Mathf.Abs(y) <= radius;
    // }

    private bool IsWithinGridBounds(int i, int j, int radius)
    {
        return Mathf.Sqrt(Mathf.Pow(i,2) + Mathf.Pow(j,2)) <= radius;
    }

    public TileController GetTileAt(Vector2Int coordinate)
    {
        return coordinateToTile.TryGetValue(coordinate, out var tile) ? tile : null;
    }
}
