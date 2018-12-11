using QuizMaker.DAL.DB;
using QuizMaker.Models.Enums;
using QuizMaker.Models.Item;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.QuizModels
{
    public class QuizCreateModel
    {
        public long Owner_Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public QuizType Type { get; set; }
        public List<ItemTypeModel> Types { get; set; }
        [Display(Name = "Is this quiz team project?")]
        public bool Team { get; set; }
        [Display(Name = "Category")]
        [Required]
        public long Category_Id { get; set; }
        public List<ItemModel> Categories { get; set; }
        public QuizCreateModel()
        {
            Categories = new List<ItemModel>();
            Types = new List<ItemTypeModel>();
        }
    }
}
