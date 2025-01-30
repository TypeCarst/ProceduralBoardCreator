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

        private SerializedProperty _unitTileHeight;
        private SerializedProperty _boardMaxHeight;

        private SerializedProperty _useBorder;

        private SerializedProperty _tile;
        private SerializedProperty _borderTile;

        #endregion Serialized Properties

        #region Private fields

        private string _currentSeed;

        private int _currentBoardWidth;
        private int _currentBoardLength;
        private float _currentBoardMaxHeight;
        private float _currentUnitTileHeight;
        private bool _currentUseBorder;

        #endregion Private fields

        private void OnEnable()
        {
            _generationSeed = serializedObject.FindProperty("_generationSeed");

            _type = serializedObject.FindProperty("_type");
            _arrangement = serializedObject.FindProperty("_arrangement");
            _boardWidth = serializedObject.FindProperty("_boardWidth");
            _boardLength = serializedObject.FindProperty("_boardLength");
            _boardMaxHeight = serializedObject.FindProperty("_boardMaxHeight");
            _unitTileHeight = serializedObject.FindProperty("_unitTileHeight");
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
            _currentBoardLength = EditorGUILayout.IntSlider(lengthContent, _currentBoardLength, 1, 20);
            _boardLength.intValue = _currentBoardLength;

            _currentBoardMaxHeight = _boardMaxHeight.floatValue;
            var maxHeightContent = new GUIContent("Maximal board height", "The maximum height of the board");
            _currentBoardMaxHeight = EditorGUILayout.Slider(maxHeightContent, _currentBoardMaxHeight, 1, 10);
            _boardMaxHeight.floatValue = _currentBoardMaxHeight;
            
            _currentUnitTileHeight = _unitTileHeight.floatValue;
            var unitTileHeightContent = new GUIContent("Unit Tile height", "The unit height of every tile");
            _currentUnitTileHeight = EditorGUILayout.Slider(unitTileHeightContent, _currentUnitTileHeight, 0.1f, 10);
            _unitTileHeight.floatValue = _currentUnitTileHeight;

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

            EditorGUI.BeginDisabledGroup(!Application.isPlaying);

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

            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}