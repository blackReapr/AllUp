namespace AllUp.Models;

public class Brand : BaseEntity
{
    public string Name { get; set; }
    public List<Brand> Brands { get; set; }
}
