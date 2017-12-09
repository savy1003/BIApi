using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biApi.Models
{


    public class TransListMulti
    {
        public bool hasError { get; set; }
        public int total_Count { get; set; }
        public dynamic data { get; set; }
        public dynamic data2 { get; set; }
        public string errorMessage { get; set; }
    }
}