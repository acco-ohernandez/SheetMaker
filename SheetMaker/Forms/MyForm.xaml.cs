using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace SheetMaker
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class MyForm : Window
    {
        ObservableCollection<DataClass1> dataList { get; set; }
        ObservableCollection<string> dataItems { get; set; }

        public MyForm(Autodesk.Revit.DB.Document doc)
        {
            InitializeComponent();
            dataList = new ObservableCollection<DataClass1>();

            dataItems = new ObservableCollection<string>();
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfCategory(BuiltInCategory.OST_TitleBlocks);
            foreach (var item in collector)
            {
                dataItems.Add(item.Name);
            }
            // Bind to column 4 "Item4_Titleblock"
            Item4_Titleblock.ItemsSource = dataItems;

            // Add the ObservableCollection to the DataGrid
            grdData.ItemsSource = dataList;

        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            dataList.Add(new DataClass1());
        }

        private void btn_Remove_Click(object sender, RoutedEventArgs e)
        {   // Grabs the selecgted row from th dataList and removes it
            try
            {
                foreach (DataClass1 curRow in dataList)
                {
                    if (grdData.SelectedItem == curRow)
                        dataList.Remove(curRow);
                }
            }
            catch (Exception)
            { }
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        public List<DataClass1> GetData()
        {
            return dataList.ToList();
        }
    }

    public class DataClass1
    {
        public string Item1 { get; set; }
        public string Item2 { get; set; }
        public bool Item3 { get; set; }
        public string Item4 { get; set; }
    }
}
