using FarmatikoData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmatikoData.FarmatikoRepoInterfaces
{
    public interface IAdminRepo
    {
        Task<IEnumerable<RequestPharmacyHead>> GetClaimingRequests();
        Task<IEnumerable<PharmacyHead>> GetPharmacyHeads();
        void RemoveClaimRequest(RequestPharmacyHead Id);
    }
}
