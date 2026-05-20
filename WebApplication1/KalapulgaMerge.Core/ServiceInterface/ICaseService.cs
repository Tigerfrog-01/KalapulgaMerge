using KalapulgaMerge.Core.Domain;
using KalapulgaMerge.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalapulgaMerge.Core.ServiceInterface
{
    public interface ICaseService
    {
        Task<IEnumerable<CaseDTO>> GetCasesAsync();
        Task<CaseDTO> GetCaseByIdAsync(Guid id);

    }
}
