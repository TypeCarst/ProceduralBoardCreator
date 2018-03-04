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

    public enum HeightArrangement
    {
        FLAT,
        RANDOM,
        SIMPLEXNOISE,
        VALLEY,
        MAZE,
        RANDOMPATH
    }

    public TileType type = TileType.SQUARE;
    public HeightArrangement arrangement = HeightArrangement.RANDOM;
    public int BoardWidth = 8;
    public int BoardLength = 8;
    public const int MaxBoardWidth = 25;
    public const int MaxBoardLength = 25;
    public int TileSideLength = 1;
    public int TileMaxheight = 8;
    public bool useBorder = true;
    public GameObject tile;
    public GameObject borderTile;

    private GameObject[,] tiles;

    // Use this for initialization
    void Start()
    {
        SetUpGameBoard();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SetNextHeightArrangement();

            DestroyBoard();
            SetUpGameBoard();
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (BoardWidth < MaxBoardWidth && BoardLength < MaxBoardLength)
            {
                BoardWidth++;
                BoardLength++;

                // TODO: keep current layout and add a row and a column

                DestroyBoard();
                SetUpGameBoard();
            }
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (BoardWidth > 1 && BoardLength > 1)
            {
                BoardWidth--;
                BoardLength--;

                // TODO: keep current layout and remove a row and a column

                DestroyBoard();
                SetUpGameBoard();
            }
        }

        // create new board
        if (Input.GetKeyDown(KeyCode.R))
        {
            DestroyBoard();
            SetUpGameBoard();
        }
    }

    public void SetUpGameBoard()
    {
        int[,] heightMap = getHeightArrangement();

        InstantiateBoard(heightMap);
    }

    public void DestroyBoard()
    {
        int w = tiles.GetLength(1);
        int l = tiles.GetLength(0);

        for (int z = 0; z < l; z++)
        {
            for (int x = 0; x < w; x++)
            {
                Destroy(tiles[z, x]);
            }
        }
    }

    #region INSTANTIATION

    #region HEIGHT MAP

    private void SetNextHeightArrangement()
    {
        int enumLength = System.Enum.GetNames(typeof(HeightArrangement)).Length;
        arrangement = (HeightArrangement)(((int)arrangement + 1) % enumLength);
    }

    private int[,] getHeightArrangement()
    {
        switch (arrangement)
        {
            case HeightArrangement.FLAT:
                return generateFlatHeightMap();
            case HeightArrangement.RANDOM:
                return generateRandomHeightMap();
            case HeightArrangement.MAZE:
                //TODO
                break;
            case HeightArrangement.RANDOMPATH:
                //TODO
                break;
            case HeightArrangement.SIMPLEXNOISE:
                //TODO
                break;
            case HeightArrangement.VALLEY:
                //TODO
                break;
        }

        // TODO: remove mockup
        return generateFlatHeightMap();
    }

    /// <summary>
    /// Returns a randomly generated height map for the tiles.
    /// </summary>
    /// <returns>Height values for each tile</returns>
    private int[,] generateFlatHeightMap()
    {
        int[,] heightMap = new int[BoardLength, BoardWidth];

        for (int z = 0; z < BoardLength; z++)
        {
            for (int x = 0; x < BoardWidth; x++)
            {
                heightMap[z, x] = 1;
            }
        }

        return heightMap;
    }

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

    private GameObject[,] InstantiateBoard(int[,] heightMap)
    {
        int length = BoardLength;
        int width = BoardWidth;
        int heightMapOffset = 0;

        if (useBorder)
        {
            length += 2;
            width += 2;
            heightMapOffset = 1;
        }

        // initialize tiles
        tiles = new GameObject[length, width];

        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                switch (type)
                {
                    case TileType.SQUARE:
                        if (useBorder && (z == 0 || z == length - 1 || x == 0 || x == width - 1))
                        {
                            tiles[z, x] = Instantiate(borderTile, new Vector3(TileSideLength * x, 0, TileSideLength * z), Quaternion.identity, this.transform);
                        }
                        else
                        {
                            tiles[z, x] = Instantiate(tile, new Vector3(TileSideLength * x, 0, TileSideLength * z), Quaternion.identity, this.transform);
                            tiles[z, x].GetComponent<TileBehaviour>().ScaleY(0.2f + heightMap[z - heightMapOffset, x - heightMapOffset]);
                        }
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

    public GameObject[,] AddSquareBorderTiles(GameObject[,] tiles)
    {
        GameObject[,] extendedTiles = new GameObject[tiles.GetLength(0) + 2, tiles.GetLength(1) + 2];

        // TODO: add borders
        // tiles[z, x] = Instantiate(borderTile, new Vector3(TileSideLength * x, 0, TileSideLength * z), Quaternion.identity, this.transform);

        return extendedTiles;
    }

    #endregion
}
