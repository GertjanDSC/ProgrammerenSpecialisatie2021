using eCommerce.Helpers.Domain;

namespace eCommerce.DomainModelLayer.Countries
{
    public class CountryCreated : DomainEvent
    {
        public Country Country { get; set; }

        public override void Flatten()
        {
            this.Args.Add("Id", this.Country.Id);
            this.Args.Add("Name", this.Country.Name);
        }
    }
}
