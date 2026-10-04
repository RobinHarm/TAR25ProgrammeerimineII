using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Kindergarten;

namespace ShopTARpe25.Controllers
{
    public class KindergartenController : Controller
    {
        private readonly IKindergartenServices _kindergartenService;
        private readonly ShopTARpe25Context _context;

        public KindergartenController
            (
            IKindergartenServices kindergartenService,
            ShopTARpe25Context context
            )
        {
            _kindergartenService = kindergartenService;
            _context = context;
        }


        public IActionResult Index()
        {
            var result = _context.Kindergartens
                .Select(x => new KindergartenIndexViewModel
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    KinderGartenName = x.KinderGartenName,
                    TeacherName = x.TeacherName,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                });
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(KindergartenCreateViewModel vm)
        {
            var dto = new KindergartenDto
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KinderGartenName = vm.KinderGartenName,
                TeacherName = vm.TeacherName,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt
            };
            var result = await _kindergartenService.Create(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var children = await _kindergartenService.DetailsAsync(id);

            if (children == null)
            {
                return NotFound();
            }

            var vm = new KindergartenDetailsViewModel();

            vm.Id = children.Id;
            vm.GroupName = children.GroupName;
            vm.ChildrenCount = children.ChildrenCount;
            vm.KinderGartenName = children.KinderGartenName;
            vm.TeacherName = children.TeacherName;
            vm.CreatedAt = children.CreatedAt;
            vm.UpdatedAt = children.UpdatedAt;
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid Id)
        {
            var children = await _kindergartenService.DetailsAsync(Id);

            if (children == null)
            {
                return NotFound();
            }

            var vm = new KindergartenUpdateViewModel();

            vm.Id = children.Id;
            vm.GroupName = children.GroupName;
            vm.ChildrenCount = children.ChildrenCount;
            vm.KinderGartenName = children.KinderGartenName;
            vm.TeacherName = children.TeacherName;
            vm.CreatedAt = children.CreatedAt;
            vm.UpdatedAt = children.UpdatedAt;

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(KindergartenUpdateViewModel vm)
        {
            var dto = new KindergartenDto
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KinderGartenName = vm.KinderGartenName,
                TeacherName = vm.TeacherName,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt
            };
            var result = await _kindergartenService.Update(dto);
            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var children = await _kindergartenService.DetailsAsync(id);

            if (children == null)
            {
                return NotFound();
            }

            var vm = new KindergartenDeleteViewModel();

            vm.Id = children.Id;
            vm.GroupName = children.GroupName;
            vm.ChildrenCount = children.ChildrenCount;
            vm.KinderGartenName = children.KinderGartenName;
            vm.TeacherName = children.TeacherName;
            vm.CreatedAt = children.CreatedAt;
            vm.UpdatedAt = children.UpdatedAt;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var result = await _kindergartenService.Delete(id);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
