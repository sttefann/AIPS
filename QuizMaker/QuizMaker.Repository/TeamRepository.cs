using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.Repository;

namespace QuizMaker.Repository
{
    public class TeamRepository : Repository<Team>, ITeamRepository
    {
        public TeamRepository(QuizMakerEntities context) : base(context as QuizMakerEntities)
        {

        }
    }
}
