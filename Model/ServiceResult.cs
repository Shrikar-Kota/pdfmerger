using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfMerge.Model
{
    public class ServiceResult
    {
        public ServiceResult()
        {
            Messages = new List<string>();
        }

        public List<string> Messages { get; set; }
        public bool Result { get; set; }
        public object Argument { get; set; }
    }
}
