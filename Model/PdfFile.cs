using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfMerge.Model
{
    public class PdfFile : INotifyPropertyChanged
    {
        private string fileName;
        private string filePath;
        private bool isSelected;

        public PdfFile(FileInfo fileInfo)
        {
            fileName = fileInfo.Name;
            filePath = fileInfo.FullName;
        }

        public string FilePath {  get { return filePath; } }

        public string FileName 
        { 
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
