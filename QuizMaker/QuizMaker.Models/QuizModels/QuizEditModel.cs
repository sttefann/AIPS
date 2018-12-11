using QuizMaker.Models.Item;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.QuizModels
{
    public class QuizEditModel
    {
        [Required]
        public string Name { get; set; }
        [Display(Name = "Category")]
        [Required]
        public long Category_Id { get; set; }
        public List<ItemModel> Categories { get; set; }
        public QuizEditModel()
        {
            Categories = new List<ItemModel>();
        }
    }
}
