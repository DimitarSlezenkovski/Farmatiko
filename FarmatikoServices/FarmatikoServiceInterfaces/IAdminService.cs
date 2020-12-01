using FarmatikoData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmatikoServices.FarmatikoServiceInterfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<PharmacyHead>> GetPharmacyHeads();
        Task<IEnumerable<RequestPharmacyHead>> GetClaimingRequests();
        bool RejectRequest(int Id);
    }
}
