using Microsoft.AspNetCore.Http;

namespace KalapulgaMerge.Core.Dto;


public class ShopItemDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Type { get; set; }
    public List<IFormFile>? Files { get; set; }
    public IEnumerable<FileToApiDTO>? FileToApiDTOs { get; set; } = new List<FileToApiDTO>();
    public IEnumerable<FileToApiDTO> Images { get; set; } = new List<FileToApiDTO>();
}
