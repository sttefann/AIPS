
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.Item
{
    public class ResultsItem<T>
    {
        public bool Success { get; set; }
        public List<T> Results { get; set; }
        public string Text { get; set; }

    }
}
