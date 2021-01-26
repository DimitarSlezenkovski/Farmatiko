using FarmatikoData.FarmatikoRepoInterfaces;
using FarmatikoData.Models;
using FarmatikoServices.FarmatikoServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmatikoServices.Services
{
    public class Service : IService
    {
        private readonly IRepository _repository;
        public Service(IRepository repository)
        {
            _repository = repository;
        }

        //GET
        public async Task<IEnumerable<HealthcareWorker>> GetAllWorkers()
        {
            var Workers = await _repository.GetAllWorkers();
            return Workers;
        }

        public async Task<IEnumerable<HealthFacility>> GetFacilities()
        {
            var Facilities = await _repository.GetFacilities();
            return Facilities;
        }

        public async Task<HealthFacility> GetFacility(int id)
        {
            var Facility = await _repository.GetFacility(id);
            return Facility;
        }

        public async Task<Medicine> GetMedicine(int id)
        {
            var Medicine = await _repository.GetMedicine(id);
            return Medicine;
        }

        public async Task<IEnumerable<Medicine>> GetMedicines()
        {
            var Medicines = await _repository.GetMedicinesAsync();
            return Medicines;
        }

        public async Task<Pandemic> GetPandemic()
        {
            var Pandemic = await _repository.GetPandemic();
            return Pandemic;
        }

        public async Task<IEnumerable<Pharmacy>> GetPharmacies()
        {
            var Pharmacies = await _repository.GetPharmacies();
            return Pharmacies;
        }

        public async Task<Pharmacy> GetPharmacy(int id)
        {
            var Pharmacy = await _repository.GetPharmacy(id);
            return Pharmacy;
        }

        public async Task<HealthcareWorker> GetWorker(int id)
        {
            var Worker = await _repository.GetWorker(id);
            return Worker;
        }

        public async Task<IEnumerable<HealthFacility>> SearchFacilities(string query)
        {
            var SearchQuery = await _repository.SearchFacilities(query);
            return SearchQuery;
        }

        public async Task<IEnumerable<Medicine>> SearchMedicines(string query)
        {
            var SearchQuery = await _repository.SearchMedicines(query);
            return SearchQuery;
        }

        public async Task<IEnumerable<Pharmacy>> SearchPharmacies(string query)
        {
            var SearchQuery = await _repository.SearchPharmacies(query);
            return SearchQuery;
        }

        public async Task<IEnumerable<HealthcareWorker>> SearchWorkers(string query)
        {
            var SearchQuery = await _repository.SearchWorkers(query);
            return SearchQuery;
        }


        //POST (ADD NEW OBJECTS)
        //za json(Sys updateer)
        public async Task AddFacility(HealthFacility healthFacilities)
        {
            if (healthFacilities != null)
                await _repository.AddFacility(healthFacilities);
            else throw new Exception("Facility is null");
        }
        //za json(Sys updateer)
        public async Task AddMedicines(Medicine medicine)
        {
            if (medicine != null)
                await _repository.AddMedicines(medicine);
            else throw new Exception("Medicine is null");
        }
        //za json(Sys updateer)
        public async Task AddPandemic(Pandemic pandemic)
        {
            if (pandemic != null)
                await _repository.AddPandemic(pandemic);
            else throw new Exception("Pandemic is null");
        }
        // Samo PharmacyHead i Admin imaat pristap
        public async Task AddPharmacy(Pharmacy pharmacy)
        {
            if (pharmacy != null)
                await _repository.AddPharmacy(pharmacy);
            else throw new Exception("Pharmacy is null");
        }

        // Ovaa kontrola ja ima samo admin
        public User MakeUser(PharmacyHead head)
        {
            
            
            User user = new User()
            {
                Name = head.Name,
                Password = head.Password,
                Email = head.Email,
                UserRole = User.Role.PharmacyHead
            };
            return user;
        }
        public async Task AddPharmacyHead(PharmacyHead pharmacyHead)
        {
            if (pharmacyHead != null)
            {
                var user = MakeUser(pharmacyHead);
                await _repository.AddUser(user);                
                await _repository.AddPharmacyHead(pharmacyHead);
            }
            else throw new Exception("PharmacyHead is null");
        }
        //za json(Sys updater)
        public async Task AddWorker(HealthcareWorker worker)
        {
            if (worker != null)
                await _repository.AddWorker(worker);
            else throw new Exception("Worker is null");
        }

        //za json(Sys updateer)
        public async Task UpdateFacility(HealthFacility healthFacilities)
        {
            if (healthFacilities != null)
                await _repository.UpdateFacility(healthFacilities);
            else throw new Exception("Facility is null");
        }
        //PharmacyHead
        public async Task RemoveMedicine(Medicine medicine)
        {
            if (medicine != null)
                await _repository.RemoveMedicine(medicine);
            else throw new Exception("Medicine is null");
        }
        //PharmacyHead
        public async Task UpdateMedicine(Medicine medicine)
        {
            if (medicine != null)
                await _repository.UpdateMedicine(medicine);
            else throw new Exception("Medicine is null");
        }
        //za json(Sys updateer)
        public async Task UpdatePandemic(Pandemic pandemic)
        {
            if (pandemic != null)
                await _repository.UpdatePandemic(pandemic);
            else throw new Exception("Pandemic is null");
        }
        //PharmacyHead
        public async Task RemovePharmacy(Pharmacy pharmacy)
        {
            if (pharmacy != null)
                await _repository.RemovePharmacy(pharmacy);
            else throw new Exception("Pharmacy is null");
        }
        //PharamcyHead
        public async Task UpdatePharmacy(Pharmacy pharmacy)
        {
            if (pharmacy != null)
                await _repository.UpadatePharmacy(pharmacy);
            else throw new Exception("Pharmacy is null");
        }
        //za json(Sys updateer)
        public async Task UpdateWorker(HealthcareWorker worker)
        {
            if (worker != null)
                await _repository.UpdateWorker(worker);
            else throw new Exception("Worker is null");
        }

        public async Task RemovePharmacyHead(int Id)
        {
            if (Id > 0)
            {
                await _repository.RemovePharmacyHead(Id);
            }
            else throw new Exception("Index out of bounds.");
        }

        public HealthFacility GetFacilityJSON(string healthFacility)
        {
            if (healthFacility != null)
                return _repository.GetFacilityJSON(healthFacility);
            return null;
        }

        //PUT (EDIT OBJECTS)


        //DELETE

    }
}
