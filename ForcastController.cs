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
    public class ForcastController : ApiController
    {
        public TransList getForcast(int pg, int tk, string fnd)
        {
            int skip = tk * (pg - 1); // skip the record
            TransList transList = new TransList();

            using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
            {
                var tempStore = (from s in enBi.STG_FORECAST
                                              select new {
                                                s.ID,
                                                s.FORECAST_DATE,
                                                s.GEO_LEVEL1_CODE,
                                                s.GEO_LEVEL2_CODE,
                                                s.GEO_LEVEL3_CODE,
                                                s.STORE_CODE,
                                                s.PROD_LEVEL1_CODE,
                                                s.PROD_LEVEL2_CODE,
                                                s.PROD_LEVEL3_CODE,
                                                s.PROD_LEVEL4_CODE,
                                                s.PROD_LEVEL5_CODE,
                                                s.PRODUCT_CODE,
                                                s.FORECAST_SALE_QTY,
                                                s.FORECAST_SALE_VAL_AT_PRICE,
                                                s.FORECAST_SALE_VAL_AT_COST,
                                                s.ARC_DATE
                                              })
                        .OrderBy(od => od.STORE_CODE)
                        .Skip(skip)
                        .Take(tk)
                        .ToList();

                transList.totalCount = enBi.STG_FORECAST.AsNoTracking().Count();
                transList.data = tempStore;
                return transList;
            }
        }

        public TransList getForecastByDate(int pg, int tk, string fnd, string pSite, string sForecastdate)
        {
            int skip = tk * (pg - 1); // skip the record
            TransList transList = new TransList();
            CultureInfo culture = new CultureInfo("en-US");
            DateTime Forecastdate = Convert.ToDateTime(sForecastdate, culture);
            using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
            {
                var Forecast = (from f in enBi.STG_FORECAST
                                  where f.STORE_CODE == pSite && f.FORECAST_DATE == Forecastdate
                                  select new
                                  {
                                     f.ID
                                    ,f.FORECAST_DATE
                                    ,f.GEO_LEVEL1_CODE
                                    ,f.GEO_LEVEL2_CODE
                                    ,f.GEO_LEVEL3_CODE
                                    ,f.STORE_CODE
                                    ,f.PROD_LEVEL1_CODE
                                    ,f.PROD_LEVEL2_CODE
                                    ,f.PROD_LEVEL3_CODE
                                    ,f.PROD_LEVEL4_CODE
                                    ,f.PROD_LEVEL5_CODE
                                    ,f.PRODUCT_CODE
                                    ,f.FORECAST_SALE_QTY
                                    ,f.FORECAST_SALE_VAL_AT_PRICE
                                    ,f.FORECAST_SALE_VAL_AT_COST
                                    ,f.ARC_DATE
                                  })
                        .OrderBy(od => od.STORE_CODE)
                        .Skip(skip)
                        .Take(tk)
                        .ToList();

                transList.totalCount = enBi.STG_FORECAST.AsNoTracking().Where(st => st.FORECAST_DATE == Forecastdate && st.STORE_CODE == pSite).Count();
                transList.data = Forecast;
                return transList;
            }
        }

        public HttpResponseMessage addForecast(dynamic pForecast)
        {
            try
            {
                using (PRINCE_STGEntities dbCtx = new PRINCE_STGEntities())
                {
                    STG_FORECAST forecast = new STG_FORECAST();
                    forecast.FORECAST_DATE = pForecast.FORECAST_DATE;
                    forecast.GEO_LEVEL1_CODE = "99999";
                    forecast.GEO_LEVEL2_CODE = "99999";
                    forecast.GEO_LEVEL3_CODE = "99999";
                    forecast.STORE_CODE = pForecast.STORE_CODE;
                    forecast.PROD_LEVEL1_CODE = pForecast.PROD_LEVEL1_CODE;
                    forecast.PROD_LEVEL2_CODE = pForecast.PROD_LEVEL2_CODE;
                    forecast.PROD_LEVEL3_CODE = "99999";
                    forecast.PROD_LEVEL4_CODE = "99999";
                    forecast.PROD_LEVEL5_CODE = "99999";
                    forecast.PRODUCT_CODE =  "99999";
                    forecast.FORECAST_SALE_QTY = 99999;
                    forecast.FORECAST_SALE_VAL_AT_PRICE = pForecast.FORECAST_SALE_VAL_AT_PRICE;
                    forecast.FORECAST_SALE_VAL_AT_COST = 99999;
                    forecast.ARC_DATE = DateTime.Now;
                    dbCtx.STG_FORECAST.Add(forecast);
                    dbCtx.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created, "Forecast successfully created.");
                }
            }
            catch (Exception ex)
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage updateForecast(dynamic pForecast)
        {
            try
            {
                using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
                {
                    using (var dbCtxTran = enBi.Database.BeginTransaction())
                    {
                        //DateTime merchdate = sStoreMerch.merch_DATE;
                        int id = pForecast.ID;
                        //string storecode = sStoreMerch.STORE_CODE;
                        STG_FORECAST forecast = enBi.STG_FORECAST.Find(id);
                        if (forecast != null)
                        {
                            forecast.ID = pForecast.ID;
                            forecast.FORECAST_DATE = pForecast.FORECAST_DATE;
                            forecast.GEO_LEVEL1_CODE = pForecast.GEO_LEVEL1_CODE;
                            forecast.GEO_LEVEL2_CODE = pForecast.GEO_LEVEL2_CODE;
                            forecast.GEO_LEVEL3_CODE = pForecast.GEO_LEVEL3_CODE;
                            forecast.STORE_CODE = pForecast.STORE_CODE;
                            forecast.PROD_LEVEL1_CODE = pForecast.PROD_LEVEL1_CODE;
                            forecast.PROD_LEVEL2_CODE = pForecast.PROD_LEVEL2_CODE;
                            forecast.PROD_LEVEL3_CODE = pForecast.PROD_LEVEL3_CODE;
                            forecast.PROD_LEVEL4_CODE = pForecast.PROD_LEVEL4_CODE;
                            forecast.PROD_LEVEL5_CODE = pForecast.PROD_LEVEL5_CODE;
                            forecast.PRODUCT_CODE = pForecast.PRODUCT_CODE;
                            forecast.FORECAST_SALE_QTY = pForecast.FORECAST_SALE_QTY;
                            forecast.FORECAST_SALE_VAL_AT_PRICE = pForecast.FORECAST_SALE_VAL_AT_PRICE;
                            forecast.FORECAST_SALE_VAL_AT_COST = pForecast.FORECAST_SALE_VAL_AT_COST;
                            forecast.ARC_DATE = pForecast.ARC_DATE;
                            enBi.Entry(forecast).State = System.Data.Entity.EntityState.Modified;
                            enBi.SaveChanges();
                            dbCtxTran.Commit();
                            return Request.CreateResponse(HttpStatusCode.OK, "Forecast successfully Updated.");

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
            return Request.CreateResponse(HttpStatusCode.OK, "Forecast successfully Updated.");
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
        public ActionResult SaveData(List<STG_FORECAST> forecast)
        {
            try
            {
                bool status = false;
                if (ModelState.IsValid)
                {

                    using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
                    {
                        foreach (var i in forecast)
                        {
                            enBi.STG_FORECAST.Add(i);
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
