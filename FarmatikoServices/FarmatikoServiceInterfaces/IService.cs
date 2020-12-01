using FarmatikoData.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmatikoServices.FarmatikoServiceInterfaces
{
    public interface IService
    {
        //GET
        Task<IEnumerable<HealthcareWorker>> GetAllWorkers();
        Task<IEnumerable<HealthcareWorker>> SearchWorkers(string query);
        Task<HealthcareWorker> GetWorker(int id);
        Task<IEnumerable<HealthFacility>> GetFacilities();
        Task<IEnumerable<HealthFacility>> SearchFacilities(string query);
        Task<HealthFacility> GetFacility(int id);
        HealthFacility GetFacilityJSON(string healthFacility);
        Task<Medicine> GetMedicine(int id);
        Task<IEnumerable<Medicine>> SearchMedicines(string query);
        Task<IEnumerable<Medicine>> GetMedicines();
        Task<Pandemic> GetPandemic();
        Task<IEnumerable<Pharmacy>> GetPharmacies();
        Task<IEnumerable<Pharmacy>> SearchPharmacies(string query);
        Task<Pharmacy> GetPharmacy(int id);
        //POST
        Task AddWorker(HealthcareWorker worker);
        Task AddFacility(HealthFacility healthFacilities);
        Task AddPharmacy(Pharmacy pharmacy);
        Task AddPharmacyHead(PharmacyHead pharmacyHead);
        Task AddMedicines(Medicine medicine);
        Task AddPandemic(Pandemic pandemic);
        Task UpdateFacility(HealthFacility healthFacilities);
        Task RemoveMedicine(Medicine medicine);
        Task RemovePharmacyHead(int Id);
        Task UpdatePandemic(Pandemic pandemic);
        Task RemovePharmacy(Pharmacy pharmacy);
        Task UpdateWorker(HealthcareWorker worker);
        Task UpdatePharmacy(Pharmacy pharmacy);
    }
}
