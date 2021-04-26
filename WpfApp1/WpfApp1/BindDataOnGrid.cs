﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1;

namespace WpfApp1
{
    class BindDataOnGrid : IDataErrorInfo, INotifyPropertyChanged
    {
        private float xSize;
        private int xNum, yNum;
        private string str;

        public V5MainCollection MainCol;

        public BindDataOnGrid(ref V5MainCollection MC)
        {
            MainCol = MC;
        }

        public float Size
        {
            get
            {
                return xSize;
            }
            set
            {
                xSize = value;
                OnPropertyChanged("Size");
            }
        }

        public int Xnum
        {
            get
            {
                return xNum;
            }
            set
            {
                xNum = value;
                OnPropertyChanged("Xnum");
            }
        }

        public int Ynum
        {
            get
            {
                return yNum;
            }
            set
            {
                yNum = value;
                OnPropertyChanged("Ynum");
            }
        }

        public string DGstr
        {
            get
            {
                return str;
            }
            set
            {
                str = value;
                OnPropertyChanged("DGstr");
            }
        }

        public string this[string columnName]
        {
            get
            {
                string msg = null;
                switch (columnName)
                {
                    case "DGstr":
                        foreach (V5DataOnGrid item in MainCol)
                            if (DGstr == item.InfoData)
                                msg = "Same string value";
                        break;
                    case "Ynum":
                        if (Ynum < 3)
                            msg = "Ynum is less than 3";
                        break;
                    case "Xnum":
                        if (Xnum <= Ynum)
                            msg = "Xnum is not bigger than Ynum";
                        break;
                    default:
                        break;
                }
                return msg;
            }
        }

        public string Error
        {
            get
            {
                return "Error text";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public void Add(float s, int xnum, int ynum, string st)
        {
            Size = s;
            Xnum = xnum;
            Ynum = ynum;
            DGstr = st;
            Grid2D grid = new Grid2D(Size, Size, Xnum, Ynum);
            V5DataOnGrid DoG = new V5DataOnGrid(DGstr, DateTime.Now , grid);
            DoG.InitRandom();
            MainCol.Add(DoG);
        }
    }
}
