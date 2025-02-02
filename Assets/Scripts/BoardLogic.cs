using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;
using SpecialLayout = DefaultNamespace.PredefinedLayouts.Layout;

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

    #region Private fields

    private readonly List<GameObject> _tilesPool = new();
    private readonly List<GameObject> _borderTilesPool = new();
    private int _numberTilesInUse;
    private int _numberBorderTilesInUse;
    private GameObject[,] _tiles;
    private System.Random _random;

    #endregion Private fields

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

            RemoveCurrentBoard();
            SetUpGameBoard();
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (_boardWidth < MAX_BOARD_WIDTH && _boardLength < MAX_BOARD_LENGTH)
            {
                _boardWidth++;
                _boardLength++;

                // TODO: keep current layout and add a row and a column

                RemoveCurrentBoard();
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

                RemoveCurrentBoard();
                SetUpGameBoard();
            }
        }

        // create new board
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveCurrentBoard();
            SetUpGameBoard();
        }
    }

    public void OnDestroy()
    {
        _tilesPool.Clear();
        _borderTilesPool.Clear();
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

        if (TryGetPredefinedLayout(_generationSeed))
        {
            return;
        }

        Reset();
    }

    private bool TryGetPredefinedLayout(string seed)
    {
        if (seed.Contains("hello"))
        {
            Debug.LogError("Hi!");

            if (_boardWidth >= 7 && _boardLength >= 7)
            {
                RemoveCurrentBoard();

                float[,] heightMap = PredefinedLayouts.GetArrangement(SpecialLayout.Smile, _boardWidth,
                    _boardLength, _unitTileHeight);

                SetUpGameBoard(heightMap);

                return true;
            }
        }

        return false;
    }

    public void Reset()
    {
        RemoveCurrentBoard();
        SetUpGameBoard();
    }

    private void SetUpGameBoard()
    {
        float[,] heightMap = GetHeightArrangement();

        InstantiateBoard(heightMap);
    }

    private void SetUpGameBoard(float[,] heightMap)
    {
        InstantiateBoard(heightMap);
    }

    private void RemoveCurrentBoard()
    {
        int w = _tiles.GetLength(1);
        int l = _tiles.GetLength(0);

        for (int z = 0; z < l; z++)
        {
            for (int x = 0; x < w; x++)
            {
                _tiles[z, x].SetActive(false);
            }
        }

        _numberTilesInUse = 0;
    }

    #region INSTANTIATION

    #region HEIGHT MAP

    private void SetNextHeightArrangement()
    {
        int enumLength = Enum.GetNames(typeof(HeightArrangement)).Length;
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

        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (_type == TileType.Square)
                {
                    if (_useBorder && (z == 0 || z == length - 1 || x == 0 || x == width - 1))
                    {
                        var borderTile = _numberBorderTilesInUse < _borderTilesPool.Count
                            ? _borderTilesPool[_numberBorderTilesInUse]
                            : Instantiate(_borderTile, new Vector3(x, 0, z), Quaternion.identity, transform);

                        if (_numberBorderTilesInUse >= _borderTilesPool.Count)
                        {
                            _borderTilesPool.Add(borderTile);
                        }

                        borderTile.SetActive(true);
                        borderTile.transform.position = new Vector3(x, 0, z);
                        _tiles[z, x] = borderTile;
                        _numberBorderTilesInUse++;
                    }
                    else
                    {
                        var tile = _numberTilesInUse < _tilesPool.Count
                            ? _tilesPool[_numberTilesInUse]
                            : Instantiate(_tile, new Vector3(x, 0, z), Quaternion.identity, transform);

                        if (_numberTilesInUse >= _tilesPool.Count)
                        {
                            _tilesPool.Add(tile);
                        }

                        tile.SetActive(true);
                        tile.transform.position = new Vector3(x, 0, z);
                        _tiles[z, x] = tile;
                        _numberTilesInUse++;

                        float height = heightMap[z - heightMapOffset, x - heightMapOffset];
                        float steppedHeight = height - (height % _unitTileHeight);
                        tile.GetComponent<TileBehaviour>().SetHeight(0.2f + steppedHeight);
                    }
                }
                else
                {
                    Debug.Log("Too bad, not implemented yet!");
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

    private static string RandomString(int length)
    {
        System.Random random = new System.Random();
        const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(CHARS, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    #endregion Helpers
}