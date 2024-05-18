using System.ComponentModel.DataAnnotations;

namespace agile_dev;

public partial class Blog
{
    public Blog()
    {
    }

    public int BlogId { get; set; }

    [StringLength(200)]
    public string Name { get; set; }

    [StringLength(200)]
    public string Url { get; set; }
    
}