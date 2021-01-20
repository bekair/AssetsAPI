using AssetsAPI.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetsAPI.Entities
{
    public class Asset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public long AssetId { get; set; }

        [Required]
        public PropertyEnum Properties { get; set; }

        [Required]
        public bool Value { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}
