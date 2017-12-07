using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapitalManagment;
using Common.Infrastructure;

namespace CapitalsTableHelper
{
    public class CapitalsTableHelper : ICapitalsTableHelper
    {
        private readonly ICapitalsTableEntryRepository _captalsTableRepository;
        private readonly ICapitalManager _capitalsManager;

        public CapitalsTableHelper(ICapitalsTableEntryRepository captalsTableRepository, ICapitalManager capitalsManager)
        {
            _captalsTableRepository = captalsTableRepository;
            _capitalsManager = capitalsManager;
        }

        public IEnumerable<CapitalsTableEntry> GetTableForCapitals()
        {
            return _captalsTableRepository.GetAll();
        }

        public void SaveTable(IEnumerable<CapitalsTableEntry> table)
        {
            _captalsTableRepository.DeleteAllCapitalTableEntries();
            
            foreach (var capitalsTableEntry in table)
            {
                var relatedCapital = _capitalsManager.GetCapitalById(capitalsTableEntry.CapitalId);
                if (relatedCapital == null)
                {
                    throw new RelatedCapitalNotExists();
                }
                _captalsTableRepository.Create(capitalsTableEntry);
            }
        }
    }
}