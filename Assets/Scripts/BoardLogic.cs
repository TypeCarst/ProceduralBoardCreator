using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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

    private const int MAX_BOARD_WIDTH = 20;
    private const int MAX_BOARD_LENGTH = 20;

    #region Serialized fields

    [SerializeField]
    private string _generationSeed;

    [SerializeField]
    private TileType _type = TileType.Square;

    [SerializeField]
    private HeightArrangement _arrangement = HeightArrangement.Random;

    [SerializeField]
    private int _boardWidth;

    [SerializeField]
    private int _boardLength;

    [SerializeField]
    private float _boardMaxHeight;

    [SerializeField]
    private float _unitTileHeight;
    
    [SerializeField]
    private bool _useBorder = true;

    [SerializeField]
    private GameObject _tile;

    [SerializeField]
    private GameObject _borderTile;

    #endregion Serialized fields

    private GameObject[,] _tiles;
    private System.Random _random;

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

    public void Regenerate(string baseSeed)
    {
        if (baseSeed.Length == 0)
        {
            string randomSeed = RandomString(10);
            _generationSeed = randomSeed;
        }
        else
        {
            _generationSeed = baseSeed;
        }
        
        int seed = _generationSeed.GetHashCode();
        Random.InitState(seed);
        
        if (_generationSeed.Contains("hello"))
        {
            Debug.LogError("Hi!");

            if (_boardWidth >= 7 && _boardLength >= 7)
            {
                DestroyBoard();
                
                float[,] heightMap = new float[_boardWidth, _boardLength];
                heightMap[1, 2] = 2*_unitTileHeight;
                heightMap[2, 1] = 2*_unitTileHeight;
                heightMap[2, 4] = 2*_unitTileHeight;
                heightMap[3, 1] = 2*_unitTileHeight;
                heightMap[4, 1] = 2*_unitTileHeight;
                heightMap[4, 4] = 2*_unitTileHeight;
                heightMap[5, 2] = 2*_unitTileHeight;
                
                SetUpGameBoard(heightMap);

                return;
            }
        }
        
        Reset();
    }

    public void Reset()
    {
        DestroyBoard();
        SetUpGameBoard();
    }

    public void SetUpGameBoard()
    {
        float[,] heightMap = GetHeightArrangement();

        InstantiateBoard(heightMap);
    }

    public void SetUpGameBoard(float[,] heightMap)
    {
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

    private float[,] GetHeightArrangement()
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
    private float[,] GenerateFlatHeightMap()
    {
        float[,] heightMap = new float[_boardLength, _boardWidth];

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
    private float[,] GenerateRandomHeightMap()
    {
        float[,] heightMap = new float[_boardLength, _boardWidth];

        for (int z = 0; z < _boardLength; z++)
        {
            for (int x = 0; x < _boardWidth; x++)
            {
                heightMap[z, x] = Random.value * _boardMaxHeight;
            }
        }

        return heightMap;
    }

    /// <summary>
    /// Returns a height map specified in a file.
    /// </summary>
    /// <param name="path">Path to height map file</param>
    /// <returns>Height map as described by file</returns>
    private float[,] GetHeightMapFromFile(string path)
    {
        float[,] heightMap = new float[_boardLength, _boardWidth];

        // TODO: get file from given path

        return heightMap;
    }

    #endregion

    private void InstantiateBoard(float[,] heightMap)
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

        // TODO: use object pooling!
        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (_type == TileType.Square)
                {
                    if (_useBorder && (z == 0 || z == length - 1 || x == 0 || x == width - 1))
                    {
                        _tiles[z, x] = Instantiate(_borderTile,
                            new Vector3(x, 0, z), Quaternion.identity,
                            transform);
                    }
                    else
                    {
                        _tiles[z, x] = Instantiate(_tile, new Vector3(x, 0, z),
                            Quaternion.identity, transform);

                        float height = heightMap[z - heightMapOffset, x - heightMapOffset];
                        float steppedHeight = height - (height % _unitTileHeight);
                        _tiles[z, x].GetComponent<TileBehaviour>()
                            .SetHeight(0.2f + steppedHeight);
                    }
                }
                else
                {
                    Debug.LogError("Too bad, not implemented yet!");
                }
            }
        }
    }

    public GameObject[,] AddSquareBorderTiles(GameObject[,] tiles)
    {
        GameObject[,] extendedTiles = new GameObject[tiles.GetLength(0) + 2, tiles.GetLength(1) + 2];

        // TODO: add borders
        // tiles[z, x] = Instantiate(borderTile, new Vector3(TileSideLength * x, 0, TileSideLength * z), Quaternion.identity, this.transform);

        return extendedTiles;
    }

    #endregion

    #region Helpers

    public static string RandomString(int length)
    {
        System.Random random = new System.Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    #endregion Helpers
}