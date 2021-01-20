using AssetsAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetsAPI.Models
{
    public class AssetIdRequestModel
    {
        public PropertyEnum Properties { get; set; }
        public bool Value { get; set; }
    }
}
