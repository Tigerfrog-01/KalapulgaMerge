using KalapulgaMerge.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalapulgaMerge.Core.Dto
{
    public class CaseDTO
    {
        public Guid CaseID { get; set; }
        public int UserID { get; set; }
        public string Description { get; set; }
        public CaseStatus? CurrentCaseStatus { get; set; }
    }
}
