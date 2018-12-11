using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuizMaker.DAL.DB
{
    public interface IQuizMakerEntities
    {
        DbSet<AspNetRole> AspNetRoles { get; set; }
        DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        DbSet<AspNetUser> AspNetUsers { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<PossibleAnswer> PossibleAnswers { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<Quiz> Quizs { get; set; }
        DbSet<Statistic> Statistics { get; set; }
        DbSet<TeamMember> TeamMembers { get; set; }
    }
}
