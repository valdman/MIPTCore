using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CapitalManagment
{
    public interface ICapitalManager
    {
        Capital GetCapitalById(int capitalId);
        Capital GetCapitalByFullUri(string capitalName);
        IEnumerable<Capital> GetAllCapitals();
        IEnumerable<Capital> GetCapitalsByPredicate(Expression<Func<Capital, bool>> predicate);

        Volume GetFundVolumeCapital();
        int CreateCapital(Capital capitalToCreate);
        void UpdateCapital(Capital capitalToUpdate);
        void DeleteCapital(int capitalToDeleteId);
    }
}