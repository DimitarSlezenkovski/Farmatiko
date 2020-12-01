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
            var PHeadInfo = await _context.PharmacyHeads.Take(10).Where(x => x.DeletedOn == null)
                .Select(x => new PharmacyHead
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Password = x.Password,
                    MedicineList = x.MedicineList,
                    PharmaciesList = x.PharmaciesList
                }).ToListAsync();
            return PHeadInfo;
        }
        //POST
        public async Task UpdatePharmacyHead(PharmacyHead pharmacyHead)
        {
            var Phead = await _context.PharmacyHeads.Where(x => x.Email == pharmacyHead.Email).FirstOrDefaultAsync();
            var EditedPHead = await _context.PharmacyHeads.AsNoTracking<PharmacyHead>().Where(x => x.Email == pharmacyHead.Email).FirstOrDefaultAsync();
            EditedPHead.Email = pharmacyHead.Email;
            EditedPHead.Name = pharmacyHead.Name;
            EditedPHead.Password = pharmacyHead.Password;
            /*if (pharmacyHead.MedicineList.Count() == 0)
                pharmacyHead.MedicineList = null;*/
            EditedPHead.MedicineList = pharmacyHead.MedicineList;
            EditedPHead.PharmaciesList = pharmacyHead.PharmaciesList;
            EditedPHead.PHMedicineList = pharmacyHead.PHMedicineList;
            //_context.Entry<PharmacyHead>(Phead).State = EntityState.Detached;
            //Phead = EditedPHead;
            await _context.SaveChangesAsync();
        }
        public async Task ClaimPharmacy(RequestPharmacyHead pharmacy)
        {
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
                .FirstOrDefault();

            return PHead;
        }

        public List<PharmacyHeadMedicine> GetPharmacyHeadMedicines(string email)
        {
            /*var meds = _context.Medicines.ToList();
            var medicines = Medicines;*/
            var Phead = _context.PharmacyHeads.Where(x => x.Email.Equals(email)).FirstOrDefault();
            var Medicines = _context.PharmacyHeadMedicines.Where(x => x.PheadId == Phead.Id).ToList();
                /*.Select(x => x.Head.MedicineList)
                .SelectMany(mList => mList)
                .ToList();*/


            return Medicines;
        }

        public IEnumerable<PharmacyHead> GetPharmacyHeads()
        {
            var heads = _context.PharmacyHeads.ToList();
            return heads;
        }

        public PharmacyHead GetPharmacyHead(string head)
        {
            var phead = _context.PharmacyHeads.Where(x => x.Email.Equals(head)).FirstOrDefault();
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
