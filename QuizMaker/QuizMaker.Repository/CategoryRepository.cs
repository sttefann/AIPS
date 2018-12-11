using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(QuizMakerEntities context) : base(context as QuizMakerEntities)
        {
        }
        public ICollection<Category> GetLeafCategories()
        {
            try
            {
                return Context.Set<Category>().Where(x => x.Partible == 0).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
