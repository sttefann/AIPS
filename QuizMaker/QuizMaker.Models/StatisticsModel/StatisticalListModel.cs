﻿using QuizMaker.DAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.StatisticsModel
{
    public class StatisticalListModel
    {
        public long Id { get; set; }
        public long Quiz_Id { get; set; }
        public AspNetUser User { get; set; }
        public int Points { get; set; }
        public bool Vote { get; set; }
        public StatisticalListModel()
        {
            User = new AspNetUser();
        }
    }
}