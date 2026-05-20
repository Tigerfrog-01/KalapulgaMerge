using System;
using System.Linq;
using System.Threading.Tasks;
using KalapulgaMerge.Core.Dto;
using KalapulgaMerge.Core.ServiceInterface;
using KalapulgaMerge.Models.Case;
using Microsoft.AspNetCore.Mvc;

namespace KalapulgaMerge.Controllers
{
    public class CaseController : Controller
    {
        private readonly ICaseService _caseService;

        public CaseController(ICaseService caseService)
        {
            _caseService = caseService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dtos = await _caseService.GetCasesAsync();
            var vms = dtos.Select(x => new CaseIndexViewModel
            {
                CaseID = x.CaseID,
                UserID = x.UserID,
                Description = x.Description,
                CurrentCaseStatus = x.CurrentCaseStatus
            });

            return View(vms);
        }

    }
}