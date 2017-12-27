using QuizMaker.DAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.TeamModel
{
    public class TeamModel
    {
        public long id { get; set; }
        public string Name { get; set; }
        public List<TeamMember> TeamMeambers { get; set; }
        public List<Quiz> Quizzes { get; set; }
        public TeamModel()
        {
            Quizzes = new List<Quiz>();
            TeamMeambers = new List<TeamMember>();

        }
    }
}
