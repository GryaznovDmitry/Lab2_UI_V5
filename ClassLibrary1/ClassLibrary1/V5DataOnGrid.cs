using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Globalization;
using System.Runtime.Serialization;

namespace ClassLibrary1
{
    [Serializable]
    public class V5DataOnGrid : V5Data, IEnumerable<DataItem>, ISerializable
    {
        public Grid2D Net { get; set; }
        public Vector2[,] Vec { get; set; }

        public V5DataOnGrid(string id, DateTime t, Grid2D grid) : base(id, t)
        {
            Net = grid;
            Vec = new Vector2[Net.NodeNumX, Net.NodeNumY];
        }

        public V5DataOnGrid(string filename)
        {
            /*
             * формат ввода построчно:
             * 
             * 1) информация для базового класса
             * 2) дата и время для базового класса
             *    формат dd.mm.yy h:m
             * 3) параметр сетки: кол-во шагов по Х
             * 4) параметр сетки: размер шага по Х
             * 5) параметр сетки: кол-во шагов по У
             * 6) параметр сетки: размер шага по У
             * 7) X*Y строк с данными для векторов э/м поля формата: 
             *    величина_по_Х величина_по_У (параметры разделены пробелом)
             */
            StreamReader sr = null;

            try
            {
                sr = new StreamReader(filename);

                InfoData = sr.ReadLine();

                Time = DateTime.Parse(sr.ReadLine());

                Grid2D grid = new Grid2D
                {
                    NodeNumX = int.Parse(sr.ReadLine()),
                    StepX = float.Parse(sr.ReadLine()),
                    NodeNumY = int.Parse(sr.ReadLine()),
                    StepY = float.Parse(sr.ReadLine()),
                };
                Net = grid;
                Vec = new Vector2[Net.NodeNumX, Net.NodeNumY];

                for (int i = 0; i < Net.NodeNumX; i++)
                {
                    for (int j = 0; j < Net.NodeNumY; j++)
                    {

                        string[] data = sr.ReadLine().Split(' ');

                        Vec[i, j] = new Vector2(
                             (float)Convert.ToDouble(data[0]),
                             (float)Convert.ToDouble(data[1]));
                    }
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Filename is empty string");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File is not found");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Directory is not found");
            }
            catch (IOException)
            {
                Console.WriteLine("Unacceptable filename");
            }
            catch (FormatException)
            {
                Console.WriteLine("String could not be parsed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (sr != null)
                    sr.Dispose();
            }
        }

        public void InitRandom(float minValue = 0, float maxValue = 10)
        {
            Random rand = new Random();
            float x, y;
            for (int i = 0; i < Net.NodeNumX; i++)
                for (int j = 0; j < Net.NodeNumY; j++)
                {
                    x = (float)rand.NextDouble();
                    y = (float)rand.NextDouble();
                    x = minValue * x + maxValue * (1 - x);
                    y = minValue * y + maxValue * (1 - y);
                    Vec[i, j] = new Vector2(x, y);
                }
        }

        public static explicit operator V5DataCollection(V5DataOnGrid data)
        {
            V5DataCollection Col = new V5DataCollection(data.InfoData, data.Time);
            Vector2 key, value;
            for (int i = 0; i < data.Net.NodeNumX; i++)
                for (int j = 0; j < data.Net.NodeNumY; j++)
                {
                    key = new Vector2(i * data.Net.StepX, j * data.Net.StepY);
                    value = new Vector2(data.Vec[i, j].X, data.Vec[i, j].Y);
                    Col.Dic.Add(key, value);
                }
            return Col;
        }

        public override Vector2[] NearEqual(float eps)
        {
            List<Vector2> v = new List<Vector2>();
            for (int i = 0; i < Net.NodeNumX; i++)
                for (int j = 0; j < Net.NodeNumY; j++)
                    if (Math.Abs(Vec[i, j].X - Vec[i, j].Y) <= eps)
                        v.Add(Vec[i, j]);
            Vector2[] res = v.ToArray();
            return res;
        }

        public override string ToLongString()
        {

            string str = "V5DataOnGrid\n";
            str += InfoData + " " + Time.ToString() + " " + Net.ToString() + "\n";
            for (int i = 0; i < Net.NodeNumX; i++)
                for (int j = 0; j < Net.NodeNumY; j++)
                {
                    str += "[" + (i * Net.StepX).ToString() + ", " +
                                 (j * Net.StepY).ToString() + "] (" +
                                 Vec[i, j].X + ", " + Vec[i, j].Y + ")\n";
                }
            str += "\n";
            return str;
        }

        public override string ToString()
        {
            string str = "V5DataOnGrid\n";
            str += InfoData + " " + Time.ToString() + " " + Net.ToString() + "\n";
            return str;

        }

        public override string ToLongString(string format)
        {
            string str = "V5DataOnGrid\n";
            str += InfoData + " " + Time.ToString() + " " + Net.ToString(format) + "\n";
            for (int i = 0; i < Net.NodeNumX; i++)
                for (int j = 0; j < Net.NodeNumY; j++)
                {
                    str += "[" + (i * Net.StepX).ToString(format) + ", " +
                                 (j * Net.StepY).ToString(format) + "] (" +
                                 Vec[i, j].X.ToString(format) + ", " +
                                 Vec[i, j].Y.ToString(format) + ")\n";
                }
            str += "\n";
            return str;
        }

        IEnumerator<DataItem> IEnumerable<DataItem>.GetEnumerator()
        {
            List<DataItem> Items = new List<DataItem>();
            Vector2 Vector = new Vector2(0, 0);
            DataItem Item = new DataItem(Vector, Vector);
            for (int i = 0; i < Net.NodeNumX; i++)
                for (int j = 0; j < Net.NodeNumX; j++)
                {
                    Vector.X = i * Net.StepX;
                    Vector.Y = j * Net.StepY;
                    Item.Coord = Vector;
                    Item.Value = Vec[i, j];
                    Items.Add(Item);
                }
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            List<DataItem> Items = new List<DataItem>();
            Vector2 Vector = new Vector2(0, 0);
            DataItem Item = new DataItem(Vector, Vector);
            for (int i = 0; i < Net.NodeNumX; i++)
                for (int j = 0; j < Net.NodeNumX; j++)
                {
                    Vector.X = i * Net.StepX;
                    Vector.Y = j * Net.StepY;
                    Item.Coord = Vector;
                    Item.Value = Vec[i, j];
                    Items.Add(Item);
                }
            return Items.GetEnumerator();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            float[] valx = new float[Net.NodeNumX];
            float[] valy = new float[Net.NodeNumY];
            for (int i = 0; i < valx.Length; i++)
                for (int j = 0; j < valy.Length; j++)
                {
                    valx[i] = Vec[i, j].X;
                    valy[j] = Vec[i, j].Y;
                }
            info.AddValue("net", Net);
            info.AddValue("valx", valx);
            info.AddValue("valy", valy);
            info.AddValue("InfoData", InfoData);
            info.AddValue("Time", Time);
        }

        public V5DataOnGrid(SerializationInfo info, StreamingContext contex):
                    base((string)info.GetValue("InfoData", typeof(string)),
                       (DateTime)info.GetValue("Time", typeof(DateTime)))
        {
            Net = (Grid2D)info.GetValue("net", typeof(Grid2D));
            Vec = new Vector2[Net.NodeNumX, Net.NodeNumY];
            float[] valx = (float[])info.GetValue("valx", typeof(float[]));
            float[] valy = (float[])info.GetValue("valy", typeof(float[]));
            for (int i = 0; i < valx.Length; i++)
                for (int j = 0; j < valy.Length; j++)
                {
                    Vec[i, j].X = valx[i];
                    Vec[i, j].Y = valy[j];
                }
        }
    }
}
