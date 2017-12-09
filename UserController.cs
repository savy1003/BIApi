using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using biApi.Models;
using System.Data.Entity.Validation;
using biApi.ErrorHelper;
using System.Data.Entity;

namespace biApi.Controllers
{
    public class UserController : ApiController
    {
        public List<User> getUser(){
            try {
                using (PRINCE_STGEntities entPrince = new PRINCE_STGEntities())
                {
                    List<User> user = (from u in entPrince.User select u).ToList();
                    return user;
                }
            }
            catch (DbEntityValidationException ex)
            {
                ExceptionEntity exDesc = new ExceptionEntity(ex);
                throw new ApiException() {  HttpStatus = HttpStatusCode.BadRequest, ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = exDesc.GetDescException() };
            }
            catch (Exception ex)
            {
                ExceptionDescription exDesc = new ExceptionDescription(ex);
                throw new ApiException() { HttpStatus = HttpStatusCode.BadRequest, ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = exDesc.GetDescException() };
            }
        }

        public TransList getUser(int pg, int tk, string fnd) {
            try
            {
                int skip = tk * (pg - 1); // skip the record
                TransList transList = new TransList();

                using (PRINCE_STGEntities entPrince = new PRINCE_STGEntities())
                {
                  var user = (from u in entPrince.User select new { u.Id, u.Code, u.EmailAdd, u.FirstName, u.LastName, u.Status })
                       .OrderBy(od => od.LastName)
                        .Skip(skip)
                        .Take(tk)
                        .ToList();

                    transList.totalCount = entPrince.User.AsNoTracking().Count();
                    transList.data = user;

                    return transList;
                }
            }
            catch (Exception ex)
            {
                ExceptionDescription exDesc = new ExceptionDescription(ex);
                throw new ApiException() { HttpStatus = HttpStatusCode.BadRequest, ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = exDesc.GetDescException() };
            }
        }

        public TransList getUserActive()
        {
            try
            {
                TransList transList = new TransList();

                using (PRINCE_STGEntities entPrince = new PRINCE_STGEntities())
                {
                    var user = (from u in entPrince.User where u.Status == "A"
                                select new {
                                    u.Id, 
                                    u.Code, 
                                    u.EmailAdd, 
                                    u.FirstName, 
                                    u.LastName, 
                                    u.Status })
                         .OrderBy(od => od.LastName)
                         .ToList();

                    transList.totalCount = entPrince.User.AsNoTracking().Count();
                    transList.data = user;

                    return transList;
                }
            }
            catch (Exception ex)
            {
                ExceptionDescription exDesc = new ExceptionDescription(ex);
                throw new ApiException() { HttpStatus = HttpStatusCode.BadRequest, ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = exDesc.GetDescException() };
            }
        }

        [HttpPost]
        public HttpResponseMessage login(User pUser) {
            try {
                String emailAdd = pUser.EmailAdd;
                String password = pUser.Password;
                using (PRINCE_STGEntities entPrince = new PRINCE_STGEntities())
                {
                    User user = (from u in entPrince.User where u.EmailAdd == emailAdd  && u.Status == "A" select u).SingleOrDefault();
                    if (user.Password == password) {
                        TempUser tempUser = new TempUser();
                        tempUser.Id = user.Id;
                        tempUser.Code = user.Code;
                        tempUser.EmailAdd = user.EmailAdd;
                        tempUser.FirstName = user.FirstName;
                        tempUser.LastName = user.LastName;
                        tempUser.Status = user.Status;
                        return Request.CreateResponse(HttpStatusCode.Accepted, tempUser);
                    }
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
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

        public UserMenu getUserMenu( int pUserCode, int pMenuId) {
            try {
                using (PRINCE_STGEntities entPrince = new PRINCE_STGEntities())
                {
                    UserMenu menu = (from m in entPrince.UserMenu where m.UserId == pUserCode && m.MenuId == pMenuId select m).SingleOrDefault();
                    return menu;
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

        public HttpResponseMessage getUserMenuList(int pUserCode)
        {
            try
            {
                using (PRINCE_STGEntities entPrince = new PRINCE_STGEntities())
                {
                    // List<UserMenu> menu = (from m in entPrince.UserMenu where m.UserId == pUserCode select m).ToList();
                    var menuList = from um in entPrince.UserMenu join m in entPrince.Menu on um.MenuId equals m.Id where um.UserId == pUserCode
                               select new {
                                   m.Code,
                                   m.Name,
                                   m.ParentId,
                                   m.LevelNo,
                                   m.SortNo,
                                   um.CanAdd,
                                   um.CanDelete,
                                   um.CanEdit,
                                   um.CanView,
                                   um.UserId
                               };
                    List<TempUserMenu> tempUserMenu = new List<TempUserMenu>();
                    foreach (var m in menuList) {
                        TempUserMenu tu = new TempUserMenu();
                        tu.Code = m.Code;
                        tu.Name = m.Name;
                        tu.ParentId = m.ParentId;
                        tu.LevelNo = m.LevelNo;
                        tu.SortNo = m.SortNo;
                        tu.canAdd = m.CanAdd;
                        tu.canDelete = m.CanDelete;
                        tu.canEdit = m.CanEdit;
                        tu.canView = m.CanView;
                        tu.UserId = m.UserId;
                        tempUserMenu.Add(tu);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, tempUserMenu);
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

        public HttpResponseMessage saveUserMenu(List<TempUserMenu> pUser) {

            try
            {
                if (pUser.Count > 0)
                {
                    int? userId = pUser[0].UserId;
                    using (PRINCE_STGEntities entPrince = new PRINCE_STGEntities())
                    {
                        List<UserMenu> listUserMenu = (from um in entPrince.UserMenu where um.UserId == userId select um).ToList();
                        foreach (UserMenu uMenu in listUserMenu)
                        {
                            foreach (TempUserMenu tUmenu in pUser)
                            {
                                if (uMenu.Code == tUmenu.Code)
                                {
                                    uMenu.CanAdd = tUmenu.canAdd;
                                    uMenu.CanEdit = tUmenu.canEdit;
                                    uMenu.CanDelete = tUmenu.canDelete;
                                    uMenu.CanView = tUmenu.canView;
                                    break;
                                }
                            }
                            entPrince.Entry(uMenu).State = EntityState.Modified;
                        }
               
                        entPrince.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "User Access Successfully Updated!");
                    }
                }
                return Request.CreateResponse(HttpStatusCode.NotModified);
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
        public HttpResponseMessage saveUser(TempUser pUser) {
            using (PRINCE_STGEntities entPrince = new PRINCE_STGEntities())
            {
                int lastIdNo = (from u in entPrince.User.OrderByDescending(p => p.Id) select u.Id).FirstOrDefault();
                User user = new User();
                user.Code = lastIdNo + 1;
                user.FirstName = pUser.FirstName;
                user.LastName = pUser.LastName;
                user.EmailAdd = pUser.EmailAdd;
                user.Status = pUser.Status;
                user.Password = "test";
                user.STORE_CODE = pUser.STORE_CODE;
                entPrince.User.Add(user);
                entPrince.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK,"User successfully save!");
            } 
        }

        [HttpPut]
        public HttpResponseMessage updateUser(TempUser pUser)
        {
            using (PRINCE_STGEntities entPrince = new PRINCE_STGEntities())
            {
                User user = entPrince.User.Where(wr => wr.Code == pUser.Code).SingleOrDefault();
                user.FirstName = pUser.FirstName;
                user.LastName = pUser.LastName;
                user.EmailAdd = pUser.EmailAdd;
                user.Status = pUser.Status;

                entPrince.Entry(user).State = EntityState.Modified;
                entPrince.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "User successfully updated!");
            }
        }



        public class TempUser {
            public int Id { get; set; }
            public int Code { get; set; }
            public string EmailAdd { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Status { get; set; }
            public string STORE_CODE { get; set; }
        }

        public class TempUserMenu {
            public int? UserId { get; set; }
            public int Code { get; set; }
            public string Name { get; set; }
            public Nullable<int> ParentId { get; set; }
            public Nullable<int> LevelNo { get; set; }
            public Nullable<int> SortNo { get; set; }
            public Nullable<int> canAdd { get; set; }
            public Nullable<int> canEdit { get; set; }
            public Nullable<int> canDelete { get; set; }
            public Nullable<int> canView { get; set; }
        }
    }
}
