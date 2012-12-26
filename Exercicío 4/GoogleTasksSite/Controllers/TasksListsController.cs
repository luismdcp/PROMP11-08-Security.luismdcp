using System.Web.Mvc;
using GoogleTasksSite.CustomAttributes;
using GoogleTasksSite.Models;
using Microsoft.IdentityModel.Claims;

namespace GoogleTasksSite.Controllers
{
    public class TasksListsController : Controller
    {
        private readonly GoogleTasksRepository _repository = new GoogleTasksRepository();

        // GET: /TaskLists/
        [RequireTokenAuthentication]
        [RequireClaims(ClaimTypes.NameIdentifier)]
        public ActionResult Index(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Redirect(_repository.GetReturnCodeURL());
            }
            else
            {
                var token = _repository.GetToken(code);
                Session["Token"] = token;
                return View(_repository.GetTaskLists(token));
            }
        }

        // GET: /TaskLists/MTQyNjQzMDMwMDQwODAyMzc2NjU6MDow/Tasks
        [RequireTokenAuthentication]
        [RequireClaims(ClaimTypes.NameIdentifier)]
        public ActionResult Tasks(string id)
        {
            var token = (Token) Session["Token"];
            return View(_repository.GetTasks(id, token));
        }
    }
}