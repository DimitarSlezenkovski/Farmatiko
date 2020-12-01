using FarmatikoData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmatikoData.FarmatikoRepoInterfaces
{
    public interface IPHRepo
    {
        Task ClaimPharmacy(RequestPharmacyHead pharmacy);
        Task<IEnumerable<PharmacyHead>> GetPharmacyHeadInfo();
        Task UpdatePharmacyHead(PharmacyHead pharmacyHead);
        Task<PharmacyHead> GetPharmacyHeadByIdAsync(int id);
        Task Add(PharmacyHead pharmacyHead);
        Task Remove(PharmacyHead phead);
        Task RemoveClaimingRequest(int id);
        PharmacyHead GetPharmacyHeadByUserName(string userName);
        List<PharmacyHeadMedicine> GetPharmacyHeadMedicines(string email);
        IEnumerable<PharmacyHead> GetPharmacyHeads();
        PharmacyHead GetPharmacyHead(string head);
        List<Pharmacy> GetPharmacies();
        void DeletePHMedicine(int id, int phId, int medId);
    }
}
