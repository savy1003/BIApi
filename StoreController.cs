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

namespace biApi.Controllers
{
    public class StoreController : ApiController
    {
        public TransList getStore(int pg, int tk, string fnd)
        {
            int skip = tk * (pg - 1); // skip the record
            TransList transList = new TransList();

            using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
            {
                //i include the dc of store
                //current design is 1 store only have 1 dc(for manthan table design purpose only) but in actual there are multiple dc in 1 store
                var tempStore = (from s in enBi.TEMP_STORE
                                              select new {
                                              s.ID,
                                              s.STORE_CODE,
                                              s.STORE_DESCRIPTION,
                                              s.AREA_MANAGER,
                                              s.AREA_MANAGER_DESC,
                                              s.REGIONAL_MANAGER,
                                              s.REGIONAL_MANAGER_DESC,
                                              s.SOM,
                                              s.SOM_DESC,
                                              s.SENIOR_SOM,
                                              s.SENIOR_SOM_DESC,
                                              s.STORE_OPENING_HOUR,
                                              s.STORE_CLOSING_HOUR,
                                              s.STORE_LATITUDE,
                                              s.STORE_LONGITUDE,
                                              DC_STORE_CODE = (from dc in enBi.TEMP_STOREDC where dc.STORE_CODE == s.STORE_CODE select dc.DC_STORE_CODE).FirstOrDefault(),
                                              DC_STORE_DESC = (from storeDc in enBi.TEMP_STOREDC join dc in enBi.TEMP_DC on storeDc.DC_STORE_CODE equals dc.DC_STORE_CODE where storeDc.STORE_CODE == s.STORE_CODE select   dc.DC_STORE_DESCRIPTION  ).FirstOrDefault()
                                              })
                        .OrderBy(od => od.STORE_CODE)
                        .Skip(skip)
                        .Take(tk)
                        .ToList();

                transList.totalCount = enBi.TEMP_STORE.AsNoTracking().Count();
                transList.data = tempStore;
                return transList;
            }
        }

        public HttpResponseMessage updateStore(StoreDc pStore) {
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
        //public HttpResponseMessage addStore(TEMP_STORE pStore)
        //{
        //    try
        //    {
        //        using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
        //        {
        //            enBi.TEMP_STORE.Add(pStore);
        //            enBi.SaveChanges();
        //        }
        //        return Request.CreateResponse(HttpStatusCode.Created);
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        ExceptionEntity exDesc = new ExceptionEntity(ex);
        //        throw new ApiException() { HttpStatus = HttpStatusCode.BadRequest, ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = exDesc.GetDescException() };
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionDescription exDesc = new ExceptionDescription(ex);
        //        throw new ApiException() { HttpStatus = HttpStatusCode.BadRequest, ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = exDesc.GetDescException() };
        //    }
        //}

        [HttpGet]
        public HttpResponseMessage getSiteById(int id)
        {
            try
            {
                using (PRINCE_STGEntities enCarina = new PRINCE_STGEntities())
                {
                    enCarina.Configuration.LazyLoadingEnabled = false;
                    Site site = enCarina.Site.Where(wr => wr.SiteId == id).SingleOrDefault();
                    return Request.CreateResponse(HttpStatusCode.OK, site);
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

        [HttpGet]
        public HttpResponseMessage getSite()
        {
            try
            {
                using (PRINCE_STGEntities enBI = new PRINCE_STGEntities())
                {
                    enBI.Configuration.LazyLoadingEnabled = false;
                    List<Site> siteList = enBI.Site.Where(a => a.Status == "A").OrderBy(od => od.Description).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, siteList);
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

        [HttpGet]
        public TransList getSite(int pg, int tk, string fnd)
        {
            try
            {
                int skip = tk * (pg - 1); // skip the record
                TransList transList = new TransList();
                using (PRINCE_STGEntities db = new PRINCE_STGEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    if (string.IsNullOrEmpty(fnd))
                    {
                        List<Site> site = db.Site.AsNoTracking()
                            .OrderBy(od => od.Description)
                        .Skip(skip)
                        .Take(tk)
                        .ToList();
                        transList.totalCount = db.Site.AsNoTracking().Count();
                        transList.data = site;
                    }
                    else
                    {
                        List<Site> site = db.Site.AsNoTracking()
                        .Where(wr => wr.Description.Contains(fnd) || wr.Status.Contains(fnd))
                            .OrderBy(od => od.Description)
                            .Skip(skip)
                            .Take(tk)
                            .ToList();
                        transList.totalCount = db.Site.AsNoTracking().Where(wr => wr.Description.Contains(fnd) || wr.Status.Contains(fnd)).Count();
                        transList.data = site;
                    }
                }
                return transList;

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

        [HttpPost]
        public HttpResponseMessage addSite(dynamic pSite)
        {
            try
            {
                using (PRINCE_STGEntities dbCtx = new PRINCE_STGEntities())
                {
                    int tempSite = pSite.SiteId;
                    String tempSiteDesc = pSite.Description;
                    Site site = dbCtx.Site.AsNoTracking().Where(wr => wr.SiteId == tempSite).SingleOrDefault();
                    if (site != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.Conflict, "Site Id: " + tempSite.ToString() + " already exist.");
                    }
                    Site objSite = new Site();
                    objSite.SiteId = tempSite;
                    objSite.Description = tempSiteDesc.Trim();
                    objSite.Status = "A";
                    dbCtx.Site.Add(objSite);
                    dbCtx.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created, "Site " + objSite.Description.Trim() + " is successfully created.");
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

        /*sample json
         {
           "Id":6,
           "Description":"sitexx",
           "Status":"A"
         }
        */
        [HttpPut]
        public HttpResponseMessage updateSite(dynamic pSite)
        {
            try
            {
                using (var dbCtx = new PRINCE_STGEntities())
                {

                    int tempSite = pSite.SiteId;
                    String tempSiteDesc = pSite.Description;
                    Site site = dbCtx.Site.AsNoTracking().Where(wr => wr.SiteId == tempSite).SingleOrDefault();
                    site.Description = tempSiteDesc;
                    site.Status = pSite.Status;
                    dbCtx.Entry(site).State = System.Data.Entity.EntityState.Modified;
                    dbCtx.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "Site " + site.Description.Trim() + " is successfully Updated.");
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

        public class StoreDc : TEMP_STORE {
            public string DC_STORE_CODE { get; set; }
            public string DC_STORE_DESC { get; set; }
        }
    }
}
