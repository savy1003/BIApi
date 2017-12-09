using biApi.ErrorHelper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace biApi.Controllers
{
    public class MenuController : ApiController
    {
        public List<Menu> getMenu(int id)
        {
            try
            {
                using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
                {
                    int iId = id;
                    int?[] arMenu = (from a in enBi.UserMenu.AsNoTracking() where a.UserId == iId && a.Status == "A" select a.MenuId).ToArray();
                    var menu = (from a in enBi.Menu.AsNoTracking() where a.Status == "A" && arMenu.Contains(a.Id) select a).OrderBy(c => c.LevelNo).ThenBy(d => d.SortNo).ToList();
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

        public dynamic getAllMenu()
        {
            try
            {
                using (PRINCE_STGEntities enBi = new PRINCE_STGEntities())
                {
                    int?[] arMenu = (from a in enBi.UserMenu.AsNoTracking() where a.Status == "A"  select a.MenuId).ToArray();
                    var menu = (from a in enBi.Menu.AsNoTracking()
                                where a.Status == "A" && arMenu.Contains(a.Id) && a.ParentId != 0
                                select new {
                                    a.Code,
                                    a.Name,
                                    canAdd = false,
                                    canEdit = false,
                                    canDelete = false,
                                    canView = false,
                                    a.LevelNo,
                                    a.SortNo
                                }
                                ).OrderBy(c => c.LevelNo).ThenBy(d => d.SortNo).ToList();
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

    }
}
