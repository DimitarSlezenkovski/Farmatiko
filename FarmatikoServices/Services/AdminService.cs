using FarmatikoData.FarmatikoRepoInterfaces;
using FarmatikoData.Models;
using FarmatikoServices.FarmatikoServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmatikoServices.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepo _adminRepo;
        public AdminService(IAdminRepo adminRepo)
        {
            _adminRepo = adminRepo;
        }

        public async Task<IEnumerable<RequestPharmacyHead>> GetClaimingRequests()
        {
            var req = await _adminRepo.GetClaimingRequests();
            if (req != null)
                return req;
            throw new Exception("No data is found.");
        }

        public async Task<IEnumerable<PharmacyHead>> GetPharmacyHeads()
        {
            var PHeads = await _adminRepo.GetPharmacyHeads();
            var list = PHeads.Select(x => x.DeletedOn == null);
            if (list != null)
            {
                return PHeads;
            }
            throw new Exception("No data is found.");
        }

        public bool RejectRequest(int Id)
        {
            if (Id >= 0)
            {
                _adminRepo.RemoveClaimRequest(Id);
                return true;
            }
            return false;
        }
    }
}
