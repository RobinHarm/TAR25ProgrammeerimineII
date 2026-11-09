using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Kindergarten;
using ShopTARpe25.Models.Spaceship;

namespace ShopTARpe25.Controllers
{
    public class KindergartenController : Controller
    {
        private readonly IKindergartenServices _kindergartenService;
        private readonly ShopTARpe25Context _context;
        private readonly IFileServices _fileService;

        public KindergartenController
            (
            IKindergartenServices kindergartenService,
            ShopTARpe25Context context,
            IFileServices fileService
            )
        {
            _kindergartenService = kindergartenService;
            _context = context;
            _fileService = fileService;
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
            KindergartenCreateUpdateViewModel vm = new();

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(KindergartenCreateUpdateViewModel vm)
        {
            var dto = new KindergartenDto
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KinderGartenName = vm.KinderGartenName,
                TeacherName = vm.TeacherName,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt,
                Files = vm.Files,
                FileToApiDtos = vm.Images
                .Select(file => new FileToApiDto
                {
                    Id = file.ImageId,
                    ExistingFilePath = file.FilePath,
                    KindergartenId = file.KindergartenId
                }).ToArray()
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

            var images = await _context.FileToApis
                .Where(x => x.KindergartenId == id)
                .Select(y => new KindergartenImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id,
                    KindergartenId = y.KindergartenId
                }).ToArrayAsync();

            var vm = new KindergartenDetailsViewModel();

            vm.Id = children.Id;
            vm.GroupName = children.GroupName;
            vm.ChildrenCount = children.ChildrenCount;
            vm.KinderGartenName = children.KinderGartenName;
            vm.TeacherName = children.TeacherName;
            vm.CreatedAt = children.CreatedAt;
            vm.UpdatedAt = children.UpdatedAt;
            vm.Images.AddRange(images);

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

            var images = await _context.FileToApis
               .Where(x => x.KindergartenId == Id)
               .Select(y => new KindergartenImageViewModel
               {
                   FilePath = y.ExistingFilePath,
                   ImageId = y.Id,
                   KindergartenId = y.KindergartenId
               }).ToArrayAsync();

            var vm = new KindergartenCreateUpdateViewModel();

            vm.Id = children.Id;
            vm.GroupName = children.GroupName;
            vm.ChildrenCount = children.ChildrenCount;
            vm.KinderGartenName = children.KinderGartenName;
            vm.TeacherName = children.TeacherName;
            vm.CreatedAt = children.CreatedAt;
            vm.UpdatedAt = children.UpdatedAt;
            vm.Images.AddRange(images);

            return View("CreateUpdate", vm);
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
                UpdatedAt = vm.UpdatedAt,
                Files = vm.Files,
                FileToApiDtos = vm.Images
                .Select(file => new FileToApiDto
                {
                    Id = file.ImageId,
                    ExistingFilePath = file.FilePath,
                    KindergartenId = file.KindergartenId
                }).ToArray()
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

            var images = await _context.FileToApis
               .Where(x => x.KindergartenId == id)
               .Select(y => new KindergartenImageViewModel
               {
                   FilePath = y.ExistingFilePath,
                   ImageId = y.Id,
                   KindergartenId = y.KindergartenId
               }).ToArrayAsync();

            var vm = new KindergartenDeleteViewModel();

            vm.Id = children.Id;
            vm.GroupName = children.GroupName;
            vm.ChildrenCount = children.ChildrenCount;
            vm.KinderGartenName = children.KinderGartenName;
            vm.TeacherName = children.TeacherName;
            vm.CreatedAt = children.CreatedAt;
            vm.UpdatedAt = children.UpdatedAt;
            vm.Images.AddRange(images);

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

        [HttpPost]
        public async Task<IActionResult> RemoveImage(KindergartenImageViewModel vm)
        {
            var dto = new FileToApiDto()
            {
                Id = vm.ImageId
            };

            var image = await _fileService.RemoveImageFromApi(dto);

            if (image == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
