namespace AllUp.Models;

public class Basket : BaseEntity
{
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Count { get; set; }
}
