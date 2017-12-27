using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Repository
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(QuizMakerEntities context) : base (context as QuizMakerEntities)
        {

        }

    }
}
