﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.Repository;

namespace QuizMaker.Repository
{
    public class TeamMemberRepository : Repository<TeamMember>, ITeamMemberRepository
    {
        public TeamMemberRepository(QuizMakerEntities context) : base ( context as QuizMakerEntities)
        {

        }
    }
}
