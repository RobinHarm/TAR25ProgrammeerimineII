using System;
using System.Collections.Generic;
using System.Text;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Domain;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface IKindergartenServices
    {
        Task<Kindergarten> Create(KindergartenDto dto);
        Task<Kindergarten> DetailsAsync(Guid id);
        Task<Kindergarten> Update(KindergartenDto dto);
        Task<Kindergarten> Delete(Guid id);
    }
}
