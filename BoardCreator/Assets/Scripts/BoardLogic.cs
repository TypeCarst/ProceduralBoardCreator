using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLogic : MonoBehaviour
{

    public enum TileType
    {
        SQUARE,
        HEXAGONAL
    }

    public int BoardWidth = 8;
    public int BoardLength = 8;
    public int TileSideLength = 1;
    public TileType type = TileType.SQUARE;
    public GameObject tile;

    private GameObject[,] tiles;

    // Use this for initialization
    void Start()
    {
        tiles = new GameObject[BoardLength, BoardWidth];

        for (int y = 0; y < BoardLength; y++)
        {
            for (int x = 0; x < BoardWidth; x++)
            {
                tiles[y, x] = Instantiate(tile, new Vector3(TileSideLength * x, ((x+y) % 2) * 0.1f, TileSideLength * y), Quaternion.identity, this.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
