using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapitalManagment;
using Common.Infrastructure;

namespace CapitalsTableHelper
{
    public class CapitalsTableHelper : ICapitalsTableHelper
    {
        private readonly IGenericRepository<CapitalsTableEntry> _captalsTableRepository;
        private readonly ICapitalManager _capitalsManager;

        public CapitalsTableHelper(IGenericRepository<CapitalsTableEntry> captalsTableRepository, ICapitalManager capitalsManager)
        {
            _captalsTableRepository = captalsTableRepository;
            _capitalsManager = capitalsManager;
        }

        public Task<IEnumerable<CapitalsTableEntry>> GetTableForCapitals()
        {
            return _captalsTableRepository.GetAll();
        }

        public Task<CapitalsTableEntry> GetEntryForCapital(int capitalId)
        {
            return _captalsTableRepository.GetByIdAsync(capitalId);
        }

        public async Task UpdateEntryForCapital(CapitalsTableEntry capitalsTableEntry)
        {
            var relatedCapital = await _capitalsManager.GetCapitalByIdAsync(capitalsTableEntry.CapitalId);

            if (relatedCapital == null)
            {
                throw new RelatedCapitalNotExists();
            }

            await SaveCaitalsEntry(capitalsTableEntry);
        }

        public async Task SaveTable(IEnumerable<CapitalsTableEntry> table)
        {
            foreach (var capitalsTableEntry in table)
            {
                await UpdateEntryForCapital(capitalsTableEntry);
            }
        }

        private async Task<int> SaveCaitalsEntry(CapitalsTableEntry capitalsTableEntry)
        {
            await _captalsTableRepository.DeleteIfExistsAsync(capitalsTableEntry.CapitalId);
            return await _captalsTableRepository.CreateAsync(capitalsTableEntry);
        }
    }
}