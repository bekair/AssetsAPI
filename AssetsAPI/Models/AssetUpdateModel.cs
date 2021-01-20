using AssetsAPI.Enums;
using System;

namespace AssetsAPI.Models
{
    public class AssetUpdateModel
    {
        public long AssetId { get; set; }
        public PropertyEnum Properties { get; set; }
        public bool Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
