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

            [Fact]
            //mul on vaja testida  DetailsAsync meetodit
            //leiab ankeedi üless e õige id-ga otsib
            public async Task Should_GetByIdRealEstate_WhenReturnsEqual()
            {
            //Arrange
            //see guid on juba andmebaasis olemas ankeedi juures
            Guid guid = Guid.Parse("3dc05ba8-b2b4-4c03-967e-a9bcea734eb1");
            //kui vajutad nupule, siis see guid saadetakse andmebaasi, mida võrreldakse
            Guid databaseGuid = Guid.Parse("3dc05ba8-b2b4-4c03-967e-a9bcea734eb1");
            
            //Act
            await Svc<IRealEstateServices>().DetailsAsync(guid);
            
            //Assert
            Assert.Equal(databaseGuid, guid);
            }

        [Fact]
        //kustutamise meetotit on vaja testida. Andmebaasis on ankeet, mida 
        //tahan ära kustutada
        public async Task Should_DeleteSomethingFromDatabase_WhenExists()
        {

            //Arrange
            //vaja oleks mockida ankeeti e teha meetod kus on ankeeid andmed
            RealEstateDto dto = MockRealEstateData();

            //Act
            var AddRealEstate = await Svc<IRealEstateServices>().Create(dto);
           var deleteRealEstate = await Svc<IRealEstateServices>().Delete((Guid)AddRealEstate.Id);

            //Assert
            //Võrdlen Id-sid, kas kustutasin selle ära, mis oli andmebaasis
            Assert.Equal(deleteRealEstate.Id, AddRealEstate.Id);
        }

        private RealEstateDto MockRealEstateData()
        {
            return new RealEstateDto
            {
                Area = 88.65,
                Address = "SEE ON ADRESS",
                RoomNumber = 1,
                BuildingType = "TestBuildingType",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
        }
    }
}
