using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLogic : MonoBehaviour
{
    public enum TileType
    {
        SQUARE,
        HEXAGONAL,
        TRIANGLE
    }

    public TileType type = TileType.SQUARE;
    public int BoardWidth = 8;
    public int BoardLength = 8;
    public int TileSideLength = 1;
    public int TileMaxheight = 8;
    public GameObject tile;

    private GameObject[,] tiles;

    // Use this for initialization
    void Start()
    {
        int[,] heightMap = generateRandomHeightMap();

        InstantiateBoard(heightMap);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region HEIGHT MAP

    /// <summary>
    /// Returns a randomly generated height map for the tiles.
    /// </summary>
    /// <returns>Height values for each tile</returns>
    private int[,] generateRandomHeightMap()
    {
        int[,] heightMap = new int[BoardLength, BoardWidth];

        for (int z = 0; z < BoardLength; z++)
        {
            for (int x = 0; x < BoardWidth; x++)
            {
                heightMap[z, x] = (int)(Random.value * TileMaxheight);
            }
        }

        return heightMap;
    }

    /// <summary>
    /// Returns a height map specified in a file.
    /// </summary>
    /// <param name="path">Path to height map file</param>
    /// <returns>Height map as described by file</returns>
    private int[,] getHeightMapFromFile(string path)
    {
        int[,] heightMap = new int[BoardLength, BoardWidth];

        // TODO: get file from given path

        return heightMap;
    }

    #endregion

    #region INSTANTIATION

    private GameObject[,] InstantiateBoard(int[,] heightMap)
    {
        // initialize tiles
        tiles = new GameObject[BoardLength, BoardWidth];

        for (int z = 0; z < BoardLength; z++)
        {
            for (int x = 0; x < BoardWidth; x++)
            {
                switch (type)
                {
                    case TileType.SQUARE:
                        tiles[z, x] = Instantiate(tile, new Vector3(TileSideLength * x, 0, TileSideLength * z), Quaternion.identity, this.transform);
                        tiles[z, x].GetComponent<TileBehaviour>().ScaleY(0.2f + heightMap[z, x] * 0.2f);
                        break;
                    case TileType.HEXAGONAL:
                        //TODO
                        break;
                    case TileType.TRIANGLE:
                        //TODO
                        break;
                }
            }
        }

        return tiles;
    }

    #endregion
}
