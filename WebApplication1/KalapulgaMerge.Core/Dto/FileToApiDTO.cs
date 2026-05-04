using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalapulgaMerge.Core.Dto
{
    public class FileToApiDTO
    {
        public Guid ImageID { get; set; }
        public string FilePath { get; set; }
        public int? ShopItemID { get; set; }
    }
}
