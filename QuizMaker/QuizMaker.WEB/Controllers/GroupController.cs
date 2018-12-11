using Microsoft.AspNet.Identity;
using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.BL;
using QuizMaker.Models.Item;
using QuizMaker.Models.QuestionModels;
using QuizMaker.Models.QuizModels;
using QuizMaker.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizMaker.WEB.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        private ApplicationUserManager _userManager;
        readonly IQuestionManager _questionManager;
        readonly ITeamManager _teamManager;
        readonly IQuizManager _quizManager;

        public GroupController(ApplicationUserManager userManager, IQuestionManager questionManager, IQuizManager quizManager, ITeamManager teamManager)
        {
            _userManager = userManager;
            _questionManager = questionManager;
            _quizManager = quizManager;
            _teamManager = teamManager;
        }
        public ActionResult Index()
        {
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            var user = _userManager.FindByName(username);
            List<QuizzCategoryModel> quizzes = _quizManager.GetAllTeamProjects(user.Id);
            return View(quizzes);
        }
        public ActionResult TeamProject(long id)
        {
            QuizModel quiz = _quizManager.Get(id);
            if (quiz != null)
            {
                if (!IsAuthorized(quiz.Id))
                {
                    if (!IsOwner(quiz.Id))
                        return RedirectToAction("Index", "StatusCode", new { statusCode = 1404, exception = new Exception() });
                }
                QuizTeamModel model = GetTeamProjectModel(id);
                model.ReadOnly = quiz.ReadOnly;
                if (model.Type == "poll" && model.Questions.Count() == 1)
                    ViewBag.finished = true;
                else
                    ViewBag.finished = false;
                ViewBag.quizname = quiz.Name;
                //if(isReadOnly)
                //  ViewBag.QuizId = id;

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "StatusCode", new { statusCode = 1404, exception = new Exception() });
            }
        }
        public ActionResult AddUser(TeamMember user)
        {
            
            ItemModel item = new ItemModel();
            var u = _userManager.FindById(user.User_Id);
            if (u == null)
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            item.Id = u.Id;
            item.Value = u.UserName;
            ItemModel quiz = new ItemModel();
            var q = _quizManager.Get(user.Quiz_Id);
            if (q == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            quiz.Id = q.Id;
            if (!IsOwner(q.Id))
                return RedirectToAction("Index", "StatusCode", new { statusCode = 1404, exception = new Exception() });
            quiz.Value = q.Name;
            var result = _teamManager.AddTeamMember(quiz.Id, u.Id, u.Email);
            if(result)
                return Json(new { success = true, user = item, quiz = quiz }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, user = item, quiz = quiz }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveUser(ItemModel item)
        {
            var quizid = item.Id;
            if (!IsOwner(quizid))
                return RedirectToAction("Index", "StatusCode", new { statusCode = 1404, exception = new Exception() });
            var username = item.Value;
            var user = _userManager.FindByName(username);
            var result = _teamManager.DeleteTeamMember(quizid, user.Id);
            return Json(new { success = result }, JsonRequestBehavior.AllowGet);
        }
        // GET: Question/Delete/5
        /// <summary>
        /// Delete question with id
        /// </summary>
        /// <param name="id">Question's id</param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {

            long quizId = _questionManager.GetQuizId(id);
            if (!IsOwner(quizId))
                return RedirectToAction("Index", "StatusCode", new { statusCode = 1404, exception = new Exception() });
            bool result = _questionManager.Delete(id);
            return Json(new { success = result }, JsonRequestBehavior.AllowGet);
        }
        // POST: Group/ReadOnly
        [HttpPost]
        public ActionResult ReadOnly(ItemReadOnly item)
        {
            bool result = _quizManager.MakeReadOnly(item.Id,item.Value);
            return Json(new { success = result, val = item.Value }, JsonRequestBehavior.AllowGet);
        }
        #region PrivateMethodes
        private List<ItemModel> GetUsers()
        {
            try
            {
                List<ItemModel> result = new List<ItemModel>();
                List<ApplicationUser> users =  _userManager.Users.ToList();
                foreach (ApplicationUser user in users)
                {
                    if (user.UserName != System.Web.HttpContext.Current.User.Identity.Name)
                    {
                        ItemModel item = new ItemModel();
                        item.Id = user.Id;
                        item.Value = user.UserName;
                        result.Add(item);
                    }
                    
                }
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private bool IsOwner(long quizId)
        {
            try
            {
                string username = System.Web.HttpContext.Current.User.Identity.Name;
                var user = _userManager.FindByName(username);
                QuizModel quiz = _quizManager.Get(quizId);
                if (quiz.Owner_Id == user.Id)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        private bool IsAuthorized(long quizId)
        {
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            List<long> members = _teamManager.GetAll(quizId);
            var user = _userManager.FindByName(username);
            if (user == null) return false;
            bool authorized = false;
            foreach (var mem in members)
            {
                if (mem == user.Id)
                    authorized = true;
            }
            return authorized;
        }
        private QuizTeamModel GetTeamProjectModel(long id)
        {
            QuizTeamModel model = new QuizTeamModel();
            List<QuestionViewModel> questionList = new List<QuestionViewModel>();
            questionList = _questionManager.GetQuestionsForQuiz(id);
            model.Quiz = _quizManager.GetEditModel(id);
            model.Questions = questionList;
            model.Type = _quizManager.GetType(id);
            model.Id = id;
            model.Users = GetUsers();
            model.IsOwner = IsOwner(id);
            model.Members = _teamManager.GetAllMembers(id);
            model.Name = model.Quiz.Name;

            return model;
        }
        #endregion
    }
}