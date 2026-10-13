using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.Core.Domain
{
    internal class FileToApi
    {
        public Guid Id { get; set; }

        public string? ExistingFilePath { get; set; }
        public Guid? SpaceshipId { get; set; }
    }
}
