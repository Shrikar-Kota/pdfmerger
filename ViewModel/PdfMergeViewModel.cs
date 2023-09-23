using Microsoft.Win32;
using PdfMerge.Model;
using PdfMerge.Service;
using PdfMerge.Utils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using static PdfMerge.MainWindowPresenter;

namespace PdfMerge.ViewModel
{
    public class PdfMergeViewModel : ViewModelBase
    {
        private static string destinationPath = WindowsShell.DOWNLOADSFOLDER;

        private Action<int> updateProgress;
        private PdfMergeService pdfMergeService;
        private ICommand addFilesCommand;
        private ICommand clearFilesCommand;
        private ICommand mergeFilesCommand;
        private ICommand stopCommand;
        private ICommand clearAllFilesCommand;
        private ICommand fileListItemClickCommand;
        private bool isProcessing;
        private string progressText;
        private CancellationTokenSource cancellationTokenSource;

        public PdfMergeViewModel(Action<object> viewChanged) : base(viewChanged)
        {
            pdfMergeService = new PdfMergeService();
            cancellationTokenSource = new CancellationTokenSource();

            updateProgress = UpdateProgress;
            addFilesCommand = new RelayCommand(_ => AddFilesExecute(), _ => !isProcessing);
            mergeFilesCommand = new RelayCommand(_ => MergeFilesExecute(), _ => !isProcessing && PdfFiles.Count != 0);
            clearFilesCommand = new RelayCommand(_ => ClearFilesExecute(), _ => !isProcessing && PdfFiles.Count != 0 && PdfFiles.Any(pdfFile => pdfFile.IsSelected));
            clearAllFilesCommand = new RelayCommand(_ => ClearFilesExecute(true), _ => !isProcessing && PdfFiles.Count != 0);
            stopCommand = new RelayCommand(_ => StopExecute(), _ => !isProcessing);
            fileListItemClickCommand = new RelayCommand((param) => FileListItemClickExecute(param), _ => !isProcessing);
            PdfFiles = new ObservableCollection<PdfFile>();
        }

        public ICommand AddFilesCommand { get { return addFilesCommand; } }

        public ICommand MergeFilesCommand { get { return mergeFilesCommand; } }

        public ICommand ClearFilesCommand { get { return clearFilesCommand; } }

        public ICommand ClearAllFilesCommand { get { return clearAllFilesCommand; } }

        public ICommand StopCommand { get { return stopCommand; } }
        
        public ICommand FileListItemClickCommand { get { return fileListItemClickCommand; } }

        public bool IsProcessing
        {
            get
            {
                return isProcessing;
            }
            set
            {
                isProcessing = value;
                OnPropertyChanged("IsProcessing");
            }
        }

        public string ProgressText
        {
            get
            {
                return progressText;
            }
            set
            {
                progressText = value;
                OnPropertyChanged("ProgressText");
            }
        }

        public ObservableCollection<PdfFile> PdfFiles { get; private set; }

        private void AddFilesExecute()
        {
            OpenFileDialog fileDialog = new OpenFileDialog() 
            { 
                Title = "Browse pdf files", 
                Filter = "pdf files (*.pdf)|*.pdf",
                Multiselect = true
            };
            if (Convert.ToBoolean(fileDialog.ShowDialog()))
            {
                foreach(var file in fileDialog.FileNames)
                {
                    if (!PdfFiles.Any(pdfFile => pdfFile.FilePath == file))
                    {
                        PdfFiles.Add(new PdfFile(new FileInfo(file)));
                    }
                }
            }
        }

        private async void MergeFilesExecute()
        {
            IsProcessing = true;

            var result = await pdfMergeService.MergePdfFiles(PdfFiles, destinationPath, cancellationTokenSource.Token, updateProgress);

            IsProcessing = false;

            if (!result.Result)
            {
                MessageBox.Show(String.Join("\n", result.Messages), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show($"Pdf file created created successfully!{Environment.NewLine}{Environment.NewLine}Path: {result.Argument}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ClearFilesExecute(bool clearAll=false)
        {
            IsProcessing = true;

            var ix = 0;

            while (ix < PdfFiles.Count)
            {
                if (PdfFiles[ix].IsSelected || clearAll)
                {
                    PdfFiles.RemoveAt(ix);
                    ix--;
                }
                ix++;
            }

            IsProcessing = false;
        }

        private void StopExecute()
        {
            IsProcessing = false;
            cancellationTokenSource.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
        }

        private void FileListItemClickExecute(object param)
        {
            var fileListItem = param as PdfFile;

            fileListItem.IsSelected = !fileListItem.IsSelected;
        }

        private void UpdateProgress(int filesProcessed)
        {
            ProgressText = $"Processed files {filesProcessed} of {PdfFiles.Count}";
        }
    }
}
