using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [CustomEditor(typeof(BoardLogic))]
    public sealed class BoardLogicEditor : Editor
    {
        #region Serialized Properties

        private SerializedProperty _generationSeed;

        private SerializedProperty _type;
        private SerializedProperty _arrangement;
        private SerializedProperty _boardWidth;
        private SerializedProperty _boardLength;

        private SerializedProperty _tileSideLength;
        private SerializedProperty _tileMaxHeight;

        private SerializedProperty _useBorder;

        private SerializedProperty _tile;
        private SerializedProperty _borderTile;

        #endregion Serialized Properties

        #region Private fields

        private string _currentSeed;

        private int _currentBoardWidth;
        private int _currentBoardLength;
        private int _currentTileMaxHeight;
        private bool _currentUseBorder;

        #endregion Private fields

        private void OnEnable()
        {
            _generationSeed = serializedObject.FindProperty("_generationSeed");

            _type = serializedObject.FindProperty("_type");
            _arrangement = serializedObject.FindProperty("_arrangement");
            _boardWidth = serializedObject.FindProperty("_boardWidth");
            _boardLength = serializedObject.FindProperty("_boardLength");
            _tileMaxHeight = serializedObject.FindProperty("_tileMaxHeight");
            _useBorder = serializedObject.FindProperty("_useBorder");
            _tile = serializedObject.FindProperty("_tile");
            _borderTile = serializedObject.FindProperty("_borderTile");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Board settings", EditorStyles.boldLabel);

            _currentBoardWidth = _boardWidth.intValue;
            var widthContent = new GUIContent("Board width", "The width of the board");
            _currentBoardWidth = EditorGUILayout.IntSlider(widthContent, _currentBoardWidth, 1, 20);
            _boardWidth.intValue = _currentBoardWidth;

            _currentBoardLength = _boardLength.intValue;
            var lengthContent = new GUIContent("Board length", "Well, the length...");
            _currentBoardWidth = EditorGUILayout.IntSlider(lengthContent, _currentBoardLength, 1, 20);
            _boardLength.intValue = _currentBoardLength;

            _currentTileMaxHeight = _tileMaxHeight.intValue;
            var maxHeightContent = new GUIContent("Tile max height", "The maximum height of a tile");
            _currentBoardWidth = EditorGUILayout.IntSlider(maxHeightContent, _currentTileMaxHeight, 1, 10);
            _tileMaxHeight.intValue = _currentTileMaxHeight;

            _currentUseBorder = _useBorder.boolValue;
            _currentUseBorder =
                EditorGUILayout.Toggle(new GUIContent("Use border", "Use border tiles"), _currentUseBorder);
            _useBorder.boolValue = _currentUseBorder;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Prefabs", EditorStyles.boldLabel);

            EditorGUILayout.ObjectField(_tile, new GUIContent("Tile"));
            EditorGUILayout.ObjectField(_borderTile, new GUIContent("Border Tile"));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Generation", EditorStyles.boldLabel);

            _currentSeed = _generationSeed.stringValue;
            _currentSeed = EditorGUILayout.TextField(
                new GUIContent("Seed", "An empty field will create a random seed."),
                _currentSeed);
            _generationSeed.stringValue = _currentSeed;
            
            if (GUILayout.Button("Regenerate with seed"))
            {
                BoardLogic boardLogic = (BoardLogic)target;
                boardLogic.Regenerate(_currentSeed);
            }

            if (GUILayout.Button("Regenerate with random seed"))
            {
                BoardLogic boardLogic = (BoardLogic)target;
                boardLogic.Regenerate("");
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}