using KalapulgaMerge.Core.Domain;

namespace KalapulgaMerge.Models.Case
{
    public class CaseCreateViewModel
    {
        public Guid CaseID { get; set; }
        public int UserID { get; set; }
        public string Description { get; set; }
        public CaseStatus? CurrentCaseStatus { get; set; }
    }
}
