using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Serialization;

namespace ClassLibrary1
{
    [Serializable]
    public class V5DataCollection : V5Data, IEnumerable<DataItem>, ISerializable
    {
        public Dictionary<Vector2, Vector2> Dic { get; set; }
        public V5DataCollection(string s, DateTime t = default) : base(s, t)
        {
            Dic = new Dictionary<Vector2, Vector2>();
        }

        public void InitRandom(int nItems, float xmax, float ymax,
                               float minValue, float maxValue)
        {
            Random rand = new Random();
            float x, y, data_x, data_y;
            Vector2 point, Value;
            for (int i = 0; i < nItems; i++)
            {
                data_x = (float)rand.NextDouble();
                data_y = (float)rand.NextDouble();
                x = (float)rand.NextDouble();
                y = (float)rand.NextDouble();
                data_x = minValue * data_x + maxValue * (1 - data_x);
                data_y = minValue * data_y + maxValue * (1 - data_y);
                x = xmax * x;
                y = ymax * y;
                point = new Vector2(x, y);
                Value = new Vector2(data_x, data_y);
                Dic.Add(point, Value);
            }
        }

        public override Vector2[] NearEqual(float eps)
        {
            List<Vector2> v = new List<Vector2>();
            foreach (KeyValuePair<Vector2, Vector2> keys in Dic)
            {
                Vector2 Elem = keys.Value;
                if (Math.Abs(Elem.X - Elem.Y) <= eps)
                    v.Add(Elem);
            }
            Vector2[] res = v.ToArray();
            return res;
        }
        
        public override string ToLongString()
        {
            string str = "V5DataCollection\n";
            str += InfoData + " " + Time.ToString() + "\nNumber of elements: " + Dic.Count + "\n";
            foreach (KeyValuePair<Vector2, Vector2> key in Dic)
            {
                str += key.Key.ToString() + " " + key.Value.ToString() + "\n";
            }
            str += "\n";
            return str;
        }

        public override string ToString()
        {
            string str = "V5DataCollection\n";
            str += InfoData + " " + Time.ToString() + "\nNumber of elements: " + Dic.Count + "\n\n";
            return str;
        }

        public override string ToLongString(string format)
        {
            string str = "V5DataCollection\n";
            str += InfoData + " " + Time.ToString() + "\nNumber of elements: " + Dic.Count + "\n";
            foreach (KeyValuePair<Vector2, Vector2> key in Dic)
            {
                str += key.Key.ToString(format) + " " + key.Value.ToString(format) + "\n";
            }
            str += "\n";
            return str;
        }
        
        IEnumerator<DataItem> IEnumerable<DataItem>.GetEnumerator()
        {
            foreach (KeyValuePair<Vector2, Vector2> key in Dic)
            {
                DataItem Item = new DataItem(key.Key, key.Value);
                yield return Item;
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (KeyValuePair<Vector2, Vector2> key in Dic)
            {
                DataItem Item = new DataItem(key.Key, key.Value);
                yield return Item;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            float[] coordx = new float[Dic.Count];
            float[] coordy = new float[Dic.Count];
            float[] valx = new float[Dic.Count];
            float[] valy = new float[Dic.Count];
            int j = 0;
            foreach (KeyValuePair<Vector2, Vector2> pair in Dic)
            {
                coordx[j] = pair.Key.X;
                coordy[j] = pair.Key.Y;
                valx[j] = pair.Value.X;
                valy[j] = pair.Value.Y;
                j++;
            }
            info.AddValue("coordx", coordx);
            info.AddValue("coordy", coordy);
            info.AddValue("valx", valx);
            info.AddValue("valy", valy);
            info.AddValue("InfoData", InfoData);
            info.AddValue("Time", Time);
        }

        public V5DataCollection(SerializationInfo info, StreamingContext context):
                         base((string)info.GetValue("InfoData", typeof(string)),
                    (DateTime)info.GetValue("Time", typeof(DateTime)))
        {
            float[] coordx = (float[])info.GetValue("coordx", typeof(float[]));
            float[] coordy = (float[])info.GetValue("coordy", typeof(float[]));
            float[] valx = (float[])info.GetValue("valx", typeof(float[]));
            float[] valy = (float[])info.GetValue("valy", typeof(float[]));
            Dic = new Dictionary<Vector2, Vector2>();
            for (int j = 0; j < coordx.Length; j++)
            {
                Dic.Add(new Vector2(coordx[j],coordy[j]), new Vector2(valx[j], valy[j]));
            }
        }
    }
}