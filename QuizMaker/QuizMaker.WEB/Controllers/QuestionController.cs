using QuizMaker.BL.Managers;
using QuizMaker.Interfaces.BL;
using QuizMaker.Models.QuestionModels;
using QuizMaker.Models.QuizModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using QuizMaker.Models.Item;

namespace QuizMaker.WEB.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        readonly IQuestionManager _questionManager;
        readonly IQuizManager _quizManager;
        readonly ApplicationUserManager _userManager;
        public QuestionController(IQuestionManager questionManager, IQuizManager quizManager, ApplicationUserManager userManager)
        {
            _questionManager = questionManager;
            _quizManager = quizManager;
            _userManager = userManager;
        }
        // GET: Question/Create
        public ActionResult Create(long id,Nullable<int> success)
        {
            QQModel model = new QQModel();
            QuizModel quiz = _quizManager.Get(id);
            List<QuestionViewModel> questionList = new List<QuestionViewModel>();
            if (quiz != null)
            {
                questionList = _questionManager.GetQuestionsForQuiz(id);
                model.Questions = questionList;
                model.Quiz = _quizManager.GetEditModel(id);
                ViewBag.type = _quizManager.GetType(id);
                if (success.HasValue)
                    ViewBag.success = success.Value;
                else
                    ViewBag.success = 0;
               // ViewBag.num = questionList.Count();
                ViewBag.quizname = quiz.Name;
                if (ViewBag.type == "poll" && questionList.Count() == 1)
                    ViewBag.finished = true;
                else
                    ViewBag.finished = false;

                ViewBag.QuizId = id;

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "StatusCode", new { statusCode = 1404, exception = new Exception() });
            }
            
        }
        // GET: Question/Create
        [HttpPost]
        public ActionResult Create(QuestionCreateModel question)
        {
            if (question != null)
            {
                long result = _questionManager.Add(question);
                if (result != -1)
                {
                    QuestionViewModel q = _questionManager.GetViewModel(result);
                    q.Id = result;
                    return Json(new { success = true, model = q, responseUrl = Url.Action("Create", "Question", new { id = question.Quiz_Id, success = 1 }) });
                }
                else
                {
                    return Json(new { success = false, responseUrl = Url.Action("Create", "Question", new { id = question.Quiz_Id, success = -1 }) });
                }
            }
            else
                return Json(new { success = false, responseUrl = Url.Action("Create", "Question", new { id = question.Quiz_Id, success = -1 }) });


        }
        // GET: Question/Delete/5
        public ActionResult Delete(int id)
        {
            long quizId = _questionManager.GetQuizId(id);
            var result = _questionManager.Delete(id);
            if (result)
            {
                return RedirectToAction("Create", "Question", new { id = quizId, success = 3 });
            } 
            else
                return RedirectToAction("Create", "Question", new { id = quizId, success = -3 });
        }
        // GET: Question/Edit/5
        public ActionResult Edit(long id)
        {
            QuestionViewModel model = _questionManager.GetViewModel(id);
            return Json(new { model }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Edit(QuestionEditModel model)
        {
            bool result = _questionManager.Update(model);
            QuestionViewModel qmodel = new QuestionViewModel();
            if (result)
            {
                qmodel = _questionManager.GetViewModel(model.Id);
            }
            return Json(new { success = result, qmodel }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReadOnly(ItemReadOnly item)
        {
            bool result = _questionManager.ReadOnly(item.Id, item.Value);

            return Json(new { result, Id = item.Id }, JsonRequestBehavior.AllowGet);
        }

    }
}
