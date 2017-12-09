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
    public class PriceBandController : ApiController
    {
        public TransList getPrice(int pg, int tk)
        {
            int skip = tk * (pg - 1); // skip the record
            TransList transList = new TransList();

            using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
            {
                var priceband = (from s in enBi.STG_DIM_PRICE_BAND
                                              select new {

                                                      s.ID,
                                                      s.PRICE_BAND_CODE,
                                                      s.PRICE_BAND_DESC,
                                                      s.PROD_LEVEL1_CODE,
                                                      s.PRICE_BAND_LOWER,
                                                      s.PRICE_BAND_UPPER,
                                                      s.PRICE_BAND_SEQ,
                                                      s.ARC_DATE
                                              })
                        .OrderBy(od => od.PRICE_BAND_CODE)
                        .Skip(skip)
                        .Take(tk)
                        .ToList();

                transList.totalCount = enBi.STG_DIM_PRICE_BAND.AsNoTracking().Count();
                transList.data = priceband;
                return transList;
            }
        }
       
        public TransList getPriceByDate(int pg, int tk, string fnd, string pCode)
        {
            int skip = tk * (pg - 1); // skip the record
            TransList transList = new TransList();
            using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
            {
                var priceband = (from p in enBi.STG_DIM_PRICE_BAND
                                 where p.PROD_LEVEL1_CODE == pCode
                                 select new
                                 {
                                     p.ID,
                                     p.PRICE_BAND_CODE,
                                     p.PRICE_BAND_DESC,
                                     p.PROD_LEVEL1_CODE,
                                     p.PRICE_BAND_LOWER,
                                     p.PRICE_BAND_UPPER,
                                     p.PRICE_BAND_SEQ,
                                     p.ARC_DATE
                                   
                                 })
                        .OrderBy(od => od.PRICE_BAND_CODE)
                        .Skip(skip)
                        .Take(tk)
                        .ToList();

                transList.totalCount = enBi.STG_DIM_PRICE_BAND.AsNoTracking().Where(st => st.PROD_LEVEL1_CODE == pCode).Count();
                transList.data = priceband;
                return transList;
            }
        }
       
        public HttpResponseMessage updatePriceBand(dynamic pPriceBand) {
            try
            {
                using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
                {
                    using (var dbCtxTran = enBi.Database.BeginTransaction())
                    {
                       //DateTime merchdate = sStoreMerch.merch_DATE;
                        int id = pPriceBand.ID;
                        string bndCode = pPriceBand.PRICE_BAND_CODE;
                        string lvlCode = pPriceBand.PROD_LEVEL1_CODE;
                       //string storecode = sStoreMerch.STORE_CODE;
                        STG_DIM_PRICE_BAND priceband = enBi.STG_DIM_PRICE_BAND.Find(id, bndCode, lvlCode);
                        if (priceband != null)
                        {
                            priceband.ID = pPriceBand.ID;
                            priceband.PRICE_BAND_CODE = pPriceBand.PRICE_BAND_CODE;
                            priceband.PRICE_BAND_DESC = pPriceBand.PRICE_BAND_DESC;
                            priceband.PROD_LEVEL1_CODE = pPriceBand.PROD_LEVEL1_CODE;
                            priceband.PRICE_BAND_LOWER = pPriceBand.PRICE_BAND_LOWER;
                            priceband.PRICE_BAND_UPPER = pPriceBand.PRICE_BAND_UPPER;
                            priceband.PRICE_BAND_SEQ = pPriceBand.PRICE_BAND_SEQ;
                            priceband.ARC_DATE = pPriceBand.ARC_DATE;
                            enBi.Entry(priceband).State = System.Data.Entity.EntityState.Modified;
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
        public ActionResult SaveData(List<STG_DIM_PRICE_BAND> priceband)
        {
            try
            {
                bool status = false;
                if (ModelState.IsValid)
                {

                    using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
                    {
                        foreach (var i in priceband)
                        {
                            enBi.STG_DIM_PRICE_BAND.Add(i);
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
