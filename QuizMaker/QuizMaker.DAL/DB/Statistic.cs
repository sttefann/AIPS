//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuizMaker.DAL.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Statistic
    {
        public long Id { get; set; }
        public long Question_Id { get; set; }
        public Nullable<long> Answer { get; set; }
        public long Quiz_Id { get; set; }
        public long User_Id { get; set; }
        public string Answer_text { get; set; }
        public Nullable<int> Points { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual PossibleAnswer PossibleAnswer { get; set; }
        public virtual Question Question { get; set; }
        public virtual Quiz Quizze { get; set; }
    }
}