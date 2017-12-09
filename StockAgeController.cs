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
    public class StockAgeController : ApiController
    {
        public TransList getStockAge(int pg, int tk)
        {
            int skip = tk * (pg - 1); // skip the record
            TransList transList = new TransList();

            using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
            {
                var stockage = (from s in enBi.STG_DIM_STOCKAGE_BAND
                                              select new {

                                                  s.ID,
                                                  s.STOCKAGE_BAND_DESC,
                                                  s.STKAGE_PROD_LEVEL1_CODE,
                                                  s.STOCKAGE_BAND_LOWER,
                                                  s.STOCKAGE_BAND_UPPER,
                                                  s.STOCKAGE_BAND_SEQ,
                                                  s.LATEST,
                                                  s.ARC_DATE
                                              })
                        .OrderBy(od => od.STOCKAGE_BAND_DESC)
                        .Skip(skip)
                        .Take(tk)
                        .ToList();

                transList.totalCount = enBi.STG_DIM_STOCKAGE_BAND.AsNoTracking().Count();
                transList.data = stockage;
                return transList;
            }
        }
       
        public TransList getStockAgeByCode(int pg, int tk, string fnd, string pCode)
        {
            int skip = tk * (pg - 1); // skip the record
            TransList transList = new TransList();
            using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
            {
                var stockage = (from s in enBi.STG_DIM_STOCKAGE_BAND
                                where s.STKAGE_PROD_LEVEL1_CODE == pCode
                                 select new
                                 {
                                     s.ID,
                                     s.STOCKAGE_BAND_DESC,
                                     s.STKAGE_PROD_LEVEL1_CODE,
                                     s.STOCKAGE_BAND_LOWER,
                                     s.STOCKAGE_BAND_UPPER,
                                     s.STOCKAGE_BAND_SEQ,
                                     s.LATEST,
                                     s.ARC_DATE
                                   
                                 })
                        .OrderBy(od => od.STOCKAGE_BAND_DESC)
                        .Skip(skip)
                        .Take(tk)
                        .ToList();

                transList.totalCount = enBi.STG_DIM_STOCKAGE_BAND.AsNoTracking().Where(st => st.STKAGE_PROD_LEVEL1_CODE == pCode).Count();
                transList.data = stockage;
                return transList;
            }
        }
       
        public HttpResponseMessage updateStockAge(dynamic sStockAge) {
            try
            {
                using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
                {
                    using (var dbCtxTran = enBi.Database.BeginTransaction())
                    {
                       //DateTime merchdate = sStoreMerch.merch_DATE;
                        int id = sStockAge.ID;
                        string bndDesc = sStockAge.PRICE_BAND_CODE;
                        string lvlCode = sStockAge.PROD_LEVEL1_CODE;
                       //string storecode = sStoreMerch.STORE_CODE;
                        STG_DIM_STOCKAGE_BAND stockage = enBi.STG_DIM_STOCKAGE_BAND.Find(id, bndDesc, lvlCode);
                        if (stockage != null)
                        {
                            stockage.ID = sStockAge.ID;
                            stockage.STOCKAGE_BAND_DESC = sStockAge.PRICE_BAND_DESC;
                            stockage.STKAGE_PROD_LEVEL1_CODE = sStockAge.PROD_LEVEL1_CODE;
                            stockage.STOCKAGE_BAND_LOWER = sStockAge.PRICE_BAND_LOWER;
                            stockage.STOCKAGE_BAND_UPPER = sStockAge.PRICE_BAND_UPPER;
                            stockage.STOCKAGE_BAND_SEQ = sStockAge.STOCKAGE_BAND_SEQ;
                            stockage.LATEST = sStockAge.LATEST;
                            stockage.ARC_DATE = sStockAge.ARC_DATE;
                            enBi.Entry(stockage).State = System.Data.Entity.EntityState.Modified;
                            enBi.SaveChanges();
                            dbCtxTran.Commit();
                            return Request.CreateResponse(HttpStatusCode.OK, "Price Band successfully Updated.");

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
            return Request.CreateResponse(HttpStatusCode.OK, "Price Band successfully Updated.");
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
        public ActionResult SaveData(List<STG_DIM_STOCKAGE_BAND> stockage)
        {
            try
            {
                bool status = false;
                if (ModelState.IsValid)
                {

                    using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
                    {
                        foreach (var i in stockage)
                        {
                            enBi.STG_DIM_STOCKAGE_BAND.Add(i);
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
