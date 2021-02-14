using System.Collections.Generic;
using System.Threading.Tasks;
using FarmatikoData.DTOs;
using FarmatikoData.Models;
using FarmatikoServices.FarmatikoServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Farmatiko.Controllers
{
    [ApiController]
    [Authorize(Roles = "PharmacyHead,Admin")]
    public class PharmacyHeadController : Controller
    {
        private readonly IPHService _PHService;
        public PharmacyHeadController(IPHService PHService)
        {
            _PHService = PHService;
        }

        //GET
        
        [HttpGet]
        [Route("api/pharmacyhead/{Id}")]
        public async Task<PharmacyHead> GetPharmacyHeadById([FromRoute] int Id)
        {
            var Phead = await _PHService.GetPharmacyHeadByIdAsync(Id);
            return Phead;
        }
        //POST
        
        [HttpPost]
        [Route("api/pharmacyhead/update")]
        public async Task<IActionResult> UpdatePharmacyHead([FromBody] PharmacyHeadDto pharmacyHead)
        {
            await _PHService.UpdatePharmacyHead(pharmacyHead);
            return Ok();
        }
        [HttpPost]
        [Route("api/pharmacyhead/requests")]
        public async Task<IActionResult> ClaimPharmacy([FromBody] RequestPharmacyHead pharmacy)
        {
            bool Success = await _PHService.ClaimPharmacy(pharmacy);
            return Ok(Success);
        }
       /* [HttpDelete]
        [Route("api/pharmacyhead/delete/{Id}")]
        public async Task<IActionResult> Remove([FromRoute] int Id)
        {
            bool Success = await _PHService.Remove(Id);
            return Ok(Success);
        }*/
       /* [HttpPost]
        [Route("api/pharmacyhead/requests/{Id}")]
        public async Task<IActionResult> RemoveClaimingRequest([FromRoute] int Id)
        {
            bool Success = await _PHService.RemoveClaimingRequest(Id);
            return Ok(Success);
        }*/

    }
}
