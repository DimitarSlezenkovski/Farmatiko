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
            var reqs = await _context.PHRequests.OrderBy(x => x.Head.Name).ToListAsync();
            return reqs;
        }

        //GET
        public async Task<IEnumerable<PharmacyHead>> GetPharmacyHeads()
        {
            var PHeads = await _context.PharmacyHeads.OrderBy(x => x.Name).ToListAsync();
            return PHeads;
        }
        //POST
        public void RemoveClaimRequest(int Id)
        {
            var req = _context.PHRequests.Where(x => x.Id == Id).FirstOrDefault();
            _context.PHRequests.Remove(req);
            _context.SaveChanges();
        }
    }
}
