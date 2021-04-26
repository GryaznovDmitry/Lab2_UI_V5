using System;
using System.Numerics;
using System.Runtime.Serialization;

namespace ClassLibrary1
{
    [Serializable]
    public struct DataItem:ISerializable
    {
        public Vector2 Coord { get; set; }
        public Vector2 Value { get; set; } 

        public DataItem(Vector2 CRD, Vector2 VL)
        {
            Coord = CRD;
            Value = VL;
        }

        public DataItem(V5DataOnGrid dg, int x, int y)
        {
            Coord = new Vector2(dg.Net.StepX * x, dg.Net.StepY * y);
            Value = dg.Vec[x, y];
        }

        public override string ToString()
        {
            return Coord.ToString() + " "
                 + Value.ToString() + "\n";
        }

        public string ToString(string format)
        {
            return Coord.ToString(format) + " "
                 + Value.ToString(format) + "\n"
                 + "Vector Size: " +
                 Math.Sqrt(Value.X * Value.X +
                           Value.Y * Value.Y).ToString(format) + "\n";
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Coord_X", Coord.X);
            info.AddValue("Coord_Y", Coord.Y);
            info.AddValue("Value_X", Value.X);
            info.AddValue("Value_y", Value.Y);
        }

        public DataItem(SerializationInfo info, StreamingContext context)
        {
            float x = info.GetSingle("Coord_X");
            float y = info.GetSingle("Coord_Y");
            Coord = new Vector2(x, y);
            x = info.GetSingle("Value_X");
            y = info.GetSingle("Value_Y");
            Value = new Vector2(x, y);
        }

    }
}

