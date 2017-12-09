using biApi.ErrorHelper;
using biApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;

namespace biApi.Controllers
{
    
    public class FootfallController : ApiController
    {
        public TransList getFootfall(int pg, int tk, string fnd)
        {
            int skip = tk * (pg - 1); // skip the record
            TransList transList = new TransList();

            using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
            {
                var tempStore = (from s in enBi.STG_STR_FOOTFALL
                                              select new {
                                                        s.FOOTFALL_DATE,
                                                        s.STORE_CODE,
                                                        s.FOOTFALL_COUNT,
                                                        s.ARC_DATE
                                              })
                        .OrderBy(od => od.STORE_CODE)
                        .Skip(skip)
                        .Take(tk)
                        .ToList();

                transList.totalCount = enBi.STG_STR_FOOTFALL.AsNoTracking().Count();
                transList.data = tempStore;
                return transList;
            }
        }

        public HttpResponseMessage addFootfall(dynamic pFootfall)
        {
            try
            {
                using (PRINCE_STGEntities dbCtx = new PRINCE_STGEntities())
                {
                    STG_STR_FOOTFALL foot = new STG_STR_FOOTFALL();
                    foot.FOOTFALL_DATE = pFootfall.FOOTFALL_DATE;
                    foot.STORE_CODE = pFootfall.STORE_CODE;
                    foot.FOOTFALL_COUNT = pFootfall.FOOTFALL_COUNT;
                    foot.ARC_DATE = DateTime.Now;
                    dbCtx.STG_STR_FOOTFALL.Add(foot);
                    dbCtx.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created, "Footfall  successfully created.");
                }
            }
            //catch (DbEntityValidationException ex)
            //{
            //    ExceptionEntity exDesc = new ExceptionEntity(ex);
            //    throw new ApiException() { HttpStatus = HttpStatusCode.BadRequest, ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = exDesc.GetDescException() };
            //}
            //catch (Exception ex)
            //{
            //    ExceptionDescription exDesc = new ExceptionDescription(ex);
            //    throw new ApiException() { HttpStatus = HttpStatusCode.BadRequest, ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = exDesc.GetDescException() };
            //}

            catch (Exception ex)
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }


        public HttpResponseMessage updateFootfall(StoreDc pStore) {
            try
            {
                using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
                {
                    using (var dbCtxTran = enBi.Database.BeginTransaction())
                    {
                        TEMP_STORE store = enBi.TEMP_STORE.Where(wr => wr.STORE_CODE == pStore.STORE_CODE).FirstOrDefault();
                        if (store!=null) {
                            store.STORE_DESCRIPTION = pStore.STORE_DESCRIPTION;
                            store.AREA_MANAGER = pStore.AREA_MANAGER;
                            store.AREA_MANAGER_DESC = pStore.AREA_MANAGER_DESC;
                            store.REGIONAL_MANAGER = pStore.REGIONAL_MANAGER;
                            store.REGIONAL_MANAGER_DESC = pStore.REGIONAL_MANAGER_DESC;
                            store.SOM = pStore.SOM;
                            store.SOM_DESC = pStore.SOM_DESC;
                            store.SENIOR_SOM = pStore.SENIOR_SOM;
                            store.SENIOR_SOM_DESC = pStore.SENIOR_SOM_DESC;
                            store.STORE_OPENING_HOUR = pStore.STORE_OPENING_HOUR;
                            store.STORE_CLOSING_HOUR = pStore.STORE_CLOSING_HOUR;
                            store.STORE_LATITUDE = pStore.STORE_LATITUDE;
                            store.STORE_LONGITUDE = pStore.STORE_LONGITUDE;
                            enBi.Entry(store).State = EntityState.Modified;
                            enBi.SaveChanges();
                            dbCtxTran.Commit();
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
            return Request.CreateResponse(HttpStatusCode.OK);
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

       
        public ActionResult SaveData(List<STG_STR_FOOTFALL> footfall)
        {
          try
            {
            bool status = false;
            if (ModelState.IsValid)
            {
                
                using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
                {
                    foreach (var i in footfall)
                    {
                        enBi.STG_STR_FOOTFALL.Add(i);
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
