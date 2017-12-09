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
using System.Collections;
using Newtonsoft.Json;

namespace biApi.Controllers
{
    public class PlanController : ApiController
    {
        [System.Web.Mvc.HttpGet]
        public TransList getPlan(int pg, int tk, string fnd)
        {
            int skip = tk * (pg - 1); // skip the record
            TransList transList = new TransList();

            using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
            {
                var plan = (from s in enBi.STG_PLAN
                                              select new {
                                                       s.ID
                                                      ,s.PLAN_DATE
                                                      ,s.GEO_LEVEL1_CODE
                                                      ,s.GEO_LEVEL2_CODE
                                                      ,s.GEO_LEVEL3_CODE
                                                      ,s.STORE_CODE
                                                      ,s.PROD_LEVEL1_CODE
                                                      ,s.PROD_LEVEL2_CODE
                                                      ,s.PROD_LEVEL3_CODE
                                                      ,s.PROD_LEVEL4_CODE
                                                      ,s.PROD_LEVEL5_CODE
                                                      ,s.PRODUCT_CODE
                                                      ,s.PLAN_VERSION
                                                      ,s.PLAN_SALE_QTY
                                                      ,s.PLAN_SALE_VAL_AT_PRICE
                                                      ,s.PLAN_SALE_VAL_AT_COST
                                                      ,s.PLAN_MARKDOWN_QTY
                                                      ,s.PLAN_MARKDOWN_VAL
                                                      ,s.PLAN_SHRINKAGE_VAL
                                                      ,s.PLAN_PURCHASE_QTY
                                                      ,s.PLAN_PURCHASE_VAL
                                                      ,s.PLAN_INV_OPENING_QTY
                                                      ,s.PLAN_OPENING_VAL_AT_COST
                                                      ,s.PLAN_OPENING_VAL_AT_PRICE
                                                      ,s.PLAN_OTH1
                                                      ,s.PLAN_OTH2
                                                      ,s.PLAN_OTH3
                                                      ,s.PLAN_OTH4
                                                      ,s.PLAN_OTH5
                                                      ,s.ARC_DATE
                                              })
                        .OrderBy(od => od.STORE_CODE)
                        .Skip(skip)
                        .Take(tk)
                        .ToList();

                transList.totalCount = enBi.STG_PLAN.AsNoTracking().Count();
                transList.data = plan;
                return transList;
            }
        }

        public TransListMulti getPlanByDate(int pg, int tk, string pSite, string pPlandate)
        {
            int skip = tk * (pg - 1); // skip the record
            TransListMulti transList1 = new TransListMulti();
            CultureInfo culture = new CultureInfo("en-US");
            DateTime Plandate = Convert.ToDateTime(pPlandate, culture);
            using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
            {
                var plan = (from s in enBi.STG_PLAN
                                 where s.STORE_CODE == pSite && s.PLAN_DATE == Plandate
                                 select new
                                 { 
                                     s.ID,
                                     s.PLAN_DATE,
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
                                     s.PLAN_VERSION,
                                     s.PLAN_SALE_QTY,
                                     s.PLAN_SALE_VAL_AT_PRICE,
                                     s.PLAN_SALE_VAL_AT_COST,
                                     s.PLAN_MARKDOWN_QTY,
                                     s.PLAN_MARKDOWN_VAL,
                                     s.PLAN_SHRINKAGE_VAL,
                                     s.PLAN_PURCHASE_QTY,
                                     s.PLAN_PURCHASE_VAL,
                                     s.PLAN_INV_OPENING_QTY,
                                     s.PLAN_OPENING_VAL_AT_COST,
                                     s.PLAN_OPENING_VAL_AT_PRICE,
                                     s.PLAN_OTH1,
                                     s.PLAN_OTH2,
                                     s.PLAN_OTH3,
                                     s.PLAN_OTH4 ,
                                     s.PLAN_OTH5,
                                     s.ARC_DATE
                                 })
                        .OrderBy(od => od.STORE_CODE)
                        .Skip(skip)
                        .Take(tk)
                        .ToList();

                transList1.total_Count = enBi.STG_PLAN.AsNoTracking().Where(wr => wr.STORE_CODE == pSite && wr.PLAN_DATE == Plandate).Count();
                transList1.data = plan;
                return transList1;
            }
        }
        public HttpResponseMessage addPlan(dynamic pPlan)
        {
            try
            {
                using (PRINCE_STGEntities dbCtx = new PRINCE_STGEntities())
                {
                    STG_PLAN plan = new STG_PLAN();
                        plan.PLAN_DATE = pPlan.PLAN_DATE;
                        plan.GEO_LEVEL1_CODE = "99999";
                        plan.GEO_LEVEL2_CODE = "99999";
                        plan.GEO_LEVEL3_CODE = "99999";
                        plan.STORE_CODE = pPlan.STORE_CODE;
                        plan.PROD_LEVEL1_CODE = pPlan.PROD_LEVEL1_CODE;
                        plan.PROD_LEVEL2_CODE = pPlan.PROD_LEVEL2_CODE;
                        plan.PROD_LEVEL3_CODE = pPlan.PROD_LEVEL3_CODE;
                        plan.PROD_LEVEL4_CODE = "99999";
                        plan.PROD_LEVEL5_CODE = "99999";
                        plan.PRODUCT_CODE = "99999";
                        plan.PLAN_VERSION = 99999;
                        plan.PLAN_SALE_QTY = 99999;
                        plan.PLAN_SALE_VAL_AT_PRICE = pPlan.PLAN_SALE_VAL_AT_PRICE;
                        plan.PLAN_SALE_VAL_AT_COST = 99999;
                        plan.PLAN_MARKDOWN_QTY = 0;
                        plan.PLAN_MARKDOWN_VAL = 0;
                        plan.PLAN_SHRINKAGE_VAL = 0;
                        plan.PLAN_PURCHASE_QTY = 0;
                        plan.PLAN_PURCHASE_VAL = 0;
                        plan.PLAN_INV_OPENING_QTY = 0;
                        plan.PLAN_OPENING_VAL_AT_COST = 0;
                        plan.PLAN_OPENING_VAL_AT_PRICE = 0;
                        plan.PLAN_OTH1 = 99999;
                        plan.PLAN_OTH2 = 99999;
                        plan.PLAN_OTH3 = 99999;
                        plan.PLAN_OTH4 = 99999;
                        plan.PLAN_OTH5 = 99999;
                        plan.ARC_DATE = DateTime.Now;

                    dbCtx.STG_PLAN.Add(plan);
                    dbCtx.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created, "Plan successfully created.");
                }
            }
            catch (Exception ex)
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }


        public HttpResponseMessage updatePlan(dynamic pPlan) {
            try
            {
                using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
                {
                    using (var dbCtxTran = enBi.Database.BeginTransaction())
                    {
                       //DateTime plandate = pPlan.PLAN_DATE;
                        int id = pPlan.ID;
                       //string storecode = pPlan.STORE_CODE;
                       STG_PLAN plan = enBi.STG_PLAN.Find(id);
                        if (plan != null)
                        {
                            plan.ID = pPlan.ID;
                            plan.PLAN_DATE = pPlan.PLAN_DATE;
                            plan.GEO_LEVEL1_CODE = pPlan.GEO_LEVEL1_CODE;
                            plan.GEO_LEVEL2_CODE = pPlan.GEO_LEVEL2_CODE;
                            plan.GEO_LEVEL3_CODE = pPlan.GEO_LEVEL3_CODE;
                            plan.STORE_CODE = pPlan.STORE_CODE;
                            plan.PROD_LEVEL1_CODE = pPlan.PROD_LEVEL1_CODE;
                            plan.PROD_LEVEL2_CODE = pPlan.PROD_LEVEL2_CODE;
                            plan.PROD_LEVEL3_CODE = pPlan.PROD_LEVEL3_CODE;
                            plan.PROD_LEVEL4_CODE = pPlan.PROD_LEVEL4_CODE;
                            plan.PROD_LEVEL5_CODE = pPlan.PROD_LEVEL5_CODE;
                            plan.PRODUCT_CODE = pPlan.PRODUCT_CODE;
                            plan.PLAN_VERSION = pPlan.PLAN_VERSION;
                            plan.PLAN_SALE_QTY = pPlan.PLAN_SALE_QTY;
                            plan.PLAN_SALE_VAL_AT_PRICE = pPlan.PLAN_SALE_VAL_AT_PRICE;
                            plan.PLAN_SALE_VAL_AT_COST = pPlan.PLAN_SALE_VAL_AT_COST;
                            plan.PLAN_MARKDOWN_QTY = pPlan.PLAN_MARKDOWN_QTY;
                            plan.PLAN_MARKDOWN_VAL = pPlan.PLAN_MARKDOWN_VAL;
                            plan.PLAN_SHRINKAGE_VAL = pPlan.PLAN_SHRINKAGE_VAL;
                            plan.PLAN_PURCHASE_QTY = pPlan.PLAN_PURCHASE_QTY;
                            plan.PLAN_PURCHASE_VAL = pPlan.PLAN_PURCHASE_VAL;
                            plan.PLAN_INV_OPENING_QTY = pPlan.PLAN_INV_OPENING_QTY;
                            plan.PLAN_OPENING_VAL_AT_COST = pPlan.PLAN_OPENING_VAL_AT_COST;
                            plan.PLAN_OPENING_VAL_AT_PRICE = pPlan.PLAN_OPENING_VAL_AT_PRICE;
                            plan.PLAN_OTH1 = pPlan.PLAN_OTH1;
                            plan.PLAN_OTH2 = pPlan.PLAN_OTH2;
                            plan.PLAN_OTH3 = pPlan.PLAN_OTH3;
                            plan.PLAN_OTH4 = pPlan.PLAN_OTH4;
                            plan.PLAN_OTH5 = pPlan.PLAN_OTH5;
                            plan.ARC_DATE = pPlan.ARC_DATE;
                            enBi.Entry(plan).State = System.Data.Entity.EntityState.Modified;
                            enBi.SaveChanges();
                            dbCtxTran.Commit();
                            return Request.CreateResponse(HttpStatusCode.OK, "Plan successfully Updated.");

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
            return Request.CreateResponse(HttpStatusCode.OK, "Plan successfully Updated.");
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

        public ActionResult SaveData(List<STG_PLAN> plan)
        {
            try
            {
                bool status = false;
                if (ModelState.IsValid)
                {

                    using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
                    {
                        foreach (var i in plan)
                        {
                            enBi.STG_PLAN.Add(i);
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
