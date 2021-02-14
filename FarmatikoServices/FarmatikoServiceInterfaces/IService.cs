using FarmatikoData.DTOs;
using FarmatikoData.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FarmatikoServices.FarmatikoServiceInterfaces
{
    public interface IService
    {
        Task AddFacility(HealthFacility healthFacility);
        Task AddMedicines(Medicine medicine);
        Task AddPandemic(Pandemic pandemic);
        void AddPharmacy(Pharmacy pharmacy);
        Task<bool> AddPharmacyHead(PharmacyHeadDto pharmacyHead);
        Task AddWorker(HealthcareWorker worker);
        Task<IEnumerable<HealthcareWorker>> GetAllWorkers();
        Task<IEnumerable<HealthFacility>> GetFacilities();
        Task<HealthFacility> GetFacility(int id);
        HealthFacility GetFacilityJSON(string healthFacility);
        Task<Medicine> GetMedicine(int id);
        Task<List<MedicineDTO>> GetMedicines();
        Pandemic GetPandemic();
        Task<List<PharmacyDTO>> GetPharmacies();
        Task<Pharmacy> GetPharmacy(int id);
        Task<HealthcareWorker> GetWorker(int id);
        Task RemoveMedicine(Medicine medicine);
        Task RemovePharmacy(Pharmacy pharmacy);
        Task RemovePharmacyHead(int Id);
        Task<IEnumerable<HealthFacility>> SearchFacilities(string query);
        Task<IEnumerable<MedicineDTO>> SearchMedicines(string query);
        Task<IEnumerable<PharmacyDTO>> SearchPharmacies(string query);
        Task<IEnumerable<HealthcareWorker>> SearchWorkers(string query);
        Task UpdateFacility(HealthFacility healthFacilities);
        Task UpdateMedicine(Medicine medicine);
        Task UpdatePandemic(Pandemic pandemic);
        Task UpdatePharmacy(Pharmacy pharmacy);
        Task UpdateWorker(HealthcareWorker worker);
    }
}