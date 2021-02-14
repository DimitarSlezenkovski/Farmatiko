using FarmatikoData.DTOs;
using FarmatikoData.FarmatikoRepoInterfaces;
using FarmatikoData.Models;
using FarmatikoServices.FarmatikoServiceInterfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmatikoServices.Services
{
    public class Service : IService
    {
        private readonly IRepository _repository;
        private readonly IPHRepo _phrepo;
        private readonly ILogger _logger;
        public Service(IRepository repository, IPHRepo phrepo, ILogger logger)
        {
            _repository = repository;
            _phrepo = phrepo;
            _logger = logger;
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

        public async Task<List<MedicineDTO>> GetMedicines()
        {
            var Medicines = await _repository.GetMedicinesAsync();
            List<MedicineDTO> list = new List<MedicineDTO>();
            var listPHMedicines = await _repository.GetAllPHMedicines();
            List<string> headNames = new List<string>();
            List<PharmacyHead> heads = new List<PharmacyHead>();
            foreach (var med in Medicines)
            {
                var meds = listPHMedicines.Where(x => x.MedicineId == med.Id).ToList();
                if (meds != null)
                {
                    heads = meds.Select(x => x.Head).ToList();
                }
                headNames = heads.Select(x => x.Name).ToList();
                MedicineDTO medicine = new MedicineDTO()
                {
                    Name = med.Name,
                    Manufacturer = med.Manufacturer,
                    Packaging = med.Packaging,
                    Form = med.Form,
                    Price = med.Price,
                    Strength = med.Strength,
                    WayOfIssuing = med.WayOfIssuing,
                    HeadNames = headNames
                };

                list.Add(medicine);
            }

            return list;
        }

        public Pandemic GetPandemic()
        {
            //var Pandemic = await _repository.GetPandemic();

            try
            {
                var Date = DateTime.UtcNow.ToString("yyyy-MM-dd");
                var client = new RestClient($"https://api.covid19tracking.narrativa.com/api/{Date}/country/north_macedonia");
                var response = client.Execute(new RestRequest());
                string original = response.Content;
                var jsonResponsePandemic = JObject.Parse(original);
                if (!jsonResponsePandemic.ContainsKey("total"))
                {
                    Date = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd");
                    client = new RestClient($"https://api.covid19tracking.narrativa.com/api/{Date}/country/north_macedonia");
                    response = client.Execute(new RestRequest());
                    original = response.Content;
                    jsonResponsePandemic = JObject.Parse(original);
                    if (!jsonResponsePandemic.ContainsKey("total"))
                    {
                        Date = DateTime.UtcNow.AddDays(-2).ToString("yyyy-MM-dd");
                        client = new RestClient($"https://api.covid19tracking.narrativa.com/api/{Date}/country/north_macedonia");
                        response = client.Execute(new RestRequest());
                        original = response.Content;
                        jsonResponsePandemic = JObject.Parse(original);
                    }
                }
                var global = JObject.Parse(jsonResponsePandemic.GetValue("total").ToString());
                var TotalConfirmed = long.Parse(global.GetValue("today_confirmed").ToString());
                var TotalDeaths = long.Parse(global.GetValue("today_deaths").ToString());
                var TotalRecovered = long.Parse(global.GetValue("today_new_recovered").ToString());

                var mk = JObject.Parse(jsonResponsePandemic.GetValue("dates").ToString());

                var date = JObject.Parse(mk.GetValue(Date).ToString());
                var country = JObject.Parse(date.GetValue("countries").ToString());
                var mkd = JObject.Parse(country.GetValue("North Macedonia").ToString());
                dynamic objP = mkd;
                var TotalMk = Int32.Parse(objP.GetValue("today_confirmed").ToString());
                var TotalDeathsMK = Int32.Parse(objP.GetValue("today_deaths").ToString());
                var TotalRecoveredMK = Int32.Parse(objP.GetValue("today_recovered").ToString());
                var NewMK = Int32.Parse(objP.GetValue("today_new_confirmed").ToString());

                var Name = "Coronavirus";
                var ActiveMk = TotalMk - (TotalRecoveredMK + TotalDeathsMK);
                var ActiveGlobal = TotalConfirmed - (TotalRecovered + TotalDeaths);

                Pandemic pandemic = new Pandemic(Name, TotalMk, ActiveMk, TotalDeathsMK, NewMK, TotalConfirmed, TotalDeaths, ActiveGlobal);
                return pandemic;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }
            return null;
        }

        public async Task<List<PharmacyDTO>> GetPharmacies()
        {
            var Pharmacies = await _repository.GetPharmacies();
            List<PharmacyDTO> pharmacies = new List<PharmacyDTO>();

            foreach (var pharm in Pharmacies)
            {
                PharmacyDTO pharmacyDTO = new PharmacyDTO()
                {
                    Name = pharm.Name,
                    Location = pharm.Location,
                    Address = pharm.Address,
                    WorkAllTime = pharm.WorkAllTime
                };
                if (pharm.PharmacyHead != null)
                {
                    pharmacyDTO.HeadName = pharm.PharmacyHead.Name;
                }

                pharmacies.Add(pharmacyDTO);
            }
            return pharmacies;
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

        public async Task<IEnumerable<MedicineDTO>> SearchMedicines(string query)
        {
            var SearchQuery = await _repository.SearchMedicines(query);
            List<MedicineDTO> list = new List<MedicineDTO>();
            var listPHMedicines = await _repository.GetAllPHMedicines();
            List<string> headNames = new List<string>();
            List<PharmacyHead> heads = new List<PharmacyHead>();
            foreach (var med in SearchQuery)
            {
                var meds = listPHMedicines.Where(x => x.MedicineId == med.Id).ToList();
                if (meds != null)
                {
                    heads = meds.Select(x => x.Head).ToList();
                }
                if (heads != null)
                    headNames = heads?.Select(x => x?.Name).ToList();
                MedicineDTO medicine = new MedicineDTO()
                {
                    Name = med.Name,
                    Manufacturer = med.Manufacturer,
                    Packaging = med.Packaging,
                    Form = med.Form,
                    Price = med.Price,
                    Strength = med.Strength,
                    WayOfIssuing = med.WayOfIssuing,
                    HeadNames = headNames
                };

                list.Add(medicine);
                headNames = new List<string>();
            }

            return list;
        }

        public async Task<IEnumerable<PharmacyDTO>> SearchPharmacies(string query)
        {
            var SearchQuery = await _repository.SearchPharmacies(query);
            List<PharmacyDTO> pharmacies = new List<PharmacyDTO>();
            var heads = await _phrepo.GetPharmacyHeadInfo();

            foreach (var pharm in SearchQuery)
            {
                PharmacyDTO pharmacyDTO = new PharmacyDTO()
                {
                    Name = pharm.Name,
                    Location = pharm.Location,
                    Address = pharm.Address,
                    WorkAllTime = pharm.WorkAllTime
                };

                foreach (var head in heads.ToList())
                {
                    if (head.Pharmacies.Contains(pharm))
                    {
                        pharmacyDTO.HeadName = head.Name;
                        break;
                    }
                }

                pharmacies.Add(pharmacyDTO);
            }
            return pharmacies;
        }

        public async Task<IEnumerable<HealthcareWorker>> SearchWorkers(string query)
        {
            var SearchQuery = await _repository.SearchWorkers(query);
            return SearchQuery;
        }


        //POST (ADD NEW OBJECTS)
        //za json(Sys updateer)
        public async Task AddFacility(HealthFacility healthFacility)
        {
            if (healthFacility != null)
            {
                var facilities = await _repository.GetFacilities();
                if (!facilities.Contains(healthFacility))
                {
                    await _repository.AddFacility(healthFacility);
                }
                else throw new Exception("The facility already exists.");
            }
            else throw new Exception("Facility is null");
        }
        //za json(Sys updateer)
        public async Task AddMedicines(Medicine medicine)
        {
            if (medicine != null)
            {
                var medicines = await _repository.GetMedicinesAsync();
                if (!medicines.Contains(medicine))
                    await _repository.AddMedicines(medicine);
                else throw new Exception("Medicine already exists.");
            }
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
        public async void AddPharmacy(Pharmacy pharmacy)
        {
            if (pharmacy != null)
            {
                var pharmacies = await _repository.GetPharmacies();
                if (!pharmacies.Contains(pharmacy))
                    await _repository.AddPharmacy(pharmacy);
                else throw new Exception("Pharmacy already exists.");
            }
            else throw new Exception("Pharmacy is null");
        }

        // Ovaa kontrola ja ima samo admin

        public async Task<bool> AddPharmacyHead(PharmacyHeadDto pharmacyHead)
        {
            if (pharmacyHead != null)
            {
                var phead = new PharmacyHead()
                {
                    Name = pharmacyHead.Name,
                    Email = pharmacyHead.Email,
                    Password = pharmacyHead.Password
                };
                User user = new User()
                {
                    Name = phead.Name,
                    Password = phead.Password,
                    Email = phead.Email,
                    UserRole = User.Role.PharmacyHead
                };
                if (user is null)
                {
                    return false;
                }
                User user1 = new User()
                {
                    Name = user.Name,
                    Password = user.Password,
                    Email = user.Email,
                    UserRole = user.UserRole
                };
                phead.User = user1;
                await _repository.AddPharmacyHead(phead);
                return true;
            }
            else throw new Exception("PharmacyHeadDto is null");
        }
        //za json(Sys updater)
        public async Task AddWorker(HealthcareWorker worker)
        {
            if (worker != null)
            {
                var workers = await _repository.GetAllWorkers();
                if (!workers.Contains(worker))
                    await _repository.AddWorker(worker);
                else throw new Exception("Worker already exists.");
            }
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
