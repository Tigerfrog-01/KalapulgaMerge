using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalapulgaMerge.Core.Domain
{
    public class FileToApi
    {
        [Key]
        public Guid ImageID { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public int? ShopItemID { get; set; } 
    }
}
