using AssetsAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetsAPI.Models
{
    public class AssetCsvResponseModel
    {
        public long AssetId { get; set; }

        public PropertyEnum Properties { get; set; }

        public bool Value { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
