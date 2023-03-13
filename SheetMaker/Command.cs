#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

#endregion

namespace SheetMaker
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // put any code needed for the form here

            // open form
            MyForm currentForm = new MyForm(doc)
            {
                Width = 800,
                Height = 450,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                Topmost = true,
            };

            currentForm.ShowDialog();

            if (currentForm.DialogResult == true)
            {
                int count = 0;
                List<DataClass1> dataList = currentForm.GetData();

                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Create Sheets");
                    foreach (DataClass1 curFormRow in dataList)
                    {
                        string sheetNumber = curFormRow.Item1 as string;
                        string sheetName = curFormRow.Item2 as string;
                        bool isPlaceHolder = curFormRow.Item3;
                        string sheetSize = curFormRow.Item4;
                        //Create a new sheet
                        ViewSheet sheet = CreateSheetFromCurFormRow(doc, sheetNumber, sheetName, isPlaceHolder, sheetSize);
                        count++;

                    } 
                    t.Commit();
                }
                TaskDialog.Show("Info", "Created " + count + "ViewSheets");
            }

            return Result.Succeeded;
        }

        public static ViewSheet CreateSheetFromCurFormRow(Document doc, string sheetNumber, string sheetName, bool isPlaceHolder, string sheetSize)
        {
            ViewSheet newSheet;
            if (isPlaceHolder)
            {
                ViewSheet sheet = ViewSheet.CreatePlaceholder(doc);
                sheet.Name        = sheetName;
                sheet.SheetNumber = sheetNumber;
                newSheet = sheet;
            }
            else
            {
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                var titleBlocks = collector.OfCategory(BuiltInCategory.OST_TitleBlocks).ToList();

                var titleBlock = titleBlocks.FirstOrDefault(x => x.Name == sheetSize);
                ViewSheet sheet = ViewSheet.Create(doc, titleBlock.Id);
                sheet.Name = sheetName;
                sheet.SheetNumber = sheetNumber;
                newSheet = sheet;
            }
          
            return newSheet;
        }

        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}
