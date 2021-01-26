using FarmatikoData.DTOs;
using FarmatikoData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmatikoServices.FarmatikoServiceInterfaces
{
    public interface IPHService
    {
        Task<IEnumerable<PharmacyHead>> GetPharmacyHeadInfo();
        Task UpdatePharmacyHead(PharmacyHeadDto pharmacyHead);
        Task<int> Login(PharmacyHead pharmacyHead);
        Task<bool> ClaimPharmacy(RequestPharmacyHead pharmacy);
        Task<PharmacyHead> GetPharmacyHeadByIdAsync(int id);
        Task<bool> Add(PharmacyHeadDto pharmacyHead);
        Task<bool> Remove(int id);
        Task<bool> RemoveClaimingRequest(int id);
        PharmacyHeadDto GetPharmacyHead(string userName);
    }
}
