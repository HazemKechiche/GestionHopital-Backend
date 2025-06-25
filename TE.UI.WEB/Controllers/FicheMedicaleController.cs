using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TE.Core.Domain;
using TE.Core.Services;

namespace TE.UI.WEB.Controllers
{
    [ApiController]
    [Route("api/fiches")]
    public class FicheMedicaleController : ControllerBase
    {
     private readonly FicheMedicaleService _ficheMedicaleService;
        public FicheMedicaleController(FicheMedicaleService ficheMedicaleService)
        {
            _ficheMedicaleService = ficheMedicaleService;
        }
        [HttpGet("getall")]
        public IList<FicheMedical> Index() { return _ficheMedicaleService.getAllFiches(); }

        [HttpPost("addfiche")]
        public FicheMedical AddFiche([FromBody] FicheMedical fiche) { return _ficheMedicaleService.addFiche(fiche); }
    }
}
