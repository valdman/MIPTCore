using DataAccess.Contexts;
using DonationManagment;

namespace DataAccess.Repositories
{
    public class DonationRepository : GenericRepository<Donation>
    {
        public DonationRepository(DonationContext context) : base(context)
        {
        }
    }
}