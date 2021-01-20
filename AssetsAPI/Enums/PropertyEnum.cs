using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssetsAPI.Enums
{
    public enum PropertyEnum
    {
        [Display(Name ="is fix income")]
        IsFixIncome = 1,

        [Display(Name = "is convertible")]
        IsConvertible = 2,

        [Display(Name = "is swap")]
        IsSwap = 3,

        [Display(Name = "is cash")]
        IsCash = 4,

        [Display(Name = "is future")]
        IsFuture = 5
    }
}
