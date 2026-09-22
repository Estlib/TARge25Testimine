using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using Xunit;

namespace TARge25Shop.SpaceshipTest
{
    public class SpaceshipTest : TestBase
    {

        [Fact] 
        // Fact tähistab ära ühte testi xUnit raamistikus
        // 1 - Kirjeldatakse ära kas test on tavaline, või negatiivne.
        // 2 - Kirjeldatakse ära mida parasjagu üritatakse testialuse objektiga teha
        // 3 - Mis tingimustel tulemust kontrollitakse, peale tegevust
        //
        // Selles testis kontrollitakse et (2) Kosmoselaeva lisamisel
        // (1) Ei tohiks (3) saadud tulemus olla tühi. Jälgi seda sõnastusviisi:
        //
        //                  1           2               3
        //                  \/          \/              \/
        public async Task ShouldNot_AddEmptySpaceShip_WhenResultIsReturned()
        {
            // Ülesseade
            SpaceshipDto dto = new SpaceshipDto() 
            { 
                Name = "X AE a L 12 menuornvöerv",
                ShipType = "lendav taldrik",
                Crew = 67,
                EnginePower = 69,//hobujõudu siis
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            // tegutsemine
            var result = await Svc<ISpaceshipServices>().Create(dto);

            // kontroll
            Assert.NotNull(result);
        }
        // Selles testis kontrollitakse et (2) Spaceshipi päring andmebaasist
        // (1) ei tohiks tagastada objekti (3) kui ID-d ei ole samad:
        //
        //                  1           2               3
        //                  \/          \/              \/
        [Fact]
        public async Task ShouldNot_GetSpaceShipByID_WhenIDNotEqual()
        {
            // ülesseade
            Guid wrongGuid = Guid.NewGuid();
            Guid goodGuid = Guid.Parse("ecbc059a-0bca-4df2-aae9-a3211e69185a");

            // tegevus
            await Svc<ISpaceshipServices>().DetailAsync(goodGuid);

            // kontroll
            Assert.NotEqual(wrongGuid, goodGuid);
        }
        // Seleta kodus lahti, nagu eelnevate testide laused, eesti keelde, selle testi oma ka.
        [Fact]
        public async Task Should_GetSpaceshipByID_WhenGuidIsEqual()
        {
            // ülessezade
            Guid databaseGuid = Guid.Parse("ecbc059a-0bca-4df2-aae9-a3211e69185a");
            Guid seekGuid = Guid.Parse("ecbc059a-0bca-4df2-aae9-a3211e69185a");

            // tegevus
            await Svc<ISpaceshipServices>().DetailAsync(seekGuid);

            // kontroll
            Assert.Equal(databaseGuid, seekGuid);
        }

        // Seleta kodus lahti, nagu eelnevate testide laused, eesti keelde, selle testi oma ka.
        [Fact]
        public async Task Should_SpaceshipDeletedByID_WhenReturnedResultIsEqual()
        {
            // ülesseade
            SpaceshipDto dto = MockSpaceshipData();

            // tegevus
            var addSpaceship = await Svc<ISpaceshipServices>().Create(dto);
            var deleteSpaceship = await Svc<ISpaceshipServices>().Delete((Guid)addSpaceship.Id);

            // kontroll
            Assert.Equal(addSpaceship.Id, deleteSpaceship.Id);
        }

        private SpaceshipDto MockSpaceshipData(bool isOneOrTwo = false)
        {
            if (isOneOrTwo == false)
            {
                return new SpaceshipDto
                {
                    Name = "X AE a L 12 menuornvöerv",
                    ShipType = "lendav taldrik",
                    Crew = 67,
                    EnginePower = 69,//hobujõudu siis
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
            }
            else
            {
                return new SpaceshipDto
                {
                    Name = "RAKETT69",
                    ShipType = "lendav kauss",
                    Crew = 420,
                    EnginePower = 999,//hobujõudu siis
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
            }
        }
    }
}
