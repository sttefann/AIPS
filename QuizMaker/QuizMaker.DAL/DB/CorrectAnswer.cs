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
    
    public partial class CorrectAnswer
    {
        public Nullable<long> PossibleAnswer { get; set; }
        public long Question { get; set; }
        public long Id { get; set; }
        public string Answer { get; set; }
    
        public virtual PossibleAnswer PossibleAnswer1 { get; set; }
        public virtual Question Question1 { get; set; }
    }
}
