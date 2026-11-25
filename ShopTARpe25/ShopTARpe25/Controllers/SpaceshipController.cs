using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Spaceship;
using static System.Net.Mime.MediaTypeNames;


namespace ShopTARpe25.Controllers
{
    public class SpaceshipController : Controller
    {
        private readonly ISpaceshipServices _spaceshipService;
        private readonly ShopTARpe25Context _context;
        private readonly IFileServices _fileService;
        //teha constructor et saaks kasutada teenust, mis on
        //defineeritud ISpaceshipServices liideses


        public SpaceshipController
            (
                ISpaceshipServices spaceshipService,
                ShopTARpe25Context context,
                IFileServices fileService
            )
        {
            _spaceshipService = spaceshipService;
            _context = context;
            _fileService = fileService;
        }


        public IActionResult Index()
        {
            //loome baheinstantsi domaini ja viewmodeli vahel
            var result = _context.Spaceships
                .Select(x => new SpaceshipIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Classification = x.Classification,
                    BuiltDate = x.BuiltDate,
                    Crew = x.Crew,
                    EnginePower = x.EnginePower
                });


            return View(result);
        }

        //kui kasutaja klikib "Create" nuppu, siis see meetod käivitatakse
        //tagastab kasutajale vormi, kuhu saab sisestada andmed
        [HttpGet]
        public IActionResult Create()
        {
            SpaceshipCreateUpdateViewModel vm = new();
            return View("CreateUpdate", vm);
        }

        //kui oled teinud vormi, siis see meetod käivitatakse
        //saadab andmed serverisse, kus need salvestatakse andmebaasi
        [HttpPost]
        public async Task<IActionResult> Create(SpaceshipCreateUpdateViewModel vm)
        {
            //luua vaheinstants, mis sisaldab andmeid, mis on saadud vormist
            //need andmed tuleb edasi saata dto-sse, mis on mõeldud andmebaasi salvestamiseks

            var dto = new SpaceshipDto
            {
                Id = vm.Id,
                Name = vm.Name,
                Classification = vm.Classification,
                BuiltDate = vm.BuiltDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                Files = vm.Files,
                FileToApiDtos = vm.Image
                    .Select(file => new FileToApiDto
                    {
                        Id = file.ImageId,
                        ExistingFilePath = file.FilePath,
                        SpaceshipId = file.SpaceshipId
                    }).ToArray()
            };

            //kutsuda teenuse meetodit, mis salvestab andmed andmebaasi
            var result = await _spaceshipService.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        //tuleb teha details meetod
        //kutsub välja interfaceist service meetodi

        [HttpGet]

        public async Task<IActionResult> Details(Guid Id)
        {
            //meetodi kutsumine interfaceist
            var spaceship = await _spaceshipService.DetailsAsync(Id);

            //veakäsitlus
            //suunab vaatele NotFound, kui andmeid ei ole
            if (spaceship == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.SpaceshipId == Id)
                .Select(y =>  new ImageViewModel
                {
                    FilePath =/* "/multipleFileUpload/" + */y.ExistingFilePath,
                    ImageId = y.Id
                }).ToArrayAsync();

            var vm = new SpaceshipDetailsViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Classification = spaceship.Classification;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.EnginePower = spaceship.EnginePower;
            vm.Crew = spaceship.Crew;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;
            vm.Images.AddRange(images);


            return View(vm);

            //tuleb teha viewmodel ja see siin välja kutsuda
            //ära map-ida vm ja domain 
        }

        [HttpGet]

        public async Task<IActionResult> Update(Guid id)
        {
            var spaceship = await _spaceshipService.DetailsAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.SpaceshipId == id)
                .Select(y => new ImageViewModel
                {
                    FilePath =/* "/multipleFileUpload/" + */y.ExistingFilePath,
                    ImageId = y.Id
                }).ToArrayAsync();

            var vm = new SpaceshipCreateUpdateViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Classification = spaceship.Classification;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.EnginePower = spaceship.EnginePower;
            vm.Crew = spaceship.Crew;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;
            vm.Image.AddRange(images);


            return View("CreateUpdate", vm);
        }

        [HttpPost]

        public async Task<IActionResult> Update(SpaceshipCreateUpdateViewModel vm)
        {

            var dto = new SpaceshipDto
            {
                Id = vm.Id,
                Name = vm.Name,
                Classification = vm.Classification,
                BuiltDate = vm.BuiltDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                Files = vm.Files,
                FileToApiDtos = vm.Image
                    .Select(file => new FileToApiDto
                    {
                        Id = file.ImageId,
                        ExistingFilePath = file.FilePath,
                        SpaceshipId = file.SpaceshipId
                    }).ToArray()
            };

            var result = await _spaceshipService.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]

        public async Task<IActionResult> Delete(Guid Id)
        {
            var spaceship = await _spaceshipService.DetailsAsync(Id);
            if (spaceship == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.SpaceshipId == Id)
                .Select(y => new ImageViewModel
                {
                    FilePath =/* "/multipleFileUpload/" + */y.ExistingFilePath,
                    ImageId = y.Id
                }).ToArrayAsync();


            var vm = new SpaceshipDeleteViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Classification = spaceship.Classification;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;
            vm.Images.AddRange(images);


            return View(vm);

        }

        [HttpPost]

        public async Task<IActionResult> DeleteConfirmation(Guid Id)
        {
            var result = await _spaceshipService.Delete(Id);

                if (result == null)
            {
                return RedirectToAction(nameof(Index));

            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(ImageViewModel vm)
        {
            //tuleb ühendada dto ja viewmodel
            var dto = new FileToApiDto()
            {
                Id = vm.ImageId
            };

            //kutsuda tenuse meetodit, mis kustutab pildi andmebaasist
            var image = await _fileService.RemoveImagesFromDatabase(dto);
            if (image == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

