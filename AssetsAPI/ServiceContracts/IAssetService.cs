using AssetsAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetsAPI.ServiceContracts
{
    public interface IAssetService
    {
        Task<bool> SaveAssetsFromFile(List<AssetCsvResponseModel> assetCsvList);
        ICollection<long> GetAssetIdList(AssetIdRequestModel assetIdRequestModel);
        bool UpdateAsset(AssetUpdateModel assetUpdateModel);
        bool AddDefaultListToDb();
    }
}
