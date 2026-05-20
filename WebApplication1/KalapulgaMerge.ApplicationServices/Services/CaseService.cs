using KalapulgaMerge.Core.Domain;
using KalapulgaMerge.Core.Dto;
using KalapulgaMerge.Core.ServiceInterface;
using KalapulgaMerge.Data;
using Microsoft.EntityFrameworkCore;

public class CaseService : ICaseService
{
    private readonly KalapulkDbContext _context;

    public CaseService(KalapulkDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CaseDTO>> GetCasesAsync()
    {
        return await _context.Cases
            .Select(x => new CaseDTO
            {
                CaseID = x.CaseID,
                UserID = x.UserID,
                Description = x.Description,
                CurrentCaseStatus = x.CurrentCaseStatus
            })
            .ToListAsync();
    }

    public async Task<CaseDTO> GetCaseByIdAsync(Guid id)
    {
        return await _context.Cases
            .Where(x => x.CaseID == id)
            .Select(x => new CaseDTO
            {
                CaseID = x.CaseID,
                UserID = x.UserID,
                Description = x.Description,
                CurrentCaseStatus = x.CurrentCaseStatus
            })
            .FirstOrDefaultAsync();
    }

   
}
