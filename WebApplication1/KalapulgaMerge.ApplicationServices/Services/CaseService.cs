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
    public async Task<CaseItem> CreateAsync(CaseDTO dto)
    {
        var domain = new CaseItem
        {
            CaseID = Guid.NewGuid(),
            UserID = dto.UserID,
            Description = dto.Description,
            CurrentCaseStatus = dto.CurrentCaseStatus
        };

        _context.Cases.Add(domain);
        await _context.SaveChangesAsync();
        return domain;
    }

    public async Task<CaseItem> UpdateAsync(CaseDTO dto)
    {
        var domain = await _context.Cases.FindAsync(dto.CaseID);
        if (domain == null) return null;

        domain.UserID = dto.UserID;
        domain.Description = dto.Description;
        domain.CurrentCaseStatus = dto.CurrentCaseStatus;

        _context.Cases.Update(domain);
        await _context.SaveChangesAsync();
        return domain;
    }


}
