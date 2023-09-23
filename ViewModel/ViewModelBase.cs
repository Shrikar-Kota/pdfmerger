using System;
using System.ComponentModel;
using System.Windows.Input;

namespace PdfMerge.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private string menuItemSelected;
        private ICommand menuItemCommand;
        private Action<object> viewChanged;
        public ViewModelBase(Action<object> viewChanged)
        {
            this.viewChanged = viewChanged;

            menuItemCommand = new RelayCommand((param) => MenuItemClickExecute(param));
            SetMenuItemSelected(this);
        }

        public string MenuItemSelected
        { 
            get
            {
                return menuItemSelected;
            }
            private set
            {
                if (menuItemSelected != value)
                {
                    menuItemSelected = value;
                    OnPropertyChanged("MenuItemSelected");
                }
            }
        }
        public ICommand MenuItemCommand { get { return menuItemCommand; } }

        public event PropertyChangedEventHandler PropertyChanged;

        public void MenuItemClickExecute(object param)
        {
            viewChanged(param);
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetMenuItemSelected(object obj)
        {
            switch (obj.GetType().Name)
            {
                case "HomeViewModel":
                    {
                        MenuItemSelected = "Home";
                        break;
                    }
                case "PdfMergeViewModel":
                    {
                        MenuItemSelected = "Pdf Merger";
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
