using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Common
{
    public class SearchFilter
    {
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public string? Status { get; set; }
    }
}
