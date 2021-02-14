using FarmatikoData.Models;
using System.Threading.Tasks;

namespace FarmatikoData.FarmatikoRepoInterfaces
{
    public interface IUpdateDataRepo
    {
        Task AddFacility(HealthFacility healthFacility);
        Task AddMedicines(Medicine medicine);
        Task AddPharmacy(Pharmacy pharmacy);
        Task AddWorker(HealthcareWorker Worker);
        HealthFacility GetFacilityJSON(string healthFacility);
    }
}