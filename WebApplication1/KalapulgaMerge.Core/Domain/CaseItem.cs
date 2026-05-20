using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalapulgaMerge.Core.Domain
{
    public enum CaseStatus
    {
        Solved,
        InProgress,
        Awaiting
    }
    public class CaseItem
    {
        [Key]
        public Guid CaseID { get; set; }
        public int UserID { get; set; }
        public string Description { get; set; }
        public CaseStatus? CurrentCaseStatus { get; set; }
    }
}
