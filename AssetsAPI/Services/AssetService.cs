using AssetsAPI.DataAccess;
using AssetsAPI.Entities;
using AssetsAPI.Enums;
using AssetsAPI.Helpers;
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

        public bool UpdateAsset(AssetUpdateModel assetUpdateModel)
        {
            var newerAsset = new Asset
            {
                AssetId = assetUpdateModel.AssetId,
                Properties = (PropertyEnum)EnumExtensions.GetEnumFromDisplayValue<PropertyEnum>(assetUpdateModel.Properties),
                Timestamp = assetUpdateModel.Timestamp,
                Value = assetUpdateModel.Value
            };

            var updatedAsset = GetAsset(newerAsset);
            if (updatedAsset == null)
            {
                Console.WriteLine($"Asset with the 'AssetId: {newerAsset.AssetId}, Properties: {newerAsset.Properties}' could not be found in Db.");

                return false;
            }

            var isSuitable = CheckAssetCanBeAdded(newerAsset);
            if (isSuitable)
            {
                updatedAsset.Properties = newerAsset.Properties;
                updatedAsset.Value = newerAsset.Value;
                updatedAsset.Timestamp = newerAsset.Timestamp;

                _context.SaveChanges();

                return true;
            }

            Console.WriteLine("Asset cannot be updated because of its Timestamp value.");
            return false;
        }

        public ICollection<long> GetAssetIdList(AssetIdRequestModel assetIdRequestModel)
        {
            return _context.Assets.Where(a =>
                                        a.Properties == (PropertyEnum)EnumExtensions.GetEnumFromDisplayValue<PropertyEnum>(assetIdRequestModel.Properties) &&
                                        a.Value == assetIdRequestModel.Value
                                  )
                                  .Select(a => a.AssetId)
                                  .ToList();
        }

        public async Task<bool> SaveAssetsFromFile(List<AssetCsvResponseModel> assetCsvList)
        {
            //Check the duplicates with the same AssetId and Properties and get the highest Timestamp valued one into list
            var refinedList = RemoveOrderedDuplicatesWithEarlierTimestamps(assetCsvList);

            List<Asset> updatedAssetList = new List<Asset>();
            List<Asset> newAssetList = new List<Asset>();
            foreach (var fileAsset in refinedList)
            {
                var newAsset = new Asset
                {
                    AssetId = fileAsset.AssetId,
                    Properties = fileAsset.Properties,
                    Timestamp = fileAsset.Timestamp,
                    Value = fileAsset.Value
                };

                var takenAsset = GetAsset(newAsset);
                if (takenAsset is null)
                {
                    Console.WriteLine($"Asset with the 'AssetId: {newAsset.AssetId}, Properties: {newAsset.Properties}' could not be found in Db.");
                }

                var isSuitable = CheckAssetCanBeAdded(newAsset);
                if (isSuitable)
                {
                    //Add the asset that is not existed in db
                    if (takenAsset == null)
                    {
                        newAssetList.Add(new Asset
                        {
                            AssetId = newAsset.AssetId,
                            Properties = newAsset.Properties,
                            Value = newAsset.Value,
                            Timestamp = newAsset.Timestamp
                        });

                        continue;
                    }

                    takenAsset.Properties = newAsset.Properties;
                    takenAsset.Value = newAsset.Value;
                    takenAsset.Timestamp = newAsset.Timestamp;

                    updatedAssetList.Add(newAsset);
                }
            }

            if (newAssetList.Any())
            {
                await _context.AddRangeAsync(newAssetList);
            }

            await _context.SaveChangesAsync();

            return true;
        }

        //Just Mock Default values will be added
        public bool AddDefaultListToDb()
        {
            List<Asset> defaultAssetList = new List<Asset>
            {
                new Asset{AssetId = 1, Properties = PropertyEnum.IsCash, Value = false, Timestamp = DateTime.MinValue },
                new Asset{AssetId = 1, Properties = PropertyEnum.IsFuture, Value = false, Timestamp = DateTime.MinValue },
                new Asset{AssetId = 2, Properties = PropertyEnum.IsFixIncome, Value = false, Timestamp = DateTime.MinValue },
                new Asset{AssetId = 2, Properties = PropertyEnum.IsSwap, Value = false, Timestamp = DateTime.MinValue },
                new Asset{AssetId = 3, Properties = PropertyEnum.IsFuture, Value = false, Timestamp = DateTime.MinValue },
                new Asset{AssetId = 3, Properties = PropertyEnum.IsConvertible, Value = false, Timestamp = DateTime.MinValue },
                new Asset{AssetId = 5, Properties = PropertyEnum.IsConvertible, Value = false, Timestamp = DateTime.MinValue },
                new Asset{AssetId = 5, Properties = PropertyEnum.IsCash, Value = false, Timestamp = DateTime.MinValue },
                new Asset{AssetId = 5, Properties = PropertyEnum.IsFixIncome, Value = false, Timestamp = DateTime.MinValue },
                new Asset{AssetId = 8, Properties = PropertyEnum.IsFuture, Value = false, Timestamp = DateTime.MinValue },
                new Asset{AssetId = 8, Properties = PropertyEnum.IsSwap, Value = false, Timestamp = DateTime.MinValue },
                new Asset{AssetId = 8, Properties = PropertyEnum.IsCash, Value = false, Timestamp = DateTime.MinValue }
            };

            _context.Assets.AddRange(defaultAssetList);
            _context.SaveChanges();

            return true;
        }

        private bool CheckAssetCanBeAdded(Asset asset)
        {
            var assetTaken = _context.Assets.Where(a => a.AssetId == asset.AssetId && a.Properties == asset.Properties);

            return assetTaken == null || (!assetTaken.Any(a => a.Timestamp.CompareTo(asset.Timestamp) >= 0));
        }

        private Asset GetAsset(Asset asset)
        {
            return _context.Assets.Where(a => a.AssetId == asset.AssetId && a.Properties == asset.Properties)
                                  .FirstOrDefault();
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
                //Next asset with same property and same AssetId can not be added into refinedAssetList under favour of our ordered list
                if (!(lastAddedAssetOfRefinedList.AssetId == nextAsset.AssetId && lastAddedAssetOfRefinedList.Properties == nextAsset.Properties))
                {
                    refinedAssetList.Add(nextAsset);
                }
            }

            return refinedAssetList;
        }
    }
}
