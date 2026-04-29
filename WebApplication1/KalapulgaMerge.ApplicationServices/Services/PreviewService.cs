using KalapulgaMerge.Core.Dto;
using KalapulgaMerge.Core.ServiceInterface;
using KalapulgaMerge.Data;
using Microsoft.EntityFrameworkCore;

namespace KalapulgaMerge.ApplicationServices.Services;

public class PreviewService : IPreviewService
{
    private readonly KalapulkDbContext _context;

    // Kalapulga baaspilt – see kuvatakse alati taustal
    private const string FishstickBaseImage = "/lib/defaultassets/image/fishstick_base.png";

    public PreviewService(KalapulkDbContext context)
    {
        _context = context;
    }

    public async Task<PreviewResultDto> GetPreviewAsync(PreviewRequestDto request)
    {
        // Ainult lugemine – ei muuda midagi
        var item = await _context.ShopItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.ItemId && x.IsAvailable);

        if (item is null)
        {
            return new PreviewResultDto(
                Success: false,
                Message: "Eset ei leitud.",
                ItemImageUrl: string.Empty,
                FishstickImageUrl: FishstickBaseImage
            );
        }

        return new PreviewResultDto(
            Success: true,
            Message: $"{item.Name} eelvaade",
            ItemImageUrl: item.ImageUrl,
            FishstickImageUrl: FishstickBaseImage
        );
    }
}
