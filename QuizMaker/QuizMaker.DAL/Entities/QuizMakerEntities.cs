using System.Data.Entity;
using Microsoft.Extensions.Configuration;

namespace QuizMaker.DAL.DB
{
    public partial class QuizMakerEntities : IQuizMakerEntities
    {
        public QuizMakerEntities(IConfiguration config) : base(config["ConnStringSection:Default"])
        {

        }
        private void FixEfProviderServicesProblem()
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    }
}
