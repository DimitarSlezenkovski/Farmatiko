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
            var Workers = await _context.HealthcareWorkers.Select(x => new HealthcareWorker 
            { 
                Id = x.Id,
                Name = x.Name,
                Branch = x.Branch,
                Facility = x.Facility,
                Title = x.Title
            }).Take(5).ToListAsync();
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

            }).Take(5).ToListAsync();
            return Medicines;
        }

        public async Task<Pandemic> GetPandemic()
        {
            var Pandemic = await _context.Pandemics.FirstOrDefaultAsync();
            return Pandemic;
        }
        public async Task<List<Pharmacy>> GetPharmacies()
        {
            var Pharmacies = await _context.Pharmacies.Select(x => new Pharmacy 
            {
                Name = x.Name,
                Location = x.Location,
                Address = x.Address,
                WorkAllTime = x.WorkAllTime,
                PheadId = x.PheadId,
                PharmacyHead = x.PharmacyHead
            }).Take(5).ToListAsync();
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
            .Where(x => x.Name.ToLower().Contains(query.ToLower())
            || x.Municipality.ToLower().Contains(query.ToLower())
            || x.Type.ToLower().Contains(query.ToLower())).Take(5)
            .OrderBy(x => x.Name).ToListAsync();

            return SearchQuery;
        }

        public async Task<IEnumerable<Medicine>> SearchMedicines(string query)
        {
            var SearchQuery = await _context.Medicines
            .Where(x => x.Name.ToLower().Contains(query.ToLower()) 
            || x.Form.ToLower().Contains(query.ToLower()) 
            || x.Strength.ToLower().Contains(query.ToLower()) 
            || x.Packaging.ToLower().Contains(query.ToLower())).Take(20)
            .OrderBy(x => x.Name).ToListAsync();

            return SearchQuery;
        }

        public async Task<IEnumerable<Pharmacy>> SearchPharmacies(string query)
        {
            var SearchQuery = await _context.Pharmacies
            .Where(x => x.Name.ToLower().Contains(query.ToLower())
            || x.PharmacyHead.Name.ToLower().Contains(query.ToLower())).Take(5)
            .OrderBy(x => x.Name).ToListAsync();

            return SearchQuery;
        }

        public async Task<IEnumerable<HealthcareWorker>> SearchWorkers(string query)
        {
            var SearchQuery = await _context.HealthcareWorkers.Include(x => x.Facility)
            .Where(x => x.Name.ToLower().Contains(query.ToLower())
            || x.Facility.Name.ToLower().Contains(query.ToLower())).Take(20)
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
            pharmacy.Id = 0;
            if (pharmacy.Id == 0)
            {
                var phars = _context.Pharmacies.Select(x => new Pharmacy
                {
                    Name = x.Name,
                    Location = x.Location,
                    Address = x.Address
                }).ToList();
                var pharms = phars.Where(x => x.Name.Equals(pharmacy.Name) && x.Location.Equals(pharmacy.Location) && x.Address.Equals(pharmacy.Address)).ToList();
                if (pharms is null || pharms.Count() == 0)
                {
                    await _context.Pharmacies.AddAsync(pharmacy);
                    _context.SaveChanges();
                }
                
            }
        }

        public async Task AddPharmacyHead(PharmacyHead pharmacyHead)
        {
            pharmacyHead.Id = 0;
            if (pharmacyHead.Id == 0)
            {
                var pheads = await _context.PharmacyHeads.Select(x => new PharmacyHead 
                {
                    Name = x.Name,
                    Email = x.Email
                }).ToListAsync();
                var pheadusr = pheads.Where(x => x.Email.Equals(pharmacyHead.Email)).ToList();
                if (pheadusr == null || pheadusr.Count() == 0)
                {
                    await _context.PharmacyHeads.AddAsync(pharmacyHead);
                    await _context.SaveChangesAsync();
                }
            }
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
            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();
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
            _context.Pharmacies.Remove(pharmacy);
            await _context.SaveChangesAsync();
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
        //not implemented, not needed 
        public Task UpdateMedicine(Medicine medicine)
        {
            throw new NotImplementedException();
        }

        public async Task RemovePharmacyHead(int Id)
        {
            var PHead = await _context.PharmacyHeads.Where(x => x.Id == Id).Include(x => x.Pharmacies).Include(x => x.Medicines).FirstOrDefaultAsync();
            var user = await _context.Users.Where(x => x.Email.Equals(PHead.Email)).FirstOrDefaultAsync();
            var PHreqs = await _context.PHRequests.Where(x => x.Head.Id.Equals(PHead.Id)).FirstOrDefaultAsync();
            PHead.Pharmacies.Select(x => x.PheadId = null);
            PHead.Pharmacies.Select(x => x.PharmacyHead = null);
            _context.PHRequests.Remove(PHreqs);
            _context.PharmacyHeads.Remove(PHead);
            _context.Users.Remove(user);
            _context.SaveChanges();
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
                Medicines = x.Medicines

            }).ToList();
            return Medicines; 
        }

        public ICollection<PharmacyHeadMedicine> GetPHMedicines(string email)
        {
            var head = _context.PharmacyHeads.Where(x => x.Email.Equals(email)).FirstOrDefault();
            var phmeds = _context.PharmacyHeadMedicines.Where(x => x.PheadId == head.Id).Include(x => x.Medicine).ToList();
            return phmeds;
        }
        
        /*public async Task<bool> AddUser(User user)
        {
            if (user.Id == 0)
            {
                var users = await _context.Users.Select(x => new User
                {
                    Name = x.Name,
                    Email = x.Email,
                    Password = x.Password,
                    UserRole = x.UserRole
                }).ToListAsync();
                var usr = users.Where(x => x.Email.Equals(user.Email)).ToList();
                if (usr != null && usr.Count() > 0)
                {
                    return true;
                }
                else
                {
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }*/

        public async Task<List<PharmacyHeadMedicine>> GetAllPHMedicines()
        {
            var list = await _context.PharmacyHeadMedicines.Select(x => new PharmacyHeadMedicine
            {
                PheadId = x.PheadId,
                Head = x.Head,
                MedicineId = x.MedicineId,
                Medicine = x.Medicine
            }
            ).ToListAsync();
            return list;
        }

        public ICollection<PharmacyHeadMedicine> GetPHMedicines()
        {
            var meds = _context.PharmacyHeadMedicines.Select(x => new PharmacyHeadMedicine
            {
                PheadId = x.PheadId,
                Head = x.Head,
                MedicineId = x.MedicineId,
                Medicine = x.Medicine
            }).ToList();
            return meds;
        }
    }
}
