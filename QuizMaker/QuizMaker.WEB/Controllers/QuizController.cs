using Microsoft.AspNet.Identity.Owin;
using QuizMaker.Interfaces.BL;
using QuizMaker.Models.QuestionModels;
using QuizMaker.Models.QuizModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using QuizMaker.DAL.DB;
using QuizMaker.Models.Item;
using QuizMaker.Models.StatisticsModel;

namespace QuizMaker.WEB.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        readonly IQuizManager _quizManager;
        readonly IQuestionManager _questionManager;
        readonly IStatisticsManager _statisticsManager;
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
        public QuizController(IQuizManager quizManager, IStatisticsManager statManager, IQuestionManager questionManager, ApplicationUserManager userManager)
        {
            _quizManager = quizManager;
            _questionManager = questionManager;
            _statisticsManager = statManager;
            UserManager = userManager;
        }
        // GET: Quiz/Play/5
        public ActionResult Play(long id)
        {
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            var user = _userManager.FindByName(username);
            if (!_statisticsManager.IsPlayed(id, user.Id))
            {
                QuizPlayModel model = _quizManager.GetQuiz(id);
                if (model != null)
                {
                    var questions = _questionManager.GetQuestions(id);
                    if (questions != null)
                    {
                        model.Questions = questions;
                    }
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "StatusCode", new { statusCode = 1404 });
                }
            }
            return RedirectToAction("Result", "Quiz", new { id = id });
        }
        [HttpPost]
        public ActionResult SaveStats(StatisticsCreateModel answers)
        {
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            var user = _userManager.FindByName(username);
            if (!_statisticsManager.IsPlayed(answers.Quiz_Id, user.Id))
            {
                foreach (var stat in answers.Stats)
                {
                    stat.User_Id = user.Id;
                }
                int result = _statisticsManager.AddStatistics(answers.Stats);
                if(result >= 0)
                {
                    return Json(new { success = true, url = Url.Action("Result", "Quiz", new { id = answers.Quiz_Id }) }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, url = Url.Action("Result", "Quiz", new { id = answers.Quiz_Id }) }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Result(long id)
        {
            string type = _quizManager.GetType(id);
            if(type != "")
            {
                string username = System.Web.HttpContext.Current.User.Identity.Name;
                var user = _userManager.FindByName(username);
                if (_statisticsManager.IsPlayed(id, user.Id))
                {
                    ItemModel model = new ItemModel()
                    {
                        Id = id,
                        Value = type
                    };
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Play", "Quiz", new { id });
                }
            }
            else
            {
                return RedirectToAction("Index", "StatusCode", new { statusCode = 1404 });
            }
           
        }
        //[HttpPost]
        //public ActionResult Result(ItemModel item)
        //{
        //    long id = item.Id;
        //    string username = System.Web.HttpContext.Current.User.Identity.Name;
        //    var user = _userManager.FindByName(username);

        //    string type = item.Value;
        //    if (type == "test")
        //    {
        //        List<QuestionPlayedModel> results = _statisticsManager.GetTestResults(user.Id, id);
        //        return Json(new { success = true, results = results, text = _quizManager.GetQuizName(id) }, JsonRequestBehavior.AllowGet);
        //    }
        //    else if (type == "poll")
        //    {
        //        List<PollStats> results = _statisticsManager.GetPollResults(id, user.Id);
        //        string txt = _quizManager.GetPollText(id);
        //        return Json(new { success = true, results = results, text = txt }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new { success = true, results = "Survey is finished and saved! Thank you for you time.", text = "" }, JsonRequestBehavior.AllowGet);
        //    }
            
        //}
        [HttpPost]
        public ActionResult TestResult(ItemModel item)
        {
            long id = item.Id;
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            var user = _userManager.FindByName(username);
            List<QuestionPlayedModel> results = _statisticsManager.GetTestResults(user.Id, id);
            return Json(new { success = true, results = results, text = _quizManager.GetQuizName(id) }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SurveyResult(ItemModel item)
        {
                return Json(new { success = true, results = "Survey is finished and saved! Thank you for you time.", text = "" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PollResult(ItemModel item)
        {
            long id = item.Id;
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            var user = _userManager.FindByName(username);
            List<PollStats> results = _statisticsManager.GetPollResults(id, user.Id);
            string txt = _quizManager.GetPollText(id);
            return Json(new { success = true, results = results, text = txt }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Stats(long id)
        {
            string type = _quizManager.GetType(id);
            if (IsOwner(id))
            {
                ItemModel model = new ItemModel()
                {
                    Id = id,
                    Value = type
                };
                return View(model);
            }
            return RedirectToAction("Status600", "StatusCode");
        }
        public ActionResult TestStats(long id)
        {
            Stats<TestRankList> stats = _statisticsManager.TestStats(id);
            return Json(new { results = stats }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SurveyStats(long id)
        {
            Stats<SurveyStats> stats = _statisticsManager.SurveyStats(id);
            return Json(new { results = stats }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PollStats(long id)
        {
            Stats<PollStats> stats = _statisticsManager.PollStats(id);
            return Json(new { results = stats }, JsonRequestBehavior.AllowGet);
        }
        // GET: Quiz/Create
        public ActionResult Create()
        {
            return View(_quizManager.GetCreateModel());
        }
        // POST: Quiz/Create
        [HttpPost]
        public ActionResult Create(QuizCreateModel model)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {

                    string username = System.Web.HttpContext.Current.User.Identity.Name;
                    var user = _userManager.FindByName(username);
                    model.Owner_Id = user.Id;
                    long result = _quizManager.Add(model);
                    if (result != -1)
                        if (model.Team)
                            return RedirectToAction("TeamProject", "Group", new { id = result });
                        else
                            return RedirectToAction("Create", "Question", new { id = result, success = 2 });
                    else
                    {
                        ViewBag.success = -1;
                        return View(model);
                    }



                }
                return View(model);
            }
            catch (Exception e)
            {
                var ex = e.InnerException;
                return RedirectToAction("Index", "StatusCode", new { statusCode = 500, exception = e });
            }
        }
        //public ActionResult DeleteUser(TeamMember user)
        //{
        //    ItemModel item = new ItemModel();
        //    item.Id = 2;
        //    item.Value = "filipfilipovic@gmail.com";
        //    ItemModel quiz = new ItemModel();
        //    quiz.Id = 10;
        //    quiz.Value = "Something";



        //    return Json(new { success = true}, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public ActionResult Edit(int id, QuizEditModel model)
        {
            try
            {
                // TODO: Add update logic here
                var result = _quizManager.Update(id,model);
                return Json(new { success = result }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }
        // GET: Quiz/Delete/5
        public ActionResult Delete(int id)
        {
            if (IsOwner(id))
            {
                if (_quizManager.IsTeamProject(id))
                {
                    _quizManager.LeaveTeamProject(id);
                    return RedirectToAction("Dashboard", "Home", new { success = 1 });
                }
                else
                {
                    bool rez = _questionManager.DeleteAll(id);
                    if (rez)
                    {
                        var result = _quizManager.Delete(id);
                        if (result)
                        {
                            return RedirectToAction("Dashboard", "Home", new { success = 1 });
                        }
                    }
                }

            }

            return RedirectToAction("Dashboard", "Home", new { success = -1 });
        }

        public ActionResult DeleteQuestion(long id)
        {
            long quizId = _questionManager.GetQuizId(id);
            var result = _questionManager.Delete(id);
            return Json(new { success = result, qId = id });
        }
        public ActionResult IsOwner(ItemModel item)
        {
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            var user = _userManager.FindByName(username);
            if (user != null)
            {
                bool isowner = _quizManager.IsOwner(item.Id, user.Id);
                return Json(new { IsOwner = isowner }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        private bool IsOwner(long id)
        {
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            var user = _userManager.FindByName(username);
            if (user != null)
            {
                bool isowner = _quizManager.IsOwner(id, user.Id);
                return isowner;
            }
            return false;
        }
    }
}
