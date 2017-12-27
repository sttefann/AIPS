using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.BL;
using QuizMaker.Interfaces.Repository;
using QuizMaker.Models.TeamModel;

namespace QuizMaker.BL.Managers
{
    public class TeamManager : ITeamManager
    {
        private readonly IUow _uow;

        public TeamManager(IUow uow)
        {
            _uow = uow;
        }
        public bool Add(TeamModel model)
        {
            try
            {

                Team team = new Team();
                team.Name = model.Name;
                team.TeamMembers = model.TeamMeambers;
                team.Quizzes = model.Quizzes;
                _uow.Teams.Add(team);
                _uow.Complete();
                return true;

            }catch(Exception e)
            {
                return false;
            }
        }

        public bool AddQuiz(Quiz quiz,long teamId)
        {
            try
            {
                Team team = _uow.Teams.Get(teamId);
                team.Quizzes.Add(quiz);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool AddTeamMember(TeamMember member, long teamId)
        {
            try
            {
                Team team = _uow.Teams.Get(teamId);
                team.TeamMembers.Add(member);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false; 
            }
        }

        public bool Delete(long id)
        {
            try
            {
                Team team = _uow.Teams.Get(id);
                _uow.Teams.Remove(team);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteTeamMember(long teamId, long memberId)
        {
            try
            {
                Team team = _uow.Teams.Get(teamId);
                TeamMember member = _uow.TeamMembers.Get(memberId);

                team.TeamMembers.Remove(member);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<Quiz> GetQuizzes(long teamId)
        {
            try
            {
                List<Quiz> quizzes = _uow.Teams.Get(teamId).Quizzes.ToList();
                return quizzes;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public TeamModel GetTeam(long id)
        {
            try
            {
                TeamModel model = new TeamModel();
                Team team = _uow.Teams.Get(id);
                model.id = team.id;
                model.Name = team.Name;
                model.Quizzes = team.Quizzes.ToList();
                model.TeamMeambers = team.TeamMembers.ToList();
                return model;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool Update(TeamModel team)
        {
            try
            {
                Team model = _uow.Teams.Get(team.id);

                model.id = team.id;
                model.Name = team.Name;
                model.Quizzes = team.Quizzes.ToList();
                model.TeamMembers = team.TeamMeambers;

                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
