namespace AllUp.Models;

public class Category: BaseEntity
{
    public string Name { get; set; }
    public string Image { get; set; }
    public int? ParentId { get; set; }
    public bool IsMain { get; set; }
    public Category Parent { get; set; }
    public List<Category> Children { get; set; }
    public List<Product> Products { get; set; }
}
