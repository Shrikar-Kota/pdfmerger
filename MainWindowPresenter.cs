using PdfMerge.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PdfMerge
{
    public class MainWindowPresenter
    {
        private Window mainWindow;
        private HomeViewModel homeViewModel;
        private PdfMergeViewModel pdfMergeViewModel;

        public MainWindowPresenter(Window mainWindow) 
        {
            this.mainWindow = mainWindow;
            viewChanged = View_Changed;

            homeViewModel = new HomeViewModel(viewChanged);
            mainWindow.DataContext = homeViewModel;
        }

        public Action<object> viewChanged;

        public void View_Changed(object param)
        {
            switch (param.ToString())
            {
                case "Home":
                    {
                        if (homeViewModel == null)
                        {
                            homeViewModel = new HomeViewModel(viewChanged);
                        }
                        mainWindow.DataContext = homeViewModel;
                        break;
                    }
                case "Pdf Merger":
                    {
                        if (pdfMergeViewModel == null)
                        {
                            pdfMergeViewModel = new PdfMergeViewModel(viewChanged);
                        }
                        mainWindow.DataContext = pdfMergeViewModel;
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
