using PdfMerge.Model;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PdfMerge.Service
{
    public class PdfMergeService
    {
        private static string MERGEDFILENAME = "MergedFile.pdf";

        public Task<ServiceResult> MergePdfFiles(IEnumerable<PdfFile> pdfFiles, string destinationFilePath, CancellationToken cancellationToken, Action<int> updateProgress)
        {
            return Task.Run(() =>
            {
                var result = new ServiceResult() { Result = true };
                
                using (PdfDocument outputPDFDocument = new PdfDocument())
                {
                    try
                    {
                        var fileCount = 0;
                        foreach (var pdfFile in pdfFiles)
                        {
                            using (PdfDocument inputPDFDocument = PdfReader.Open(pdfFile.FilePath, PdfDocumentOpenMode.Import))
                            {
                                outputPDFDocument.Version = inputPDFDocument.Version;
                                var pageCount = 0;
                                foreach (PdfPage page in inputPDFDocument.Pages)
                                {
                                    if (pageCount % 20 == 0 && cancellationToken.IsCancellationRequested)
                                    {
                                        break;
                                    }
                                    outputPDFDocument.AddPage(page);
                                }
                            }
                            if (cancellationToken.IsCancellationRequested)
                            {
                                result.Result = false;
                                result.Messages.Add("Process cancelled by user.");
                                break;
                            }
                            
                            fileCount += 1;
                            updateProgress(fileCount);
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Result = false;
                        result.Messages.Add(ex.Message);
                    }

                    var mergedFileName = GetFileName(destinationFilePath, MERGEDFILENAME, true);

                    outputPDFDocument.Save(mergedFileName);
                    result.Argument = mergedFileName;
                }

                return result;
            });
        }

        private static string GetFileName(string directoryName, string fileName, bool getNewName=false)
        {
            var fileNameWithouExtension = Path.GetFileNameWithoutExtension(fileName);
            var fileExtension = Path.GetExtension(fileName);

            if (getNewName)
            {
                var fileCount = Directory.EnumerateFiles(directoryName, $"*{fileExtension}")
                                .Where(file => Regex.IsMatch(file, $@"{fileNameWithouExtension}( \(\d\))?{fileExtension}"))
                                .Count();
                if (fileCount != 0)
                {
                    fileName = $"{fileNameWithouExtension} ({fileCount}){fileExtension}";
                }
            }

            return Path.Combine(directoryName, $"{fileName}");
        }
    }
}
