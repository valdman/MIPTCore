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

        public Task<IEnumerable<CapitalsTableEntry>> GetTableForCapitals()
        {
            return _captalsTableRepository.GetAll();
        }

        public async Task SaveTable(IEnumerable<CapitalsTableEntry> table)
        {
            await _captalsTableRepository.DeleteAllCapitalTableEntriesAsync();
            
            foreach (var capitalsTableEntry in table)
            {
                var relatedCapital = await _capitalsManager.GetCapitalByIdAsync(capitalsTableEntry.CapitalId);
                if (relatedCapital == null)
                {
                    throw new RelatedCapitalNotExists();
                }
                await _captalsTableRepository.CreateAsync(capitalsTableEntry);
            }
        }
    }
}