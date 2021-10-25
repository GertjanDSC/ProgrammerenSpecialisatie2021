using AutoMapper;
using eCommerce.ApplicationLayer.Carts;
using eCommerce.ApplicationLayer.Customers;
using eCommerce.ApplicationLayer.History;
using eCommerce.ApplicationLayer.Products;
using eCommerce.DomainModelLayer.Carts;
using eCommerce.DomainModelLayer.Customers;
using eCommerce.DomainModelLayer.Products;
using eCommerce.DomainModelLayer.Purchases;
using eCommerce.Helpers.Domain;

namespace eCommerce.ApplicationLayer
{
    public class Map
    {
        private static  Map _instance;
        public static Map Instance { get { if (_instance == null) _instance = new Map(); return _instance; } }

        private readonly MapperConfiguration _config;

        public IMapper Mapper { get; }

        public Map()
        {
            _config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Cart, CartDto>();
                cfg.CreateMap<CartProduct, CartProductDto>();

                cfg.CreateMap<Purchase, CheckOutResultDto>()
                    .ForMember(x => x.PurchaseId, options => options.MapFrom(x => x.Id));

                cfg.CreateMap<CreditCard, CreditCardDto>();
                cfg.CreateMap<Customer, CustomerDto>();
                cfg.CreateMap<Product, ProductDto>();
                cfg.CreateMap<CustomerPurchaseHistoryReadModel, CustomerPurchaseHistoryDto>();
                cfg.CreateMap<DomainEventRecord, EventDto>();
            });

            Mapper = _config.CreateMapper();
        }
    }
}
