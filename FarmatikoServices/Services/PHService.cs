using FarmatikoData.DTOs;
using FarmatikoData.FarmatikoRepoInterfaces;
using FarmatikoData.Models;
using FarmatikoServices.FarmatikoServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmatikoServices.Services
{
    public class PHService : IPHService
    {
        private readonly IPHRepo _iPHRepo;
        private readonly IRepository _repository;
        public PHService(IPHRepo iPHRepo, IRepository repository)
        {
            _iPHRepo = iPHRepo;
            _repository = repository;
        }

        public async Task<bool> ClaimPharmacy(RequestPharmacyHead pharmacy)
        {
            if (pharmacy != null)
            {
                await _iPHRepo.ClaimPharmacy(pharmacy);
                return true;
            }
            return false;
        }

        public async Task<PharmacyHead> GetPharmacyHeadByIdAsync(int id)
        {
            PharmacyHead Phead = null;
            if (id >= 0)
                Phead = await _iPHRepo.GetPharmacyHeadByIdAsync(id);
            if (Phead != null)
                return Phead;
            throw new Exception("No data found.");
        }

        public async Task<IEnumerable<PharmacyHead>> GetPharmacyHeadInfo()
        {
            var PHeads = await _iPHRepo.GetPharmacyHeadInfo();
            if (PHeads != null)
                return PHeads;
            throw new Exception("No Pharmacy heads found.");
        }

        public async Task<int> Login(PharmacyHead pharmacyHead)
        {
            var PHead = await _iPHRepo.GetPharmacyHeadByIdAsync(pharmacyHead.Id);
            if (PHead.Password.Equals(pharmacyHead.Password))
                return PHead.Id;
            return -1;
        }



        public async Task UpdatePharmacyHead(PharmacyHeadDto pharmacyHead)
        {
            if (pharmacyHead != null)
            {
                var phead = _iPHRepo.GetPharmacyHead(pharmacyHead.Email);

                phead.Medicines = _repository.GetPHMedicines(phead.Email).ToList();
                
                List<Medicine> medicines = _repository.GetMedicines().ToList();
                
                List<Medicine> PHMedicines = medicines.Where(x => x.Id == phead.Medicines.Select(x => x.MedicineId).Single()).ToList();
                
                List<PharmacyHeadMedicine> list = new List<PharmacyHeadMedicine>();

                

                if (!pharmacyHead.Medicines.Equals(PHMedicines))
                {
                    //phead.Medicines = pharmacyHead.Medicines;
                    if (pharmacyHead.Medicines.Count() == 0)
                    {
                        phead.Medicines = null;
                        int PHMId = phead.Medicines.Select(x => x.Id).Single();
                        int phId = phead.Medicines.Select(x => x.PheadId).Single();
                        int medId = phead.Medicines.Select(x => x.MedicineId).Single();
                        _iPHRepo.DeletePHMedicine(PHMId, phId, medId);
                        return;
                    }
                    foreach (var med in pharmacyHead.Medicines)
                    {

                        PharmacyHeadMedicine PHMObj = phead.Medicines.Select(x => new PharmacyHeadMedicine
                        {
                            Id = x.Id,
                            PheadId = x.PheadId,
                            Head = x.Head,
                            MedicineId = x.MedicineId,
                            Medicine = x.Medicine
                        }).Where(x => !x.Medicine.Equals(med)).Single();
                        if (PHMObj == null || PHMObj == default)
                            break;
                        if (PHMObj.MedicineId == med.Id)
                            list.Add(PHMObj);

                    }

                    phead.Medicines = list;

                    await _iPHRepo.UpdatePharmacyHead(phead);
                }
                PharmacyHead head = new PharmacyHead()
                {
                    Name = pharmacyHead.Name,
                    Email = pharmacyHead.Email,
                    Password = pharmacyHead.Password,
                    Pharmacies = pharmacyHead.Pharmacies,
                    Medicines = list
                };
                if (!phead.Equals(head))
                {
                    await _iPHRepo.UpdatePharmacyHead(head);
                }
                else throw new Exception("Cannot update pharmacy head since there was no changes.");
            }
            else throw new Exception("PharmacyHead has a null value.");
        }
        public async Task<bool> Add(PharmacyHeadDto pharmacyHead)
        {
            if (pharmacyHead != null)
            {
                PharmacyHead head = new PharmacyHead()
                {
                    Name = pharmacyHead.Name,
                    Email = pharmacyHead.Email,
                    Password = pharmacyHead.Password,
                    Pharmacies = null,
                    Medicines = null
                };
                await _iPHRepo.Add(head);
                return true;
            }
            return false;
        }

        public async Task<bool> Remove(int id)
        {
            PharmacyHead Phead = await _iPHRepo.GetPharmacyHeadByIdAsync(id);
            if (Phead != null && id >= 0)
            {
                Phead.DeletedOn = DateTime.UtcNow;
                await _iPHRepo.Remove(Phead);
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveClaimingRequest(int id)
        {
            if (id >= 0)
            {
                await _iPHRepo.RemoveClaimingRequest(id);
                return true;
            }
            return false;
        }

        public PharmacyHeadDto GetPharmacyHead(string userName)
        {
            if (userName != null)
            {
                var Phead = _iPHRepo.GetPharmacyHeadByUserName(userName);
                List<PharmacyHeadMedicine> PHMedicines = _iPHRepo.GetPharmacyHeadMedicines(userName);
                List<Medicine> Medicines = _repository.GetMedicines().ToList();
                List<Medicine> MedicineList = new List<Medicine>();

                var user = _repository.GetRole(userName);


                if (user.UserRole.ToString().Equals("Admin"))
                {
                    List<Pharmacy> pharmacies = new List<Pharmacy>();
                    pharmacies = Phead.Pharmacies;
                    var Admin = new PharmacyHeadDto()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Name = user.Name,
                        Password = user.Password,
                        Pharmacies = pharmacies
                    };

                    return Admin;
                }
                else
                {
                    foreach (var med in Medicines)
                    {
                        var PHMObj = Phead.Medicines.Where(x => x.MedicineId == med.Id).SingleOrDefault();
                        if (PHMObj == null)
                        {
                            continue;
                        }
                        if (PHMObj.MedicineId == med.Id)
                        {
                            var medicine = new Medicine()
                            {
                                Id = med.Id,
                                Name = med.Name,
                                Strength = med.Strength,
                                Form = med.Form,
                                WayOfIssuing = med.WayOfIssuing,
                                Manufacturer = med.Manufacturer,
                                Price = med.Price,
                                Packaging = med.Packaging
                            };
                            MedicineList.Add(medicine);
                        }
                    }
                }

                PharmacyHeadDto pharmacyHead = new PharmacyHeadDto()
                {
                    Id = Phead.Id,
                    Medicines = MedicineList,
                    Pharmacies = Phead.Pharmacies,
                    Email = Phead.Email,
                    Name = Phead.Name,
                    Password = Phead.Password
                };
                return pharmacyHead;
            }
            else throw new Exception("Username is null.");
        }
    }
}
