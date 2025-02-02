namespace DefaultNamespace
{
    public class PredefinedLayouts
    {
        public enum Layout : byte
        {
            Smile
        }

        public static float[,] GetArrangement(Layout layout, int boardWidth, int boardLength, float unitTileHeight)
        {
            float[,] heightMap = new float[boardWidth, boardLength];

            switch (layout)
            {
                case Layout.Smile:
                    heightMap = GetSmileLayout(boardWidth, boardLength, unitTileHeight);
                    break;
                default:
                    break;
            }

            return heightMap;
        }

        private static float[,] GetSmileLayout(int boardWidth, int boardLength, float unitTileHeight)
        {
            float height = 2 * unitTileHeight;
            float[,] heightMap = new float[boardWidth, boardLength];
            heightMap[1, 2] = height;
            heightMap[2, 1] = height;
            heightMap[2, 4] = height;
            heightMap[3, 1] = height;
            heightMap[4, 1] = height;
            heightMap[4, 4] = height;
            heightMap[5, 2] = height;

            return heightMap;
        }
    }
}