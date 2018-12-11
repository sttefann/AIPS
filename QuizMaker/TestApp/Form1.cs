using QuizMaker.BL.Managers;
using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.BL;
using QuizMaker.Interfaces.Repository;
using QuizMaker.Models.CategoryModel;
using QuizMaker.Models.QuizModels;
using QuizMaker.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestApp
{
    public partial class Form1 : Form
    {
        private readonly ICategoryManager _categoryManager;
        private readonly IQuizManager _quizManager;
        private readonly IUow _uow;
        private readonly QuizMakerEntities _context;

        public Form1()
        {
            InitializeComponent();
            _context = new QuizMakerEntities();
            _uow = new UoW(_context);
            _categoryManager = new CategoryManager(_uow);
         //   _quizManager = new QuizManager(_uow);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<CategoryModel> models = _categoryManager.GetMainCategories();
            string txt = "";
            foreach(CategoryModel category in models)
            {
                txt += category.Name + Environment.NewLine;
            }
            MessageBox.Show(txt);
        }

        private void button2_Click(object sender, EventArgs e)
        {
         /*   
            List<CategoryModel> models = _categoryManager.GetSubCategories(1);
            string txt = "";
            foreach (CategoryModel category in models)
            {
                txt += category.Name + Environment.NewLine;
            }
            MessageBox.Show(txt);*/
        }

        private void button9_Click(object sender, EventArgs e)
        {
            CategoryModel model = new CategoryModel();
            model.Name = "Music";
            model.Parent_id = 0;
            model.Partible = false;
            long id = _categoryManager.NewCategory(model);
            Category category = _categoryManager.GetCategory(id);
            MessageBox.Show(
                category.id + Environment.NewLine +
                 category.Name + Environment.NewLine +
            category.Parent_id + Environment.NewLine +
            category.Partible
                );
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //QuizModel model = new QuizModel();
            //model.Category = _categoryManager.GetCategory(10003);
            //model.Owner_Id = 1;
            //model.Name = "Test";
            //model.Team = null;
            //model.Questions = null;
            //model.StatisticalList = null;
            //model.SurveyStatisticsList = null;
            //_quizManager.Add(model);


            //model = new QuizModel();
            //model.Category = _categoryManager.GetCategory(10003);
            //model.Owner_Id = 1;
            //model.Name = "Example";
            //model.Team = null;
            //model.Questions = null;
            //model.StatisticalList = null;
            //model.SurveyStatisticsList = null;
            //_quizManager.Add(model);

            //model = new QuizModel();
            //model.Category = _categoryManager.GetCategory(10003);
            //model.Owner_Id = 1;
            //model.Name = "Something completly new";
            //model.Team = null;
            //model.Questions = null;
            //model.StatisticalList = null;
            //model.SurveyStatisticsList = null;
            //_quizManager.Add(model);

            //model = new QuizModel();
            //model.Category = _categoryManager.GetCategory(10003);
            //model.Owner_Id = 1;
            //model.Name = "Why so serious?";
            //model.Team = null;
            //model.Questions = null;
            //model.StatisticalList = null;
            //model.SurveyStatisticsList = null;
            //_quizManager.Add(model);
        }
    }
}
