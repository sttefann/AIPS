using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.Enums
{
    public enum QuizType
    {
        Test = 1,
        Survey = 2,
        Poll = 3,
        Error = 4
    }
    public static class QuizTypeExtensions
    {
        public static QuizType ToQuizType(this int type)
        {
            switch (type)
            {
                case 1:
                    return QuizType.Test;
                case 2:
                    return QuizType.Survey;
                case 3:
                   return QuizType.Poll;
                default:
                    return QuizType.Error;
            }
        }
        public static string ConvertToString(this QuizType type)
        {
            switch (type)
            {
                case QuizType.Test:
                    return "test";
                case QuizType.Survey:
                    return "survey";
                case QuizType.Poll:
                    return "poll";
                default:
                    return "Error!";
            }
        }
    }
}
