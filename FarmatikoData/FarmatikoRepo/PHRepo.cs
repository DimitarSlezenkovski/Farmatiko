using FarmatikoData.FarmatikoRepoInterfaces;
using FarmatikoData.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmatikoData.FarmatikoRepo
{
    public class PHRepo : IPHRepo
    {
        private readonly FarmatikoDataContext _context;
        public PHRepo(FarmatikoDataContext context)
        {
            _context = context;
        }
        //GET
        public async Task<PharmacyHead> GetPharmacyHeadByIdAsync(int id)
        {
            var Phead = await _context.PharmacyHeads.Where(x => x.Id == id).FirstOrDefaultAsync();
            return Phead;
        }

        public async Task<IEnumerable<PharmacyHead>> GetPharmacyHeadInfo()
        {
            var PHeadInfo = await _context.PharmacyHeads.Take(10)
                .Where(x => x.DeletedOn == null)
                .Select(x => new PharmacyHead
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Password = x.Password,
                    Pharmacies = x.Pharmacies,
                    Medicines = x.Medicines

                }).ToListAsync();
            return PHeadInfo;
        }
        //POST
        public async Task UpdatePharmacyHead(PharmacyHead pharmacyHead)
        {
            var user = await _context.Users.Where(x => x.Email == pharmacyHead.Email).FirstOrDefaultAsync();
            var EditedPHead = await _context.PharmacyHeads.Where(x => x.Email.Equals(pharmacyHead.Email)).FirstOrDefaultAsync();

            /*if (!EditedPHead.Email.Equals(pharmacyHead.Email) && !user.Email.Equals(pharmacyHead.Email))
            {
                EditedPHead.Email = pharmacyHead.Email;
                user.Email = pharmacyHead.Email;
            }*/

            if (!EditedPHead.Name.Equals(pharmacyHead.Name) || !user.Name.Equals(pharmacyHead.Name))
            {
                EditedPHead.Name = pharmacyHead.Name;
                user.Name = pharmacyHead.Name;
            }

            if (!EditedPHead.Password.Equals(pharmacyHead.Password) || !user.Password.Equals(pharmacyHead.Password))
            {
                EditedPHead.Password = pharmacyHead.Password;
                user.Password = pharmacyHead.Password;
            }
            foreach(var pharmacy in pharmacyHead.Pharmacies)
            {
                if (!EditedPHead.Pharmacies.Contains(pharmacy))
                {
                    pharmacy.PheadId = EditedPHead.Id;
                    pharmacy.PharmacyHead = EditedPHead;
                    EditedPHead.Pharmacies.Add(pharmacy);
                }
            }
            _context.Entry(EditedPHead).State = EntityState.Modified;
            
            _context.SaveChanges();
        }
        public async Task ClaimPharmacy(RequestPharmacyHead pharmacy)
        {
            var phead = _context.PharmacyHeads.Where(x => x.Email.Equals(pharmacy.Head.Email)).FirstOrDefault();
            pharmacy.Head = phead;
            await _context.PHRequests.AddAsync(pharmacy);
            await _context.SaveChangesAsync();
        }
        public async Task Add(PharmacyHead pharmacyHead)
        {
            await _context.PharmacyHeads.AddAsync(pharmacyHead);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(PharmacyHead phead)
        {
            var Phead = await _context.PharmacyHeads.Where(x => x.Id == phead.Id).FirstOrDefaultAsync();
            Phead.DeletedOn = phead.DeletedOn;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveClaimingRequest(int id)
        {
            var req = await _context.PHRequests.Where(r => r.Id == id).FirstOrDefaultAsync();
            _context.PHRequests.Remove(req);
            await _context.SaveChangesAsync();
        }


        public PharmacyHead GetPharmacyHeadByUserName(string userName)
        {

                       
            var PHead = _context.PharmacyHeads
                .Where(x => x.Email.Equals(userName))
                .Select(x => new PharmacyHead 
                {
                   Email = x.Email,
                   Name = x.Name,
                   Password = x.Password,
                   Medicines = x.Medicines,
                   Pharmacies = x.Pharmacies
                }).FirstOrDefault();

            

            return PHead;
        }

        public List<PharmacyHeadMedicine> GetPharmacyHeadMedicines(string email)
        {
            var Phead = _context.PharmacyHeads.Where(x => x.Email.Equals(email)).FirstOrDefault();
            var Medicines = _context.PharmacyHeadMedicines.Select(x => new PharmacyHeadMedicine 
            { 
                PheadId = x.PheadId,
                Head = x.Head,
                MedicineId = x.MedicineId,
                Medicine = x.Medicine
            }).ToList();
            if (Medicines == null || Medicines == default)
                Medicines = null;
            var meds = Medicines.Where(x => x.PheadId == Phead.Id).ToList();

            return meds;
        }

        public IEnumerable<PharmacyHead> GetPharmacyHeads()
        {
            var heads = _context.PharmacyHeads.ToList();
            return heads;
        }

        public PharmacyHead GetPharmacyHead(string head)
        {
            var phead = _context.PharmacyHeads.Where(x => x.Email.Equals(head)).Include(x => x.Pharmacies).FirstOrDefault();
            return phead;
        }

        public List<Pharmacy> GetPharmacies()
        {
            var pharms = _context.Pharmacies.ToList();
            return pharms;
        }

        public void DeletePHMedicine(int id, int phId, int medId)
        {
            var PH = _context.PharmacyHeadMedicines.Where(x => x.PheadId == phId).Single();
            var PHMed = _context.PharmacyHeadMedicines.Where(x => x.MedicineId == medId).Single();
            var PHID = _context.PharmacyHeadMedicines.Where(x => x.Id == id).Single();
            _context.PharmacyHeadMedicines.Remove(PHID);
            _context.SaveChanges();
        }
    }
}
