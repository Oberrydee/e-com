using ECommerce.API.Common.Models;

namespace ECommerce.API.Modules.Products.Entities;

public class ProductFile : BaseEntity
{
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public byte[] BinaryContent { get; set; } = [];
    public long SizeInBytes { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
