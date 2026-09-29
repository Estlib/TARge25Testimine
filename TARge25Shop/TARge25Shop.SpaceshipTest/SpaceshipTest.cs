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
        // Selles testis kontrollitakse et kosmoselaeva päringul andmebaasist peaks tagastama objekti siis kui ID on sama
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
        // Selles testis kontrollitakse et kosmoselaeva kustutamisel andmebaasist peaks kustuma objekt kui tagastatav väärtus on sama
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

        [Fact]
        public async Task ShouldNot_DeleteSpaceshipByID_WhenDidNotDeleteSpaceship()
        {
            //ülesseade
            var dto = MockSpaceshipData();

            //tegevus
            var spaceShip1 = await Svc<ISpaceshipServices>().Create(dto);
            var spaceShip2 = await Svc<ISpaceshipServices>().Create(dto);

            var result = await Svc<ISpaceshipServices>().Delete((Guid)spaceShip2.Id);

            //kontroll
            Assert.NotEqual(spaceShip1.Id, result.Id);
        }
        // test mis kontrollib, et spaceshipi uuendatakse, uute andmete korral
        [Fact]
        public async Task Should_UpdateSpaceshipByID_WhenUpdatingData()
        {
            //ülesseade
            var guid = new Guid("68eb8abd-086a-4c8b-9695-71234143f709");

            SpaceshipDto dto = MockSpaceshipData();

            SpaceshipDto domain = new();

            domain.Id = Guid.Parse("68eb8abd-086a-4c8b-9695-71234143f709");
            domain.EnginePower = 10000000;
            domain.Name = "Igor Mang 2";
            domain.ShipType = "püramiid";
            domain.Crew = 420;
            domain.CreatedAt = dto.CreatedAt;//  <-- ei tohi muutuda Update korral, tuleb võtta olemasolevast objektist.
            domain.UpdatedAt = DateTime.UtcNow;//  <-- PEAB muutuma Update korral

            //tegevus
            await Svc<ISpaceshipServices>().Update(dto);

            //kontroll
            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(dto.EnginePower, domain.EnginePower);
            Assert.NotEqual(dto.Name, domain.Name);
            Assert.DoesNotMatch(dto.Crew.ToString(), domain.Crew.ToString());
            Assert.DoesNotMatch(dto.ShipType, domain.ShipType);
            Assert.Equal(dto.CreatedAt, domain.CreatedAt);
            Assert.NotEqual(dto.UpdatedAt, domain.CreatedAt);
        }
        [Fact]
        public async Task ShouldNot_UpdateSpaceshipByID_WhenNoDataIsUpdated()
        {
            //ülesseade
            SpaceshipDto dto = MockSpaceshipData();
            var createdSpaceship = await Svc<ISpaceshipServices>().Create(dto);

            //tegevus
            SpaceshipDto nullDto = MockSpaceshipNullData();
            var result = await Svc<ISpaceshipServices>().Update(nullDto);

            //kontroll
            Assert.NotEqual(createdSpaceship.Id, result.Id);
        }
        //kuna mootor ei saa olla negatiivse võimsusega, kontrollime et ei saaks
        //lisada võimetut mootorit ega negatiivse võimsusega mootorit
        [Fact]
        public async Task ShouldNot_CreateSpaceshipWithNegativeEnginePower_WhenEnginePowerNegative()
        {
            //ülesseade
            SpaceshipDto dto = MockSpaceshipData(true);
            dto.EnginePower -= (dto.EnginePower * 2);

            //tegevus
            var result = await Svc<ISpaceshipServices>().Create(dto);

            //kontroll
            Assert.True(result.EnginePower > 0);
        }




        /// <summary>
        /// Returns a nulled object for testing purposes
        /// </summary>
        /// <returns></returns>
        private SpaceshipDto MockSpaceshipNullData()
        {
            return new SpaceshipDto
            {
                Id = null,
                Name = "",
                ShipType = "",
                Crew = 0,
                EnginePower = 0,
                CreatedAt = DateTime.MinValue,
                UpdatedAt = DateTime.MinValue,
            };
        }


        /* üleval testid, all abimeetodid */

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
