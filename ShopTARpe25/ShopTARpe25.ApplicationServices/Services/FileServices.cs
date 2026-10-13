using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using Microsoft.Extensions.Hosting;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Domain;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly ShopTARpe25Context _context;
        private readonly IHostEnvironment _webHost;

        public FileServices
           (
               ShopTARpe25Context context,
               IHostEnvironment webHost
           )
        {
            _context = context;
            _webHost = webHost;
        }

        public void FilesToApi(SpaceshipDto dto, Spaceship domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                if (!Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"))
                {
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");
                }
            }
        }
    }
}
