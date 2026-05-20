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
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CaseCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CaseCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var dto = new CaseDTO
            {
                CaseID = vm.CaseID,
                UserID = vm.UserID,
                Description = vm.Description,
                CurrentCaseStatus = vm.CurrentCaseStatus
            };

            await _caseService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var dto = await _caseService.GetCaseByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = new CaseUpdateViewModel
            {
                CaseID = dto.CaseID,
                UserID = dto.UserID,
                Description = dto.Description,
                CurrentCaseStatus = dto.CurrentCaseStatus
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CaseUpdateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var dto = new CaseDTO
            {
                CaseID = vm.CaseID,
                UserID = vm.UserID,
                Description = vm.Description,
                CurrentCaseStatus = vm.CurrentCaseStatus
            };

            var result = await _caseService.UpdateAsync(dto);
            if (result == null) return NotFound();

            return RedirectToAction(nameof(Index));
        }

    }
}