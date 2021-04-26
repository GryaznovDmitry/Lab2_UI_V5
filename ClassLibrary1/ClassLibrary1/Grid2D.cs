using System;

namespace ClassLibrary1
{
    [Serializable]
    public struct Grid2D
    {
        public float StepX { get; set; }
        public int NodeNumX { get; set; }
        public float StepY { get; set; }
        public int NodeNumY { get; set; }

        public Grid2D(float sx = 10, float sy = 10,
                        int nnx = 2, int nny = 2)
        {
            StepX = sx;
            StepY = sy;
            NodeNumX = nnx;
            NodeNumY = nny;
        }

        public Grid2D(Grid2D grid)
        {
            StepX = grid.StepX;
            StepY = grid.StepY;
            NodeNumX = grid.NodeNumX;
            NodeNumY = grid.NodeNumY;
        }

        public override string ToString()
        {
            return "\nGrid Parametrs: \n" +
                   "Step X size: " + StepX.ToString() + ",\n" +
                   "Number of steps X: " + NodeNumX.ToString() + ",\n" +
                   "Step Y size: " + StepY.ToString() + ",\n" +
                   "Number of steps Y: " + NodeNumY.ToString() + "\n\n";
        }

        public string ToString(string format)
        {
            return "\nGrid Parametrs: \n" +
                   "Step X size: " + StepX.ToString(format) + ",\n" +
                   "Number of steps X: " + NodeNumX.ToString() + ",\n" +
                   "Step Y size: " + StepY.ToString(format) + ",\n" +
                   "Number of steps Y: " + NodeNumY.ToString() + "\n\n";
        }

    }
}
