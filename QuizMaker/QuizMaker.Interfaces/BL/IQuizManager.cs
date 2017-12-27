using QuizMaker.Models.QuizModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Interfaces.BL
{
    public interface IQuizManager
    {
        List<QuizModel> GetAll();
        QuizModel Get(long id);
        bool Update(QuizModel model);
        bool Delete(long id);
        bool Add(QuizModel model);
    }
}
