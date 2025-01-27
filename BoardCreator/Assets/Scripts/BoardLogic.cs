using UnityEngine;
using UnityEngine.Serialization;

public class BoardLogic : MonoBehaviour
{
    private enum TileType
    {
        Square,
        Hexagonal,
        Triangle
    }

    private enum HeightArrangement
    {
        Flat,
        Random,
        SimplexNoise,
        Valley,
        Maze,
        RandomPath
    }

    private const int MAX_BOARD_WIDTH = 25;
    private const int MAX_BOARD_LENGTH = 25;

    #region Serialized fields

    [SerializeField]
    private TileType _type = TileType.Square;
    
    [SerializeField]
    private HeightArrangement _arrangement = HeightArrangement.Random;

    [SerializeField]
    private int _boardWidth = 8;
    
    [SerializeField]
    private int _boardLength = 8;

    [SerializeField]
    private int _tileSideLength = 1;
    
    [SerializeField]
    private int _tileMaxHeight = 8;
    
    [SerializeField]
    private bool _useBorder = true;
    
    [SerializeField]
    private GameObject _tile;

    [SerializeField]
    private GameObject _borderTile;

    #endregion Serialized fields

    private GameObject[,] _tiles;

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
            if (_boardWidth < MAX_BOARD_WIDTH && _boardLength < MAX_BOARD_LENGTH)
            {
                _boardWidth++;
                _boardLength++;

                // TODO: keep current layout and add a row and a column

                DestroyBoard();
                SetUpGameBoard();
            }
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (_boardWidth > 1 && _boardLength > 1)
            {
                _boardWidth--;
                _boardLength--;

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
        int[,] heightMap = GetHeightArrangement();

        InstantiateBoard(heightMap);
    }

    public void DestroyBoard()
    {
        // TODO: pooling
        int w = _tiles.GetLength(1);
        int l = _tiles.GetLength(0);

        for (int z = 0; z < l; z++)
        {
            for (int x = 0; x < w; x++)
            {
                Destroy(_tiles[z, x]);
            }
        }
    }

    #region INSTANTIATION

    #region HEIGHT MAP

    private void SetNextHeightArrangement()
    {
        int enumLength = System.Enum.GetNames(typeof(HeightArrangement)).Length;
        _arrangement = (HeightArrangement)(((int)_arrangement + 1) % enumLength);
    }

    private int[,] GetHeightArrangement()
    {
        switch (_arrangement)
        {
            case HeightArrangement.Flat:
                return GenerateFlatHeightMap();
            case HeightArrangement.Random:
                return GenerateRandomHeightMap();
            case HeightArrangement.Maze:
                //TODO
                break;
            case HeightArrangement.RandomPath:
                //TODO
                break;
            case HeightArrangement.SimplexNoise:
                //TODO
                break;
            case HeightArrangement.Valley:
                //TODO
                break;
        }

        // TODO: remove mockup
        return GenerateFlatHeightMap();
    }

    /// <summary>
    /// Returns a randomly generated height map for the tiles.
    /// </summary>
    /// <returns>Height values for each tile</returns>
    private int[,] GenerateFlatHeightMap()
    {
        int[,] heightMap = new int[_boardLength, _boardWidth];

        for (int z = 0; z < _boardLength; z++)
        {
            for (int x = 0; x < _boardWidth; x++)
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
    private int[,] GenerateRandomHeightMap()
    {
        int[,] heightMap = new int[_boardLength, _boardWidth];

        for (int z = 0; z < _boardLength; z++)
        {
            for (int x = 0; x < _boardWidth; x++)
            {
                heightMap[z, x] = (int)(Random.value * _tileMaxHeight);
            }
        }

        return heightMap;
    }

    /// <summary>
    /// Returns a height map specified in a file.
    /// </summary>
    /// <param name="path">Path to height map file</param>
    /// <returns>Height map as described by file</returns>
    private int[,] GetHeightMapFromFile(string path)
    {
        int[,] heightMap = new int[_boardLength, _boardWidth];

        // TODO: get file from given path

        return heightMap;
    }

    #endregion

    private GameObject[,] InstantiateBoard(int[,] heightMap)
    {
        int length = _boardLength;
        int width = _boardWidth;
        int heightMapOffset = 0;

        if (_useBorder)
        {
            length += 2;
            width += 2;
            heightMapOffset = 1;
        }

        // initialize tiles
        _tiles = new GameObject[length, width];

        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                switch (_type)
                {
                    case TileType.Square:
                        if (_useBorder && (z == 0 || z == length - 1 || x == 0 || x == width - 1))
                        {
                            // TODO: pooling!
                            _tiles[z, x] = Instantiate(_borderTile,
                                new Vector3(_tileSideLength * x, 0, _tileSideLength * z), Quaternion.identity,
                                this.transform);
                        }
                        else
                        {
                            _tiles[z, x] = Instantiate(_tile, new Vector3(_tileSideLength * x, 0, _tileSideLength * z),
                                Quaternion.identity, this.transform);
                            _tiles[z, x].GetComponent<TileBehaviour>()
                                .SetHeight(0.2f + heightMap[z - heightMapOffset, x - heightMapOffset]);
                        }

                        break;
                    case TileType.Hexagonal:
                        //TODO
                        break;
                    case TileType.Triangle:
                        //TODO
                        break;
                }
            }
        }

        return _tiles;
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