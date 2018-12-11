using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.BL;
using QuizMaker.Interfaces.Repository;

using System.Diagnostics;
using QuizMaker.Models.Item;

namespace QuizMaker.BL.Managers
{
    public class TeamManager : ITeamManager
    {
        private readonly IUow _uow;
        public TeamManager(IUow uow)
        {
            _uow = uow;
        }

        public bool AddTeamMember(long quizId, long userId, string username)
        {
            try
            {
                TeamMember member = new TeamMember
                {
                    Quiz_Id = quizId,
                    User_Id = userId,
                    Username = username
                };
                TeamMember result = _uow.TeamMembers.Add(member);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e.InnerException;
                Debug.Write(exception.Message);
                return false;
            }
        }
        public bool DeleteTeamMember(long quizId, long userId)
        {
            try
            {
                List<TeamMember> members = _uow.TeamMembers.GetAll().Where(x => x.Quiz_Id == quizId).ToList();
                TeamMember result = new TeamMember();
                bool check = false;
                foreach (var member in members)
                {
                    if(member.User_Id == userId)
                    {
                        result = member;
                        check = true;
                    }
                }
                if (check)
                {
                    _uow.TeamMembers.Remove(result);
                    _uow.Complete();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e.InnerException;
                Debug.Write(exception.Message);
                return false;
            }
        }
        public List<long> GetAll(long quizId)
        {
            try
            {
                List<TeamMember> members = _uow.TeamMembers.GetAll().Where(x => x.Quiz_Id == quizId).ToList();
                List<long> users = new List<long>();
                foreach (var mem in members)
                {
                    users.Add(mem.User_Id);
                }
                return users;
            }
            catch(Exception e)
            {
                return null;
            }
        }
        public List<string> GetAllMembers(long quizId)
        {
            try
            {
                List<TeamMember> members = _uow.TeamMembers.GetAll().Where(x => x.Quiz_Id == quizId).ToList();
                List<string> users = new List<string>();
                foreach (var mem in members)
                {
                    users.Add(mem.Username);
                }
                return users;
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}
