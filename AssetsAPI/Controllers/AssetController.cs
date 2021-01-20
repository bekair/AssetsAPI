using AssetsAPI.DataAccess;
using AssetsAPI.Helpers;
using AssetsAPI.Models;
using AssetsAPI.ServiceContracts;
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
        private readonly IAssetService _assetService;
        public AssetController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        [HttpGet("Index")]
        public string Index()
        {
            return "API is running...";
        }

        [HttpGet("SaveAssets")]
        public async Task<bool> SaveAssets()
        {
            List<AssetCsvResponseModel> assetList = CsvHelper.ReadCsv("Csv\\assets.csv");
            if (assetList is null)
            {
                return false;
            }
           
            return await _assetService.SaveAssetsFromFile(assetList);
        }

        [HttpGet("GetAssetIdList")]
        public List<long> GetAssetIdList([FromBody]AssetIdRequestModel assetIdRequestModel)
        {
            return (List<long>)_assetService.GetAssetIdList(assetIdRequestModel);
        }

        [HttpGet("UpdateAsset")]
        public bool UpdateAsset([FromBody]AssetUpdateModel assetUpdateModel)
        {
            return _assetService.UpdateAsset(assetUpdateModel);
        }

    }
}
