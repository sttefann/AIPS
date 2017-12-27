using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Repository
{
    public class StatisticalListRepository : Repository<StatisticalList>, IStatisticalListRepository
    {
        public StatisticalListRepository(QuizMakerEntities context) : base(context as QuizMakerEntities)
        {

        }
    }
}
