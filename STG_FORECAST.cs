//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace biApi
{
    using System;
    using System.Collections.Generic;
    
    public partial class STG_FORECAST
    {
        public int ID { get; set; }
        public System.DateTime FORECAST_DATE { get; set; }
        public string GEO_LEVEL1_CODE { get; set; }
        public string GEO_LEVEL2_CODE { get; set; }
        public string GEO_LEVEL3_CODE { get; set; }
        public string STORE_CODE { get; set; }
        public string PROD_LEVEL1_CODE { get; set; }
        public string PROD_LEVEL2_CODE { get; set; }
        public string PROD_LEVEL3_CODE { get; set; }
        public string PROD_LEVEL4_CODE { get; set; }
        public string PROD_LEVEL5_CODE { get; set; }
        public string PRODUCT_CODE { get; set; }
        public decimal FORECAST_SALE_QTY { get; set; }
        public Nullable<decimal> FORECAST_SALE_VAL_AT_PRICE { get; set; }
        public Nullable<decimal> FORECAST_SALE_VAL_AT_COST { get; set; }
        public Nullable<System.DateTime> ARC_DATE { get; set; }
    }
}
