using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;

namespace Caste.API.Services
{
    public class CastleService
    {
        private readonly CastleContext ctx;
        private readonly IConfiguration _configuration;


        public CastleService(CastleContext ctx, 
                             IConfiguration configuration)
        {
            this.ctx = ctx;
            _configuration = configuration;
        }

        public async Task<List<Castle.ViewModels.CastleResponse>> GetCastles()
        {
            var castles = new List<Castle.ViewModels.CastleResponse>();

            foreach (var castle in this.ctx.Castles.ToList()){
                castles.Add(new Castle.ViewModels.CastleResponse
                {
                    Name = castle.Name,
                    Location = castle.Location,
                    FileName = castle.FileName,
                    Country = castle.Country,
                    DateBuilt = castle.DateBuilt,
                    Url = _configuration["FileLocation"] + $"/castles/" + castle.FileName 
                });
            }

            return await Task.FromResult(castles);
        }

        public async Task<bool> createCastle(Castle.ViewModels.CastleRequest castle)
        {
            var newCastle = new Castle.Data.Castle
            {
                Name = castle.Name,
                Location = castle.Location,
                FileName = castle.FileName,
                Country = castle.Country,
                DateBuilt = castle.DateBuilt
            };

            this.ctx.Castles.Add(newCastle);

            return await Task.FromResult(this.ctx.SaveChanges() == 1) ;
        }

        public async Task<bool> CreateFile(IFormFile file)
        {

            var storageConnectionString = _configuration["ConnectionStrings:AzureStorageConnectionString"];

            if (CloudStorageAccount.TryParse(storageConnectionString, out CloudStorageAccount storageAccount))
            {
                var blobClient = storageAccount.CreateCloudBlobClient();

                var container = blobClient.GetContainerReference("castles");

                await container.CreateIfNotExistsAsync();

                var blob = container.GetBlockBlobReference(file.FileName);

                await blob.UploadFromStreamAsync(file.OpenReadStream());

                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }
    }
}
