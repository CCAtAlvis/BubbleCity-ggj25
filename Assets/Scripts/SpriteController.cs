using UnityEngine;
using System.Collections.Generic;

public class SpriteController : MonoBehaviour
{
    public int initialGridDiameter = 3;
    public int bufferGridDiameter = 1;
    public float diameterVolumeMultiplier = 0.65f;
    public SpriteRenderer spriteRenderer;

    private int maxGridDiameter = 0;
    private Dictionary<Vector2Int, TileController> coordinateToTile = new Dictionary<Vector2Int, TileController>();

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

        var totalSize = diameter / 2 + 1 + bufferGridDiameter/2;
        Debug.Log("Total size: " + totalSize + " Diameter: " + diameter + " Buffer: " + bufferGridDiameter);
        for (int i = -totalSize; i <= totalSize; i++)
        {
            for (int j = -totalSize; j <= totalSize; j++)
            {
                Vector2Int coord = new Vector2Int(i, j);

                // Skip if we already have a tile at this coordinate
                if (coordinateToTile.ContainsKey(coord))
                {
                    // Update tile active state based on whether it's within bounds
                    bool isWithinBounds = IsWithinGridBounds(i, j, diameter / 2);
                    coordinateToTile[coord].SetWithinDiameter(isWithinBounds);
                    continue;
                }

                var xOffset = 0f;
                if (Mathf.Abs(j) % 2 == 1)
                {
                    xOffset = 0.9f;
                }

                var sprite = Instantiate(spriteRenderer);
                sprite.transform.position = new Vector3(xOffset + i * 1.8f, j * 0.45f, 0);
                sprite.sortingOrder = j * -1;

                // Add TileController component and configure it
                var tileController = sprite.gameObject.AddComponent<TileController>();
                bool isWithinMainGrid = IsWithinGridBounds(i, j, diameter / 2);
                tileController.SetWithinDiameter(isWithinMainGrid);

                // Store in our coordinate mapping
                coordinateToTile[coord] = tileController;
            }
        }
    }

    private bool IsWithinGridBounds(int x, int y, int radius)
    {
        return Mathf.Abs(x) <= radius && Mathf.Abs(y) <= radius;
    }

    public TileController GetTileAt(Vector2Int coordinate)
    {
        return coordinateToTile.TryGetValue(coordinate, out var tile) ? tile : null;
    }
}
