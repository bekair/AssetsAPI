using AssetsAPI.DataAccess;
using AssetsAPI.Helpers;
using AssetsAPI.Models;
using AssetsAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly AssetsDbContext _context;
        public AssetController(AssetsDbContext context)
        {
            _context = context;
        }

        [HttpGet("SaveAssets")]
        public async Task<bool> SaveAssets()
        {
            List<AssetCsvResponseModel> assetList = CsvHelper.ReadCsv("Csv\\assets.csv");
            if (assetList is null)
            {
                return false;
            }

            var assetService = new AssetService(_context);
            var result = await assetService.SaveAssetsFromFile(assetList);

            return true;
        }
    }
}
