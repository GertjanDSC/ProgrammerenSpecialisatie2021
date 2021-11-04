using eCommerce.Helpers.Domain;

namespace eCommerce.DomainModelLayer.Products
{
    public class ProductCodeCreated : DomainEvent
    {
        public ProductCode ProductCode { get; set; }

        public override void Flatten()
        {
            this.Args.Add("ProductCodeId", this.ProductCode.Id);
            this.Args.Add("ProductCodeName", this.ProductCode.Name);
        }
    }
}
