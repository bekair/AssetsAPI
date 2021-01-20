using AssetsAPI.DataAccess;
using AssetsAPI.Entities;
using AssetsAPI.Models;
using AssetsAPI.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace AssetsAPI.Services
{
    public class AssetService : IAssetService
    {
        private readonly AssetsDbContext _context;
        public AssetService(AssetsDbContext context)
        {
            _context = context;
        }

        public bool UpdateAsset([NotNull] AssetUpdateModel assetUpdateModel)
        {
            var updateAsset = new Asset
            {
                AssetId = assetUpdateModel.AssetId,
                Properties = assetUpdateModel.Properties,
                Timestamp = assetUpdateModel.Timestamp,
                Value = assetUpdateModel.Value
            };

            var isSuitable = CheckAssetCanBeAdded(updateAsset);
            if (isSuitable)
            {
                _context.SaveChanges();
                _context.Add(updateAsset);

                return true;
            }

            Console.WriteLine("Asset cannot be updated because of its Timestamp value.");
            return false;
        }

        public ICollection<long> GetAssetIdList(AssetIdRequestModel assetIdRequestModel)
        {
            return _context.Assets.Where(a =>
                                a.Properties == assetIdRequestModel.Property &&
                                a.Value == assetIdRequestModel.Value
                           ).Select(a => a.AssetId)
                            .ToList();
        }

        public async Task<bool> SaveAssetsFromFile(List<AssetCsvResponseModel> assetCsvList)
        {
            //Check the duplicates with the same AssetId and Properties and get the highest Timestamp valued one into list
            var refinedList = RemoveOrderedDuplicatesWithEarlierTimestamps(assetCsvList);

            List<Asset> newAssetList = new List<Asset>();
            foreach (var asset in refinedList)
            {
                var newAsset = new Asset
                {
                    AssetId = asset.AssetId,
                    Properties = asset.Properties,
                    Timestamp = asset.Timestamp,
                    Value = asset.Value
                };

                var isSuitable = CheckAssetCanBeAdded(newAsset);
                if (isSuitable)
                {
                    newAssetList.Add(newAsset);
                }
            }

            await _context.Assets.AddRangeAsync(newAssetList);
            await _context.SaveChangesAsync();

            return true;
        }

        private bool CheckAssetCanBeAdded(Asset asset)
        {
            if (!IsAssetExistInDb(asset))
            {
                Console.WriteLine($"Asset with the AssetId: {asset.AssetId} could not be found in Db.");

                return false;
            }

            var assetTaken = _context.Assets.Where(a => a.Id == asset.Id && a.Properties == asset.Properties);

            return !assetTaken.Any(a => a.Timestamp.CompareTo(asset.Timestamp) >= 0);
        }

        private bool IsAssetExistInDb(Asset asset)
        {
            return _context.Assets.Where(a => a.Id == asset.Id && a.Properties == asset.Properties)
                           .Any();
        }

        private ICollection<AssetCsvResponseModel> RemoveOrderedDuplicatesWithEarlierTimestamps(List<AssetCsvResponseModel> assetCsvList)
        {
            var orderedAssetList = assetCsvList.GroupBy(x => x)
                                               .OrderByDescending(x => (x.Key.AssetId, x.Key.Properties, x.Key.Timestamp))
                                               .Select(x => x.Key)
                                               .ToList();

            var refinedAssetList = new List<AssetCsvResponseModel>();
            foreach (var nextAsset in orderedAssetList)
            {
                if (!refinedAssetList.Any())
                {
                    refinedAssetList.Add(nextAsset);
                    continue;
                }

                var lastAddedAssetOfRefinedList = refinedAssetList[^1];
                //Same asset with same property should not be added under favour of our ordered list
                if (!(lastAddedAssetOfRefinedList.AssetId == nextAsset.AssetId && lastAddedAssetOfRefinedList.Properties == nextAsset.Properties))
                {
                    refinedAssetList.Add(nextAsset);
                }
            }

            return refinedAssetList;
        }
    }
}
