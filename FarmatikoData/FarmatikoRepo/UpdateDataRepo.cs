using FarmatikoData.FarmatikoRepoInterfaces;
using FarmatikoData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmatikoData.FarmatikoRepo
{
    public class UpdateDataRepo : IUpdateDataRepo
    {
        private readonly FarmatikoDataContext _context;
        public UpdateDataRepo(FarmatikoDataContext context)
        {
            _context = context;
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

        public async Task AddFacility(HealthFacility healthFacility)
        {
            await _context.HealthFacilities.AddAsync(healthFacility);
            _context.SaveChanges();
        }
        public async Task AddWorker(HealthcareWorker Worker)
        {
            await _context.HealthcareWorkers.AddAsync(Worker);
            _context.SaveChanges();
        }

        public async Task AddMedicines(Medicine medicine)
        {
            await _context.Medicines.AddAsync(medicine);
            _context.SaveChanges();
        }

        public HealthFacility GetFacilityJSON(string healthFacility)
        {
            var Facility = _context.HealthFacilities.Where(x => x.Name.Equals(healthFacility)).FirstOrDefault();
            return Facility;
        }
    }
}
