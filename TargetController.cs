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
    public class TargetController : ApiController
    {
        public TransList getTarget(int pg, int tk, string fnd)
        {
            int skip = tk * (pg - 1); // skip the record
            TransList transList = new TransList();

            using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
            {
                var target = (from s in enBi.STG_TARGET
                                              select new {
                                                s.ID,
                                                s.TARGET_DATE,
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
                                                s.TARGET_SALE_QTY,
                                                s.TARGET_SALE_VAL_AT_PRICE,
                                                s.TARGET_SALE_VAL_AT_COST,
                                                s.ARC_DATE
                                              })
                        .OrderBy(od => od.STORE_CODE)
                        .Skip(skip)
                        .Take(tk)
                        .ToList();

                transList.totalCount = enBi.STG_TARGET.AsNoTracking().Count();
                transList.data = target;
                return transList;
            }
        }

        public TransList getTargetByDate(int pg, int tk, string fnd, string pSite, string sTargetdate)
        {
            int skip = tk * (pg - 1); // skip the record
            TransList transList = new TransList();
            CultureInfo culture = new CultureInfo("en-US");
            DateTime Targetdate = Convert.ToDateTime(sTargetdate, culture);
            using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
            {
                var Target = (from t in enBi.STG_TARGET
                                where t.STORE_CODE == pSite && t.TARGET_DATE == Targetdate
                                select new
                                {
                                    t.ID,
                                    t.TARGET_DATE,
                                    t.GEO_LEVEL1_CODE,
                                    t.GEO_LEVEL2_CODE,
                                    t.GEO_LEVEL3_CODE,
                                    t.STORE_CODE,
                                    t.PROD_LEVEL1_CODE,
                                    t.PROD_LEVEL2_CODE,
                                    t.PROD_LEVEL3_CODE,
                                    t.PROD_LEVEL4_CODE,
                                    t.PROD_LEVEL5_CODE,
                                    t.PRODUCT_CODE,
                                    t.TARGET_SALE_QTY,
                                    t.TARGET_SALE_VAL_AT_PRICE,
                                    t.TARGET_SALE_VAL_AT_COST,
                                    t.ARC_DATE
                                })
                        .OrderBy(od => od.STORE_CODE)
                        .Skip(skip)
                        .Take(tk)
                        .ToList();

                transList.totalCount = enBi.STG_TARGET.AsNoTracking().Where(st => st.TARGET_DATE == Targetdate && st.STORE_CODE == pSite).Count();
                transList.data = Target;
                return transList;
            }
        }

        public HttpResponseMessage addTarget(dynamic pTarget)
        {
            try
            {
                using (PRINCE_STGEntities dbCtx = new PRINCE_STGEntities())
                {
                    STG_TARGET target = new STG_TARGET();
                    target.TARGET_DATE = pTarget.TARGET_DATE;
                    target.GEO_LEVEL1_CODE = "99999";
                    target.GEO_LEVEL2_CODE = "99999";
                    target.GEO_LEVEL3_CODE = "99999";
                    target.STORE_CODE = pTarget.STORE_CODE;
                    target.PROD_LEVEL1_CODE = pTarget.PROD_LEVEL1_CODE;
                    target.PROD_LEVEL2_CODE = pTarget.PROD_LEVEL2_CODE;
                    target.PROD_LEVEL3_CODE = "99999";
                    target.PROD_LEVEL4_CODE = "99999";
                    target.PROD_LEVEL5_CODE = "99999";
                    target.PRODUCT_CODE = "99999";
                    target.TARGET_SALE_QTY = 99999;
                    target.TARGET_SALE_VAL_AT_PRICE = pTarget.TARGET_SALE_VAL_AT_PRICE;
                    target.TARGET_SALE_VAL_AT_COST = 99999;
                    target.ARC_DATE = DateTime.Now;
                    dbCtx.STG_TARGET.Add(target);
                    dbCtx.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created, "Target successfully created.");
                }
            }
            catch (Exception ex)
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }


        public HttpResponseMessage updateTarget(dynamic pTarget)
        {
            try
            {
                using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
                {
                    using (var dbCtxTran = enBi.Database.BeginTransaction())
                    {
                        //DateTime merchdate = sStoreMerch.merch_DATE;
                        int id = pTarget.ID;
                        //string storecode = sStoreMerch.STORE_CODE;
                        STG_TARGET target = enBi.STG_TARGET.Find(id);
                        if (target != null)
                        {
                            target.ID = pTarget.ID;
                            target.TARGET_DATE = pTarget.TARGET_DATE;
                            target.GEO_LEVEL1_CODE = pTarget.GEO_LEVEL1_CODE;
                            target.GEO_LEVEL2_CODE = pTarget.GEO_LEVEL2_CODE;
                            target.GEO_LEVEL3_CODE = pTarget.GEO_LEVEL3_CODE;
                            target.STORE_CODE = pTarget.STORE_CODE;
                            target.PROD_LEVEL1_CODE = pTarget.PROD_LEVEL1_CODE;
                            target.PROD_LEVEL2_CODE = pTarget.PROD_LEVEL2_CODE;
                            target.PROD_LEVEL3_CODE = pTarget.PROD_LEVEL3_CODE;
                            target.PROD_LEVEL4_CODE = pTarget.PROD_LEVEL4_CODE;
                            target.PROD_LEVEL5_CODE = pTarget.PROD_LEVEL5_CODE;
                            target.PRODUCT_CODE = pTarget.PRODUCT_CODE;
                            target.TARGET_SALE_QTY = pTarget.TARGET_SALE_QTY;
                            target.TARGET_SALE_VAL_AT_PRICE = pTarget.TARGET_SALE_VAL_AT_PRICE;
                            target.TARGET_SALE_VAL_AT_COST = pTarget.TARGET_SALE_VAL_AT_COST;
                            target.ARC_DATE = pTarget.ARC_DATE;
                            enBi.Entry(target).State = System.Data.Entity.EntityState.Modified;
                            enBi.SaveChanges();
                            dbCtxTran.Commit();
                            return Request.CreateResponse(HttpStatusCode.OK, "Target successfully Updated.");

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
            return Request.CreateResponse(HttpStatusCode.OK, "Target successfully Updated.");
        }

        public ActionResult SaveData(List<STG_TARGET> target)
        {
            try
            {
                bool status = false;
                if (ModelState.IsValid)
                {

                    using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
                    {
                        foreach (var i in target)
                        {
                            enBi.STG_TARGET.Add(i);
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
