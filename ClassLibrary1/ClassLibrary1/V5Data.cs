using System;
using System.Numerics;

namespace ClassLibrary1
{
    [Serializable]
    public abstract class V5Data
    {
        public string InfoData { get; set; }
        public DateTime Time { get; set; }

        public V5Data(string id = "Empty Data", DateTime t = default)
        {
            InfoData = id;
            Time = t;
        }

        public abstract Vector2[] NearEqual(float eps);

        public abstract string ToLongString();

        public override string ToString()
        {
            return InfoData + ", " + Time.ToString() + "\n\n";
        }

        public abstract string ToLongString(string format);
    }
}
