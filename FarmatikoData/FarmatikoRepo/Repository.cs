using FarmatikoData.FarmatikoRepoInterfaces;
using FarmatikoData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmatikoData.FarmatikoRepo
{
    public class Repository : IRepository
    {
        private readonly FarmatikoDataContext _context;
        public Repository(FarmatikoDataContext context)
        {
            _context = context;
        }
        //GET
        public async Task<IEnumerable<HealthcareWorker>> GetAllWorkers()
        {
            var Workers = await _context.HealthcareWorkers.Take(5).ToListAsync();
            return Workers;
        }

        public async Task<IEnumerable<HealthFacility>> GetFacilities()
        {
            var Facilities = await _context.HealthFacilities.Take(5).ToListAsync();
            return Facilities;
        }

        public async Task<HealthFacility> GetFacility(int Id)
        {
            var Facility = await _context.HealthFacilities.FindAsync(Id);
            return Facility;
        }

        public async Task<Medicine> GetMedicine(int Id)
        {
            var Medicine = await _context.Medicines.FindAsync(Id);
            return Medicine;
        }

        public async Task<IEnumerable<Medicine>> GetMedicinesAsync()
        {
            var Medicines = await _context.Medicines.Select(x => new Medicine
            {
                Id = x.Id,
                Name = x.Name,
                Strength = x.Strength,
                Form = x.Form,
                WayOfIssuing = x.WayOfIssuing,
                Manufacturer = x.Manufacturer,
                Price = x.Price,
                Packaging = x.Packaging

            }).Take(3).ToListAsync();
            return Medicines;
        }

        public async Task<Pandemic> GetPandemic()
        {
            var Pandemic = await _context.Pandemics.FirstOrDefaultAsync();
            return Pandemic;
        }

        public async Task<IEnumerable<Pharmacy>> GetPharmacies()
        {
            var Pharmacies = await _context.Pharmacies.Take(5).ToListAsync();
            return Pharmacies;
        }

        public async Task<Pharmacy> GetPharmacy(int id)
        {
            var Pharmacy = await _context.Pharmacies.FindAsync(id);
            return Pharmacy;
        }

        public async Task<HealthcareWorker> GetWorker(int id)
        {
            var Worker = await _context.HealthcareWorkers.FindAsync(id);
            return Worker;
        }

        public async Task<IEnumerable<HealthFacility>> SearchFacilities(string query)
        {
            var SearchQuery = await _context.HealthFacilities
            .Where(x => x.Name.Contains(query))
            .OrderBy(x => x.Name).ToListAsync();

            return SearchQuery;
        }

        public async Task<IEnumerable<Medicine>> SearchMedicines(string query)
        {
            var SearchQuery = await _context.Medicines
            .Where(x => x.Name.Contains(query))
            .OrderBy(x => x.Name).ToListAsync();

            return SearchQuery;
        }

        public async Task<IEnumerable<Pharmacy>> SearchPharmacies(string query)
        {
            var SearchQuery = await _context.Pharmacies.Take(5)
            .Where(x => x.Name.Contains(query))
            .OrderBy(x => x.Name).ToListAsync();

            return SearchQuery;
        }

        public async Task<IEnumerable<HealthcareWorker>> SearchWorkers(string query)
        {
            var SearchQuery = await _context.HealthcareWorkers.Take(5)
            .Where(x => x.Name.Contains(query))
            .OrderBy(x => x.Name).ToListAsync();

            return SearchQuery;
        }
        public HealthFacility GetFacilityJSON(string healthFacility)
        {
            var Facility = _context.HealthFacilities.Where(x => x.Name.Equals(healthFacility)).FirstOrDefault();
            return Facility;
        }

        //POST

        public async Task AddWorker(HealthcareWorker Worker)
        {
            await _context.HealthcareWorkers.AddAsync(Worker);
            _context.SaveChanges();
        }

        public async Task AddFacility(HealthFacility healthFacility)
        {
            await _context.HealthFacilities.AddAsync(healthFacility);
            _context.SaveChanges();
        }

        public async Task AddPharmacy(Pharmacy pharmacy)
        {
            await _context.Pharmacies.AddAsync(pharmacy);
            _context.SaveChanges();
        }

        public async Task AddPharmacyHead(PharmacyHead pharmacyHead)
        {
            await _context.PharmacyHeads.AddAsync(pharmacyHead);
            _context.SaveChanges();
        }

        public async Task AddMedicines(Medicine medicine)
        {
            await _context.Medicines.AddAsync(medicine);
            _context.SaveChanges();
        }

        public async Task AddPandemic(Pandemic pandemic)
        {
            var pand = await _context.Pandemics.AddAsync(pandemic);
            _context.SaveChanges();
        }

        public async Task UpdateFacility(HealthFacility healthFacility)
        {
            var Facility = await _context.HealthFacilities.Where(x => x.Id == healthFacility.Id).FirstOrDefaultAsync();
            Facility.Address = healthFacility.Address;
            Facility.Email = healthFacility.Email;
            Facility.Municipality = healthFacility.Municipality;
            Facility.Name = healthFacility.Name;
            Facility.Phone = healthFacility.Phone;
            Facility.Type = healthFacility.Type;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveMedicine(Medicine medicine)
        {
            await Task.Run(() => _context.Medicines.Remove(medicine));
            _context.SaveChanges();
        }

        public async Task UpdatePandemic(Pandemic pandemic)
        {
            var Pandemic = await _context.Pandemics.Where(x => x.Id == pandemic.Id).FirstOrDefaultAsync();
            Pandemic.ActiveGlobal = pandemic.ActiveGlobal;
            Pandemic.ActiveMK = pandemic.ActiveMK;
            Pandemic.DeathsGlobal = pandemic.DeathsGlobal;
            Pandemic.DeathsMK = pandemic.DeathsMK;
            Pandemic.Name = pandemic.Name;
            Pandemic.NewMK = pandemic.NewMK;
            Pandemic.TotalGlobal = pandemic.TotalGlobal;
            Pandemic.TotalMK = pandemic.TotalMK;
            await _context.SaveChangesAsync();
        }

        public async Task RemovePharmacy(Pharmacy pharmacy)
        {
            await Task.Run(() => _context.Pharmacies.Remove(pharmacy));
            _context.SaveChanges();
        }
        //not impl
        public Task UpdateWorker(HealthcareWorker worker)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpadatePharmacy(Pharmacy pharmacy)
        {
            var Pharmacy = await _context.Pharmacies.Where(x => x.Id == pharmacy.Id).FirstOrDefaultAsync();
            Pharmacy.Name = pharmacy.Name;
            Pharmacy.Location = pharmacy.Location;
            Pharmacy.WorkAllTime = pharmacy.WorkAllTime;
            Pharmacy.Address = pharmacy.Address;
            await _context.SaveChangesAsync();
        }
        //ke vidime
        public Task UpdateMedicine(Medicine medicine)
        {
            throw new NotImplementedException();
        }

        public async Task RemovePharmacyHead(int Id)
        {
            var PHead = await _context.PharmacyHeads.Where(x => x.Id == Id).FirstOrDefaultAsync();
            PHead.DeletedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public IDictionary<string, User> GetUsers()
        {
            var users = _context.Users.ToDictionary(x => x.Email, x => new User
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Password = x.Password,
                UserRole = x.UserRole
            });
            return users;
        }

        public User GetRole(string userName)
        {
            var user = _context.Users.Where(x => x.Email.Equals(userName)).FirstOrDefault();
            return user;
        }

        public ICollection<Medicine> GetMedicines()
        {
            var Medicines = _context.Medicines.Select(x => new Medicine
            {
                Id = x.Id,
                Name = x.Name,
                Strength = x.Strength,
                Form = x.Form,
                WayOfIssuing = x.WayOfIssuing,
                Manufacturer = x.Manufacturer,
                Price = x.Price,
                Packaging = x.Packaging,
                MedicineList = x.MedicineList

            }).ToList();
            return Medicines; 
        }

        public ICollection<PharmacyHeadMedicine> GetPHMedicines(string email)
        {
            var head = _context.PharmacyHeads.Where(x => x.Email.Equals(email)).FirstOrDefault();
            var phmeds = _context.PharmacyHeadMedicines.Where(x => x.PheadId == head.Id).ToList();
            return phmeds;
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
