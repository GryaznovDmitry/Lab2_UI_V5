using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ClassLibrary1;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private V5MainCollection MC = new V5MainCollection();
        BindDataOnGrid bind;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MC;
            bind = new BindDataOnGrid(ref MC);
            AddCustomGrid.DataContext = bind;
        }

        public static RoutedCommand AddCustomDoG = new RoutedCommand("Add", typeof(WpfApp1.MainWindow));

        private void Add_FromFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                if ((bool)dlg.ShowDialog())
                    MC.AddFromFile(dlg.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add From File error: " + ex.Message);
            }
            finally
            {
                ErrorMsg();
            }
        }

        private void AddDefault_V5DataCollection_Click(object sender, RoutedEventArgs e)
        {
            MC.AddDefaultDataCollection();
            ErrorMsg();
        }

        private void AddDefaults_Click(object sender, RoutedEventArgs e)
        {
            MC.AddDefaults();
            DataContext = MC;
            ErrorMsg();
        }

        private void AddDefault_V5DataOnGrid_Click(object sender, RoutedEventArgs e)
        {
            MC.AddDefaultDataOnGrid();
            ErrorMsg();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (MC.IsChanged)
            {
                UnsavedChanges();
            }
            MC = new V5MainCollection();
            DataContext = MC;
            bind = new BindDataOnGrid(ref MC);
            AddCustomGrid.DataContext = bind;
            ErrorMsg();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MC.IsChanged)
                {
                    UnsavedChanges();
                }
                Microsoft.Win32.OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();
                if ((bool)fd.ShowDialog())
                {
                    MC = new V5MainCollection();
                    MC.Load(fd.FileName);
                    DataContext = MC;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loading Error: " + ex.Message);
            }
            finally
            {
                ErrorMsg();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                if ((bool)dialog.ShowDialog())
                    MC.Save(dialog.FileName);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Saving Error: " + ex.Message);
            }
            finally
            {
                ErrorMsg();
            }
        }

        private bool UnsavedChanges()
        {
            MessageBoxResult msg = MessageBox.Show("Save Changes?", "Save", MessageBoxButton.YesNoCancel);
            if (msg == MessageBoxResult.Yes)
            {
                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                if ((bool)dialog.ShowDialog())
                    MC.Save(dialog.FileName);
            }
            else if (msg == MessageBoxResult.Cancel)
            {
                return true;
            }
            return false;
        }

        private void AppClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MC.IsChanged)
            {
                e.Cancel = UnsavedChanges();
            }
            ErrorMsg();
        }

        public void ErrorMsg()
        {
            if (MC.ErrorMessage != null)
            {
                MessageBox.Show(MC.ErrorMessage, "Error");
                MC.ErrorMessage = null;
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var selectedLB = LB_Main.SelectedItems;
            List<V5Data> Items = new List<V5Data>();
            Items.AddRange(selectedLB.Cast<V5Data>());
            foreach (V5Data item in Items)
            {
                MC.Remove(item.InfoData, item.Time);
            }
            ErrorMsg();
        }

        private void FilterDataCollection(object sender, FilterEventArgs args)
        {
            var item = args.Item;
            if (item != null)
            {
                if (item.GetType() == typeof(V5DataCollection)) args.Accepted = true;
                else args.Accepted = false;
            }
        }

        private void FilterDataOnGrid(object sender, FilterEventArgs args)
        {
            var item = args.Item;
            if (item != null)
            {
                if (item.GetType() == typeof(V5DataOnGrid)) args.Accepted = true;
                else args.Accepted = false;
            }
        }

        private void OpenHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (MC.IsChanged)
            {
                UnsavedChanges();
            }
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            if ((bool)dialog.ShowDialog())
            {
                MC = new V5MainCollection();
                MC.Load(dialog.FileName);
                DataContext = MC;
            }
            ErrorMsg();
        }

        private void SaveHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            if ((bool)dialog.ShowDialog())
                MC.Save(dialog.FileName);
            ErrorMsg();
        }

        private void CanSaveHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = MC.IsChanged;
        }

        private void DeleteHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var Selection = LB_Main.SelectedItems;
            List<V5Data> selectedItems = new List<V5Data>();
            selectedItems.AddRange(Selection.Cast<V5Data>());
            foreach (V5Data item in selectedItems)
            {
                MC.Remove(item.InfoData, item.Time);
            }
        }

        private void CanDeleteHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (LB_Main != null)
            {
                var Selection = LB_Main.SelectedItems;
                List<V5Data> selectedItems = new List<V5Data>();
                selectedItems.AddRange(Selection.Cast<V5Data>());
                if (selectedItems.Count != 0)
                    e.CanExecute = true;
                else
                    e.CanExecute = false;
            }
        }

        private void AddDataOnGridHandler(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                bind.Add();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void CanAddDataOnGridHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (TextBox_Size == null || TextBox_Xnum == null ||
                TextBox_Ynum == null || TextBox_DGstr == null)
            {
                e.CanExecute = false;
            }
            else if (Validation.GetHasError(TextBox_Size) || Validation.GetHasError(TextBox_Xnum) ||
                     Validation.GetHasError(TextBox_Ynum) || Validation.GetHasError(TextBox_DGstr))
            {
                e.CanExecute = false;
            }
            else
                e.CanExecute = true;
        }     
    }
}
