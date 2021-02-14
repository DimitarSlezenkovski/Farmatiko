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

                var phmeds = await _repository.GetAllPHMedicines();

                List<Medicine> medicines = _repository.GetMedicines().ToList();

                List<Medicine> PHMedicines = new List<Medicine>();

                List<PharmacyHeadMedicine> list = new List<PharmacyHeadMedicine>();


                if (pharmacyHead.Medicines != null && pharmacyHead.Medicines.Count() > 0)
                {
                    foreach (var med in phead.Medicines)
                    {
                        var medicine = medicines.Where(x => x.Id == med.MedicineId).FirstOrDefault();
                        if (medicine != null)
                            PHMedicines.Add(medicine);
                    }
                    foreach(var phMed in PHMedicines)
                    {
                        if (!pharmacyHead.Medicines.Contains(phMed))
                        {
                            /*
                             * USELESS
                             * if (pharmacyHead.Medicines.Count() == 0)
                            {
                                phead.Medicines = null;
                                int PHMId = phead.Medicines.Select(x => x.Id).Single();
                                int phId = phead.Medicines.Select(x => x.PheadId).Single();
                                int medId = phead.Medicines.Select(x => x.MedicineId).Single();
                                _iPHRepo.DeletePHMedicine(PHMId, phId, medId);
                                return;
                            }*/
                            if (phead.Medicines != null && phead.Medicines.Count() > 0)
                            {
                                foreach (var med in pharmacyHead.Medicines)
                                {
                                    Medicine medicine = new Medicine()
                                    {
                                        Name = med.Name,
                                        Form = med.Form,
                                        Manufacturer = med.Manufacturer,
                                        Medicines = med.Medicines,
                                        Packaging = med.Packaging,
                                        Price = med.Price,
                                        Strength = med.Strength,
                                        WayOfIssuing = med.WayOfIssuing
                                    };

                                    PharmacyHeadMedicine phm = new PharmacyHeadMedicine()
                                    {
                                        PheadId = phead.Id,
                                        Head = phead,
                                        MedicineId = med.Id,
                                        Medicine = medicine
                                    };

                                    bool ifExists = phead.Medicines.Contains(phm);
                                    if (!ifExists)
                                        list.Add(phm);

                                }
                            }
                            else
                            {
                                foreach (var med in pharmacyHead.Medicines)
                                {
                                    PharmacyHead head1 = new PharmacyHead()
                                    {
                                        Id = pharmacyHead.Id,
                                        Name = pharmacyHead.Name,
                                        Email = pharmacyHead.Email,
                                        Password = pharmacyHead.Password
                                    };
                                    PharmacyHeadMedicine phmed = new PharmacyHeadMedicine()
                                    {
                                        Head = head1,
                                        Medicine = med
                                    };
                                    list.Add(phmed);
                                }
                            }


                            phead.Medicines = list;

                            await _iPHRepo.UpdatePharmacyHead(phead);

                        }
                    }
                    
                }
                PharmacyHead head = new PharmacyHead()
                {
                    Name = pharmacyHead.Name,
                    Email = pharmacyHead.Email,
                    Password = pharmacyHead.Password,
                    Medicines = phead.Medicines,
                    Pharmacies = phead.Pharmacies
                };
                if (!phead.Name.Equals(head.Name) && !phead.Password.Equals(head.Email))
                {
                    await _iPHRepo.UpdatePharmacyHead(head);
                }
                List<Pharmacy> pharmacies = new List<Pharmacy>();
                pharmacies = phead.Pharmacies;
                if (head.Pharmacies != null && pharmacyHead.Pharmacies != null)
                {
                    if (head.Pharmacies.Count() > 0 && pharmacyHead.Pharmacies.Count() > 0)
                    {
                        foreach (var pharmacy in pharmacyHead.Pharmacies)
                        {
                            if (!head.Pharmacies.Contains(pharmacy))
                            {
                                pharmacy.PheadId = phead.Id;
                                pharmacy.PharmacyHead = phead;
                                pharmacies.Add(pharmacy);
                            }
                        }
                        head.Pharmacies = pharmacies;
                        await _iPHRepo.UpdatePharmacyHead(head);
                    }
                }
            }
            else throw new Exception("Cannot update pharmacy head since there was no changes.");
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
                        if (Phead.Medicines == null || Phead.Medicines.Count() == 0)
                            break;
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
