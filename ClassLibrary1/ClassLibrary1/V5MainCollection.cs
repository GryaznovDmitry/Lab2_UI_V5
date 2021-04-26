using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;

[assembly: InternalsVisibleToAttribute("WpfApp1")]
namespace ClassLibrary1
{
    [Serializable]
    public class V5MainCollection : IEnumerable<V5Data>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private List<V5Data> V5List { get; set; }
        public bool IsChanged { get; set; }

        public V5MainCollection()
        {
            V5List = new List<V5Data>();
        }

        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void OnCollectionChanged(NotifyCollectionChangedAction ev)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public string ErrorMessage { get; set; }

        public IEnumerator<V5Data> GetEnumerator()
        {
            return V5List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return V5List.GetEnumerator();
        }

        public int Count => V5List.Count; 
           
        public bool Remove(string id, DateTime date)
        {
            bool flag = false;
            for (int i = 0; i < V5List.Count; i++)
            {
                if (Equals(V5List[i].InfoData, id) == true
                        && V5List[i].Time.CompareTo(date) == 0)
                {
                    V5List.RemoveAt(i);
                    i--;
                    flag = true;
                    OnPropertyChanged("IsChanged");
                    OnCollectionChanged(NotifyCollectionChangedAction.Remove);
                    OnPropertyChanged("Count");
                }
            }
            return flag;
        }

        public void AddDefaults()
        {
            Random rand = new Random();
          
            int NElem = rand.Next(3, 7), Rand1, Rand2, Rand3, Rand4;
            Grid2D Gr;
            V5DataCollection DataColl;
            V5DataOnGrid DataGrid;
            V5List = new List<V5Data>();
            for (int i = 0; i < NElem; i++)
            {
                Rand1 = rand.Next(0, 2);                
                if (Rand1 == 0)
                {     
                    Rand3 = rand.Next(1, 10);
                    Rand4 = rand.Next(1, 10);
                    Gr = new Grid2D(Rand3, Rand3, Rand4, Rand4);
                    DataGrid = new V5DataOnGrid("", DateTime.Now, Gr);
                    DataGrid.InitRandom(0, 10);
                    V5List.Add(DataGrid);
                }
                else
                {
                    Rand2 = rand.Next(1, 10);
                    DataColl = new V5DataCollection("", DateTime.Now);
                    DataColl.InitRandom(Rand2, 4, 5, 1, 4);
                    V5List.Add(DataColl);
                }
            }
            OnCollectionChanged(NotifyCollectionChangedAction.Add);
            OnPropertyChanged("Count");
            OnPropertyChanged("MinMC");
            IsChanged = true;
        }

        public override string ToString()
        {
            string str = "";
            foreach (V5Data item in V5List)
            {
                str += item.ToString();
            }
            str += "\n\n";
            return str;
        }

        public string ToLongString(string format)
        {
            string str = "";
            foreach (V5Data item in V5List)
            {
                str += item.ToLongString(format);
            }
            str += "\n\n";
            return str;
        }

        public float MinVecLenDC
        {
            get
            {
                var query = from elem in (from data in V5List
                                          where data is V5DataCollection
                                          select (V5DataCollection)data)
                            from item in elem
                            select item.Value.Length();
                if (query.Count() > 0)
                    return query.Min();
                else return 0;
            }
        }

        public float MinVecLenDG
        {
            get
            {
                var query = from elem in (from data in V5List
                                          where data is V5DataOnGrid
                                          select (V5DataOnGrid)data)
                            from item in elem
                            select item.Value.Length();
                if (query.Count() > 0)
                    return query.Min();
                else return 0;
            }
        }

        public int DoGCount
        {
            get
            {
                var query = from data in V5List
                            where data is V5DataOnGrid
                            select (V5DataOnGrid)data;
                return query.Count();
            }
        }

        public int DCCount
        {
            get
            {
                var query = from data in V5List
                            where data is V5DataCollection
                            select (V5DataCollection)data;
                return query.Count();
            }
        }

        public string MinMC
        {
            get 
            {   if (Count == 0)
                {
                    return 0.ToString();
                }
                else
                {
                    if ((DoGCount != 0) && (DCCount != 0) && (MinVecLenDC > MinVecLenDG))
                        return MinVecLenDC.ToString("g3");
                    else if (DCCount == 0)
                        return MinVecLenDG.ToString("g3");
                    else return MinVecLenDC.ToString("g3");
                }
            }
        }



        public IEnumerable<DataItem> CollMinElems
        {
            get
            {
                if (MinVecLenDC < MinVecLenDG)
                {
                    var query1 = //from data in V5List
                                 from elems in
                                (from dat in V5List
                                 where dat is V5DataCollection
                                 select (V5DataCollection)dat)
                                 from item in elems
                                 where item.Value.Length() == MinVecLenDC
                                 select item;
                    return query1;
                }
                else
                {
                    var query2 = //from data in V5List
                                 from elems in
                                (from dat in V5List
                                 where dat is V5DataOnGrid
                                 select (V5DataOnGrid)dat)
                                 from item in elems
                                 where item.Value.Length() == MinVecLenDG
                                 select item;
                    return query2;
                }
            }
        }

        public IEnumerable<Vector2> Points
        {
            get
            {
                IEnumerable<Vector2> query1 = from elem in
                                                  (from data in V5List
                                                   where data is V5DataOnGrid
                                                   select (V5DataOnGrid)data)
                                              from item in elem
                                              select item.Coord;
                IEnumerable<Vector2> query2 = from elem in
                                                  (from data in V5List
                                                   where data is V5DataCollection
                                                   select (V5DataCollection)data)
                                              from item in elem
                                              select item.Coord;
                var query = from item in query1.Except(query2)
                            select item;
                return query;
            }
        }

        public IEnumerable<V5DataCollection> getDCElems()
        {
            var query = from item in this.V5List
                        where item is V5DataCollection
                        select item as V5DataCollection;
            return query;
        }

        public IEnumerable<V5DataOnGrid> getDoGElems()
        {
            var query = from item in this.V5List
                        where item is V5DataOnGrid
                        select item as V5DataOnGrid;
            return query;
        }

        public void Save(string filename)
        {
            FileStream fs = null;
            try
            {
                if (!File.Exists(filename))
                {
                    fs = File.Create(filename);
                }
                else
                {
                    fs = File.OpenWrite(filename);
                }
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, V5List);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
                IsChanged = false;
                OnPropertyChanged("IsChanged");
            }
        }

        public void Load(string filename)
        {
            FileStream fs = null;
            try
            {
                fs = File.OpenRead(filename);
                BinaryFormatter bf = new BinaryFormatter();
                var list = (List<V5Data>)bf.Deserialize(fs);
                V5List = list;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
                IsChanged = true;
                OnCollectionChanged(NotifyCollectionChangedAction.Add);
                OnPropertyChanged("IsChanged");
                OnPropertyChanged("Count");
                OnPropertyChanged("MinMC");
            }
        }

        public void Add(V5Data item)
        {
            try
            {
                V5List.Add(item);
                OnCollectionChanged(NotifyCollectionChangedAction.Add);
                IsChanged = true;
                OnPropertyChanged("IsChanged");
                OnPropertyChanged("Count");
                OnPropertyChanged("MinMC");              
            }
            catch (Exception ex)
            {
                ErrorMessage = "Add Element: " + ex.Message;
            }
        }

        public void AddFromFile(string filename)
        {
            try
            {
                V5DataOnGrid DG = new V5DataOnGrid(filename);
                Add(DG);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddDefaultDataCollection()
        {
            V5DataCollection DC = new V5DataCollection("Default DC");
            DC.InitRandom(1, 10, 10, 0, 10);
            Add(DC);
        }

        public void AddDefaultDataOnGrid()
        {
            Grid2D grid = new Grid2D();
            V5DataOnGrid DoG = new V5DataOnGrid("Default DoG", default, grid);
            DoG.InitRandom(0, 10);
            Add(DoG);
        }
    }
}