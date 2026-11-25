using Azure.Core;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            // Arrange
            RealEstateDto dto = new()
            {
                Area = 120.5,
                Address = "Test location",
                RoomNumber = 3,
                BuildingType = "house",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow

            };

            //Act Siin tuleb kutsuda RealEstate Create meetod esile
            var result = await Svc<IRealEstateServices>().Create(dto);

            // Assert
            Assert.NotNull(result);

            //nüüd uurite kuidas testi käima panna


        }


        //teha test, kus otsib ankeeti id alusel, aga seda ei ole andmebaasis
        public class RealEstateTest2 : TestBase
        {
            [Fact]
            public async Task ShouldNot_FindRealEstate_WhenIdDoesNotExist()
            {
                // Arrange
                Guid databaseGuid = Guid.Parse("54cfecb3-1cf8-4ef4-ab28-7699de10b700");
                Guid wrongGuid = Guid.NewGuid();

                // Act
                await Svc<IRealEstateServices>().DetailsAsync(databaseGuid);

                // Assert
                //vastupidist loogikat tuleb kasutada
                Assert.NotEqual(wrongGuid, databaseGuid);
            }
        }
    }
}
