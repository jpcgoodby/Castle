using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Castle.Data
{
    public class CastleSeed
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new CastleContext(
                serviceProvider.GetRequiredService<DbContextOptions<CastleContext>>());

            if (context.Castles.Any())
                return;

            context.Castles.AddRange(
                new Castle
                {
                    Id = 1,
                    Name = "Bodiam Castle",
                    FileName = "300px - Bodiam - castle - 10My8 - 1197.jpg",
                    Location = "East Sussex",
                    Country = "England",
                    DateBuilt = 1385
                },
                new Castle
                {
                    Id = 2,
                    Name = "Alcázar of Segovia",
                    FileName = "300px-Panorámica_Otoño_Alcázar_de_Segovia.jpg",
                    Location = "Segovia",
                    Country = "Spain",
                    DateBuilt = 1120
                },
                new Castle
                {
                    Id = 3,
                    Name = "Tower of London",
                    FileName = "Tower_of_London_viewed_from_the_River_Thames.jpg",
                    Location = "London",
                    Country = "England",
                    DateBuilt = 1066
                },
                new Castle
                {
                    Id = 4,
                    Name = "Baba Vida",
                    FileName = "Baba_Vida_Klearchos_1.jpg,Vidin",
                    Location = "Vidin",
                    Country = "Bulgaria",
                    DateBuilt = 950
                },
                new Castle
                {
                    Id = 5,
                    Name = "São Jorge Castle",
                    FileName = "Castelo_de_São_Jorge_(Lissabon_2009).jpg",
                    Location = "Lisbon",
                    Country = "Portugal",
                    DateBuilt = 50
                },
                new Castle
                {
                    Id = 6,
                    Name = "Windsor Castle",
                    FileName = "220px - Windsor_Castle_at_Sunset_ - _Nov_2006.jpg",
                    Location = "Windsor",
                    Country = "London",
                    DateBuilt = 1070
                });

            context.SaveChanges();
        }
    }
}
