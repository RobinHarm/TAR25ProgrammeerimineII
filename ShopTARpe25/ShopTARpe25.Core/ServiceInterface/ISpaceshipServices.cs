using ShopTARpe25.Core.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using ShopTARpe25.Core.Domain;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface ISpaceshipServices
    {

        Task<Spaceship> Create(SpaceshipDto dto);
    }
}
