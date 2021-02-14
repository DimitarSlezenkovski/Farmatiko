using FarmatikoData.FarmatikoRepoInterfaces;
using FarmatikoData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmatikoData.FarmatikoRepo
{
    public class AdminRepo : IAdminRepo
    {
        private readonly FarmatikoDataContext _context;
        public AdminRepo(FarmatikoDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RequestPharmacyHead>> GetClaimingRequests()
        {
            var reqs = await _context.PHRequests.Select(x => new RequestPharmacyHead 
            {
                Head = x.Head,
                Pharmacy = x.Pharmacy
            }).OrderBy(x => x.Head.Name).ToListAsync();
            return reqs;
        }

        //GET
        public async Task<IEnumerable<PharmacyHead>> GetPharmacyHeads()
        {
            var PHeads = await _context.PharmacyHeads
                .Include(x => x.Medicines)
                .Include(x => x.Pharmacies)
                .OrderBy(x => x.Name)
                .ToListAsync();
            return PHeads;
        }
        //POST
        public void RemoveClaimRequest(RequestPharmacyHead request)
        {
            var req = _context.PHRequests.Select(x => new RequestPharmacyHead { Head = x.Head, Pharmacy = x.Pharmacy, Id = x.Id})
                .Where(x => x.Head.Email.Equals(request.Head.Email)).FirstOrDefault();
            _context.PHRequests.Remove(req);
            _context.SaveChanges();
        }
    }
}
