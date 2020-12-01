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



        public async Task UpdatePharmacyHead(PharmacyHead pharmacyHead)
        {
            if (pharmacyHead != null)
            {
                var phead = _iPHRepo.GetPharmacyHead(pharmacyHead.Email);

                /*if (pharmacyHead.PharmaciesList.Count() == 0)
                    pharmacyHead.PharmaciesList = null;*/


                phead.PHMedicineList = _repository.GetPHMedicines(phead.Email);

                if (phead.MedicineList != pharmacyHead.MedicineList)
                {
                    phead.MedicineList = pharmacyHead.MedicineList;
                    List<PharmacyHeadMedicine> list = new List<PharmacyHeadMedicine>();
                    if (pharmacyHead.MedicineList.Count() == 0)
                    {
                        phead.MedicineList = null;
                        int PHMId = phead.PHMedicineList.Select(x => x.Id).Single();
                        int phId = phead.PHMedicineList.Select(x => x.PheadId).Single();
                        int medId = phead.PHMedicineList.Select(x => x.MedicineId).Single();
                        _iPHRepo.DeletePHMedicine(PHMId, phId, medId);
                        return;
                    }
                    List<PharmacyHeadMedicine> PHMList = new List<PharmacyHeadMedicine>();
                    if (phead.PHMedicineList == pharmacyHead.PHMedicineList)
                    {
                        foreach (var med in phead.MedicineList)
                        {
                            //list = phead.PHMedicineList.ToList();

                            var PHMObj = phead.PHMedicineList.Select(x => new PharmacyHeadMedicine
                            {
                                Id = x.Id,
                                PheadId = x.PheadId,
                                Head = x.Head,
                                MedicineId = x.MedicineId,
                                Medicine = x.Medicine
                            }).Where(x => x.MedicineId == med.Id).Single();
                            if (PHMObj == null || PHMObj == default)
                                break;
                            if (PHMObj.MedicineId == med.Id)
                                list.Add(PHMObj);

                        }

                        phead.PHMedicineList = list;
                    }

                    await _iPHRepo.UpdatePharmacyHead(phead);
                }
                else if (!phead.Equals(pharmacyHead))
                {
                    await _iPHRepo.UpdatePharmacyHead(pharmacyHead);
                }
                else throw new Exception("Cannot update pharmacy head since there was no changes.");
            }
            else throw new Exception("PharmacyHead has a null value.");
        }
        public async Task<bool> Add(PharmacyHead pharmacyHead)
        {
            if (pharmacyHead != null)
            {
                await _iPHRepo.Add(pharmacyHead);
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

        public object GetPharmacyHead(string userName)
        {
            if (userName != null)
            {
                var Phead = _iPHRepo.GetPharmacyHeadByUserName(userName);
                List<PharmacyHeadMedicine> PHMedicines = _iPHRepo.GetPharmacyHeadMedicines(userName);
                List<Medicine> Medicines = _repository.GetMedicines().ToList();
                List<Medicine> PHMedicineList = new List<Medicine>();


                //var meds = PHMedicines.Where(x => x.Id == Phead.Id).ToList();
                var pharmacies = _iPHRepo.GetPharmacies();
                var PheadPharms = pharmacies.Where(x => x.PheadId == Phead.Id).ToList();
                var user = _repository.GetRole(userName);


                if (user.UserRole.ToString().Equals("Admin"))
                {
                    var Admin = new PharmacyHead()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Name = user.Name,
                        Password = user.Password,
                        PharmaciesList = Phead.PharmaciesList
                    };

                    return Admin;
                }

                if (PheadPharms.Count() > 0 || PheadPharms != null)
                    Phead.PharmaciesList = PheadPharms;
                else Phead.PharmaciesList = pharmacies;

                if (Phead.PHMedicineList.Count() > 0 || Phead.PHMedicineList != null)
                {
                    foreach (var med in Medicines)
                    {

                        var PHMObj = Phead.PHMedicineList.Where(x => x.MedicineId == med.Id).SingleOrDefault();
                        if (PHMObj == null)
                        {
                            continue;
                        }
                        var phm = Phead.MedicineList;
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
                            PHMedicineList.Add(medicine);
                        }
                    }
                    Phead.MedicineList = PHMedicineList;
                }
                else
                {
                    Phead.MedicineList = Medicines;
                }

                PharmacyHead pharHead = new PharmacyHead()
                {
                    Id = Phead.Id,
                    MedicineList = Phead.MedicineList,
                    PharmaciesList = Phead.PharmaciesList,
                    Email = Phead.Email,
                    Name = Phead.Name,
                    Password = Phead.Password
                };
                return pharHead;
            }
            else throw new Exception("Username is null.");
        }
    }
}
