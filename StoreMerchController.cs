using biApi.ErrorHelper;
using biApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace biApi.Controllers
{
    public class StoreMerchController : ApiController
    {
        public TransList getMerch(int pg, int tk)
        {
            int skip = tk * (pg - 1); // skip the record
            TransList transList = new TransList();

            using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
            {
                var tempStore = (from s in enBi.STG_STR_MERCH_DET
                                              select new {

                                                       s.ID 
                                                      ,s.STRMRGRP_DATE
                                                      ,s.STORE_CODE
                                                      ,s.PROD_LEVEL1_CODE
                                                      ,s.PROD_LEVEL2_CODE
                                                      ,s.PROD_LEVEL3_CODE
                                                      ,s.PROD_LEVEL4_CODE
                                                      ,s.SELLING_AREA
                                                      ,s.TOTAL_AREA
                                                      ,s.PERMENANT_FTE
                                                      ,s.CONTRACT_FTE
                                                      ,s.OTH_DET1
                                                      ,s.OTH_DET2
                                                      ,s.OTH_DET3
                                                      ,s.OTH_DET4
                                                      ,s.OTH_DET5
                                                      ,s.ARC_DATE
                                              })
                        .OrderBy(od => od.STORE_CODE)
                        .Skip(skip)
                        .Take(tk)
                        .ToList();

                transList.totalCount = enBi.STG_STR_MERCH_DET.AsNoTracking().Count();
                transList.data = tempStore;
                return transList;
            }
        }
       
        public TransList getStoreMerchByDate(int pg, int tk, string fnd, string pSite, string sStoreMerchdate)
        {
            int skip = tk * (pg - 1); // skip the record
            TransList transList = new TransList();
            CultureInfo culture = new CultureInfo("en-US");
            DateTime StoreMerchdate = Convert.ToDateTime(sStoreMerchdate, culture);
            using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
            {
                var Storemerch = (from s in enBi.STG_STR_MERCH_DET
                                 where s.STORE_CODE == pSite && s.STRMRGRP_DATE == StoreMerchdate
                                 select new
                                 { 
                                    s.ID,
                                    s.STRMRGRP_DATE
                                    ,s.STORE_CODE
                                    ,s.PROD_LEVEL1_CODE
                                    ,s.PROD_LEVEL2_CODE
                                    ,s.PROD_LEVEL3_CODE
                                    ,s.PROD_LEVEL4_CODE
                                    ,s.SELLING_AREA
                                    ,s.TOTAL_AREA
                                    ,s.PERMENANT_FTE
                                    ,s.CONTRACT_FTE
                                    ,s.OTH_DET1
                                    ,s.OTH_DET2
                                    ,s.OTH_DET3
                                    ,s.OTH_DET4
                                    ,s.OTH_DET5
                                    ,s.ARC_DATE
                                 })
                        .OrderBy(od => od.STORE_CODE)
                        .Skip(skip)
                        .Take(tk)
                        .ToList();

                transList.totalCount = enBi.STG_STR_MERCH_DET.AsNoTracking().Where(st => st.STRMRGRP_DATE == StoreMerchdate && st.STORE_CODE == pSite).Count();
                transList.data = Storemerch;
                return transList;
            }
        }
        //public HttpResponseMessage addStoreMerch(dynamic sStoreMerch)
        //{
        //    try
        //    {
        //        using (PRINCE_STGEntities dbCtx = new PRINCE_STGEntities())
        //        {
        //            STG_merch merch = new STG_merch();
        //                merch.merch_DATE = sStoreMerch.merch_DATE;
        //                merch.GEO_LEVEL1_CODE = "99999";
        //                merch.GEO_LEVEL2_CODE = "99999";
        //                merch.GEO_LEVEL3_CODE = "99999";
        //                merch.STORE_CODE = sStoreMerch.STORE_CODE;
        //                merch.PROD_LEVEL1_CODE = sStoreMerch.PROD_LEVEL1_CODE;
        //                merch.PROD_LEVEL2_CODE = sStoreMerch.PROD_LEVEL2_CODE;
        //                merch.PROD_LEVEL3_CODE = sStoreMerch.PROD_LEVEL3_CODE;
        //                merch.PROD_LEVEL4_CODE = "99999";
        //                merch.PROD_LEVEL5_CODE = "99999";
        //                merch.PRODUCT_CODE = "99999";
        //                merch.merch_VERSION = 99999;
        //                merch.merch_SALE_QTY = 99999;
        //                merch.merch_SALE_VAL_AT_PRICE = sStoreMerch.merch_SALE_VAL_AT_PRICE;
        //                merch.merch_SALE_VAL_AT_COST = 99999;
        //                merch.merch_MARKDOWN_QTY = 0;
        //                merch.merch_MARKDOWN_VAL = 0;
        //                merch.merch_SHRINKAGE_VAL = 0;
        //                merch.merch_PURCHASE_QTY = 0;
        //                merch.merch_PURCHASE_VAL = 0;
        //                merch.merch_INV_OPENING_QTY = 0;
        //                merch.merch_OPENING_VAL_AT_COST = 0;
        //                merch.merch_OPENING_VAL_AT_PRICE = 0;
        //                merch.merch_OTH1 = 99999;
        //                merch.merch_OTH2 = 99999;
        //                merch.merch_OTH3 = 99999;
        //                merch.merch_OTH4 = 99999;
        //                merch.merch_OTH5 = 99999;
        //                merch.ARC_DATE = DateTime.Now;

        //            dbCtx.STG_merch.Add(merch);
        //            dbCtx.SaveChanges();
        //            return Request.CreateResponse(HttpStatusCode.Created, "merch successfully created.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK);
        //}


        public HttpResponseMessage updateStoreMerch(dynamic sStoreMerch) {
            try
            {
                using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
                {
                    using (var dbCtxTran = enBi.Database.BeginTransaction())
                    {
                       //DateTime merchdate = sStoreMerch.merch_DATE;
                        int id = sStoreMerch.ID;
                       //string storecode = sStoreMerch.STORE_CODE;
                        STG_STR_MERCH_DET merch = enBi.STG_STR_MERCH_DET.Find(id);
                        if (merch != null)
                        {
                            merch.ID = sStoreMerch.ID;
                            merch.STRMRGRP_DATE = sStoreMerch.STRMRGRP_DATE;
                            merch.STORE_CODE = sStoreMerch.STORE_CODE;
                            merch.PROD_LEVEL1_CODE = sStoreMerch.PROD_LEVEL1_CODE;
                            merch.PROD_LEVEL2_CODE = sStoreMerch.PROD_LEVEL2_CODE;
                            merch.PROD_LEVEL3_CODE = sStoreMerch.PROD_LEVEL3_CODE;
                            merch.PROD_LEVEL4_CODE = sStoreMerch.PROD_LEVEL4_CODE;
                            merch.SELLING_AREA = sStoreMerch.SELLING_AREA;
                            merch.TOTAL_AREA = sStoreMerch.TOTAL_AREA;
                            merch.PERMENANT_FTE = sStoreMerch.PERMENANT_FTE;
                            merch.CONTRACT_FTE = sStoreMerch.CONTRACT_FTE;
                            merch.OTH_DET1 = sStoreMerch.OTH_DET1;
                            merch.OTH_DET2 = sStoreMerch.OTH_DET2;
                            merch.OTH_DET3 = sStoreMerch.OTH_DET3;
                            merch.OTH_DET4 = sStoreMerch.OTH_DET4;
                            merch.OTH_DET5 = sStoreMerch.OTH_DET5;
                            merch.ARC_DATE = sStoreMerch.ARC_DATE;
                            enBi.Entry(merch).State = System.Data.Entity.EntityState.Modified;
                            enBi.SaveChanges();
                            dbCtxTran.Commit();
                            return Request.CreateResponse(HttpStatusCode.OK, "Store Merch successfully Updated.");

                        }
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                ExceptionEntity exDesc = new ExceptionEntity(ex);
                throw new ApiException() { HttpStatus = HttpStatusCode.BadRequest, ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = exDesc.GetDescException() };
            }
            catch (Exception ex)
            {
                ExceptionDescription exDesc = new ExceptionDescription(ex);
                throw new ApiException() { HttpStatus = HttpStatusCode.BadRequest, ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = exDesc.GetDescException() };
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Storemerch successfully Updated.");
        }

        public List<TEMP_DC> getStoreDc() {
            try
            {
                TransList trans = new TransList();
                using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
                {
                    List<TEMP_DC> listDc = enBi.TEMP_DC.AsNoTracking().ToList();
                  //  trans.data = listDc;
                    return listDc;
                }
            }
            catch (DbEntityValidationException ex)
            {
                ExceptionEntity exDesc = new ExceptionEntity(ex);
                throw new ApiException() { HttpStatus = HttpStatusCode.BadRequest, ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = exDesc.GetDescException() };
            }
            catch (Exception ex)
            {
                ExceptionDescription exDesc = new ExceptionDescription(ex);
                throw new ApiException() { HttpStatus = HttpStatusCode.BadRequest, ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = exDesc.GetDescException() };
            }
        }
        public ActionResult SaveData(List<STG_STR_MERCH_DET> storemerch)
        {
            try
            {
                bool status = false;
                if (ModelState.IsValid)
                {

                    using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
                    {
                        foreach (var i in storemerch)
                        {
                            enBi.STG_STR_MERCH_DET.Add(i);
                        }
                        enBi.SaveChanges();
                        status = true;
                    }
                }
                return new JsonResult { Data = new { status = status } };
            }
            catch (DbEntityValidationException ex)
            {
                ExceptionEntity exDesc = new ExceptionEntity(ex);
                throw new ApiException() { HttpStatus = HttpStatusCode.BadRequest, ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = exDesc.GetDescException() };
            }
            catch (Exception ex)
            {
                ExceptionDescription exDesc = new ExceptionDescription(ex);
                throw new ApiException() { HttpStatus = HttpStatusCode.BadRequest, ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = exDesc.GetDescException() };
            }
        }

        public class StoreDc : TEMP_STORE {
            public string DC_STORE_CODE { get; set; }
            public string DC_STORE_DESC { get; set; }
        }
    }
}
