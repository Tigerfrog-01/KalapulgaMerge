using KalapulgaMerge.Core.Dto;

namespace KalapulgaMerge.Core.ServiceInterface;

public interface IPreviewService
{

    Task<PreviewResultDto> GetPreviewAsync(PreviewRequestDto request);
}
