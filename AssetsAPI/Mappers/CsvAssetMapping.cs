using AssetsAPI.Enums;
using AssetsAPI.Helpers;
using AssetsAPI.Models;
using System;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace AssetsAPI.Mappers
{
    public class CsvAssetMapping : CsvMapping<AssetCsvResponseModel>
    {
        public CsvAssetMapping() : base()
        {
            MapProperty(0, x => x.AssetId);
            MapProperty(1, x => x.Properties, new AssetPropertiesTypeConverter());
            MapProperty(2, x => x.Value);
            MapProperty(3, x => x.Timestamp, new TimestampTypeConverter());
        }
    }

    class AssetPropertiesTypeConverter : EnumConverter<PropertyEnum>
    {
        protected override bool InternalConvert(string value, out PropertyEnum result)
        {
            result = (PropertyEnum)EnumExtensions.GetEnumFromDisplayValue<PropertyEnum>(value);

            return result != 0;
        }
    }

    class TimestampTypeConverter : ITypeConverter<DateTime>
    {
        public Type TargetType => typeof(DateTime);

        public bool TryConvert(string value, out DateTime result)
        {
            result = Convert.ToDateTime(value);

            return true;
        }
    }
}
