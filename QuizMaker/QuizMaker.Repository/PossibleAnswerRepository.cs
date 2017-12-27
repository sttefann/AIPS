using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Repository
{
    public class PossibleAnswerRepository : Repository<PossibleAnswer>, IPossibleAnswerRepository
    {
        public PossibleAnswerRepository(QuizMakerEntities context) : base(context as QuizMakerEntities)
        {
        }
    }
}
