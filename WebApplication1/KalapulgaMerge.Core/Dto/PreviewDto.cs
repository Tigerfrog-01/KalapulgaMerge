namespace KalapulgaMerge.Core.Dto;



public record PreviewRequestDto(int ItemId);

public record PreviewResultDto(
    bool Success,
    string Message,
    string ItemImageUrl,        // ese (müts / prillid / värv)
    string FishstickImageUrl    // kalapulk baaspilt
);
