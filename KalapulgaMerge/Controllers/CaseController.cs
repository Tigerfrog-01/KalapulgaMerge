using System;
using System.Linq;
using System.Threading.Tasks;
using KalapulgaMerge.Core.Domain;
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
        private bool IsAdmin()
        {
            bool isLoggedIn = !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("UserId"));
            bool isAdmin = HttpContext.Session.GetString("UserName") == "admin" && HttpContext.Session.GetString("UserEmail") == "admin@admin";

            return isLoggedIn && isAdmin;
        }
        private bool IsLoggedIn()
        {
            return !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("UserId"));
        }
        [HttpGet]
        public IActionResult OpenCase()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Accounts");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OpenCase(string description)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Accounts");

            if (string.IsNullOrWhiteSpace(description))
            {
                ModelState.AddModelError("", "Please provide a description of the problem.");
                return View();
            }

            if (!int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                return RedirectToAction("Login", "Accounts");
            }

            var dto = new CaseDTO
            {
                CaseID = Guid.NewGuid(),
                UserID = userId,
                Description = description,
                CurrentCaseStatus = (CaseStatus)2 // Forces status
            };

            await _caseService.CreateAsync(dto);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchCaseId, string searchUserId, int? searchStatus)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Accounts");

            var dtos = await _caseService.GetCasesAsync();

            ViewBag.AllCaseIds = dtos.Select(x => x.CaseID.ToString()).Distinct().ToList();
            ViewBag.AllUserIds = dtos.Select(x => x.UserID.ToString()).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();


            if (!string.IsNullOrWhiteSpace(searchCaseId))
            {
                dtos = dtos.Where(x => x.CaseID.ToString().Contains(searchCaseId, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(searchUserId))
            {
                dtos = dtos.Where(x => x.UserID != null && x.UserID.ToString().Contains(searchUserId, StringComparison.OrdinalIgnoreCase));
            }

            if (searchStatus.HasValue)
            {
                dtos = dtos.Where(x => (int)x.CurrentCaseStatus == searchStatus.Value);
            }
            var vms = dtos.Select(x => new CaseIndexViewModel
            {
                CaseID = x.CaseID,
                UserID = x.UserID,
                Description = x.Description,
                CurrentCaseStatus = x.CurrentCaseStatus



            });

            ViewBag.SearchCaseId = searchCaseId;
            ViewBag.SearchUserId = searchUserId;
            ViewBag.SearchStatus = searchStatus;

            return View(vms);
        }
        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Accounts");

            return View(new CaseCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CaseCreateViewModel vm)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Accounts");

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
            if (!IsAdmin()) return RedirectToAction("Login", "Accounts");

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
            if (!IsAdmin()) return RedirectToAction("Login", "Accounts");

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
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Accounts");

            var dto = await _caseService.GetCaseByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = new CaseDeleteViewModel
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
        public async Task<IActionResult> DeleteConfirmed(Guid caseID)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Accounts");

            await _caseService.DeleteAsync(caseID);
            return RedirectToAction(nameof(Index));
        }

    }
}