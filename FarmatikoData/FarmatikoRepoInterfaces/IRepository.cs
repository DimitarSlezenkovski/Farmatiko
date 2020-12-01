using FarmatikoData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmatikoData.FarmatikoRepoInterfaces
{
    public interface IRepository
    {
        //GET
        Task<IEnumerable<HealthcareWorker>> GetAllWorkers();
        Task<IEnumerable<HealthFacility>> GetFacilities();
        Task<HealthFacility> GetFacility(int Id);
        Task<Medicine> GetMedicine(int Id);
        Task<IEnumerable<Medicine>> GetMedicinesAsync();
        ICollection<Medicine> GetMedicines();
        Task<Pandemic> GetPandemic();
        Task<IEnumerable<Pharmacy>> GetPharmacies();
        Task<Pharmacy> GetPharmacy(int id);
        Task<HealthcareWorker> GetWorker(int id);
        Task<IEnumerable<HealthFacility>> SearchFacilities(string query);
        Task<IEnumerable<Medicine>> SearchMedicines(string query);
        Task<IEnumerable<Pharmacy>> SearchPharmacies(string query);
        Task<IEnumerable<HealthcareWorker>> SearchWorkers(string query);
        HealthFacility GetFacilityJSON(string healthFacility);
        IDictionary<string, User> GetUsers();

        //POST
        Task AddWorker(HealthcareWorker Worker);
        Task AddFacility(HealthFacility healthFacility);
        Task AddPharmacy(Pharmacy pharmacy);
        Task AddPharmacyHead(PharmacyHead pharmacyHead);
        Task AddMedicines(Medicine medicine);
        Task AddPandemic(Pandemic pandemic);
        Task UpdateFacility(HealthFacility healthFacility);
        Task RemoveMedicine(Medicine medicine);
        Task UpdatePandemic(Pandemic pandemic);
        Task RemovePharmacy(Pharmacy pharmacy);
        Task UpdateWorker(HealthcareWorker worker);
        Task UpadatePharmacy(Pharmacy pharmacy);
        Task UpdateMedicine(Medicine medicine);
        Task RemovePharmacyHead(int Id);
        User GetRole(string userName);
        ICollection<PharmacyHeadMedicine> GetPHMedicines(string email);
        Task AddUser(User user);
    }
}
