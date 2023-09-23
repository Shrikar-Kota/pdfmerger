using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PdfMerge.MainWindowPresenter;

namespace PdfMerge.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel(Action<object> viewChanged) : base(viewChanged)
        {
        }
    }
}
