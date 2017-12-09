using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biApi.Models
{

    public class TransList
    {
        public bool hasError { get; set; }
        public int totalCount { get; set; }
        public dynamic data { get; set; }
        public string errorMessage { get; set; }
    }

    
}