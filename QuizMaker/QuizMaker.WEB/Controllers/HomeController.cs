using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using QuizMaker.BL;
using QuizMaker.Interfaces.BL;
using QuizMaker.Models.CategoryModel;
using QuizMaker.Models.QuizModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizMaker.WEB.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICategoryManager _categoryManager;
        readonly IQuizManager _quizManager;
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {

                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public HomeController(ICategoryManager categoryManager, IQuizManager quizManager, ApplicationUserManager userManager)
        {
            _categoryManager = categoryManager;
            _quizManager = quizManager;
            UserManager = userManager;
        }
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Route("Home/Category")]
        public ActionResult Category()
        {
            GroupCategory root = _categoryManager.GetAllCategories();
            ViewBag.Tree = root.Show();
            ViewBag.Quiz = "[]";
            return View(root);

        }
        [HttpGet]
        public ActionResult Category(long id)
        {
            GroupCategory root = _categoryManager.GetAllCategories();
            ICategory category = root.GetChild(id);
            List<QuizzCategoryModel> quizzes = category.ShowQuizzes();
            ViewBag.Quiz = JsonConvert.SerializeObject(quizzes);
            ViewBag.Alert = id;
            ViewBag.Category = category.GetName();

            return View(root);
        }
        public ActionResult Dashboard(Nullable<int> success)
        {
            if (success.HasValue)
                ViewBag.success = success.Value;
            else
                ViewBag.success = 0;
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            var user = _userManager.FindByName(username);
            if (user != null)
            {
                List<QuizzCategoryModel> models = _quizManager.GetAll(user.Id);
                if (models != null)
                    return View(models);
                else
                    return View(new List<QuizzCategoryModel>());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }
    }
}