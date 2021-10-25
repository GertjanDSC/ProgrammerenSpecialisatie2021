using System;
using eCommerce.Helpers.Repository;
using eCommerce.DomainModelLayer.Customers;
using eCommerce.DomainModelLayer.Products;
using eCommerce.DomainModelLayer.Purchases;
using eCommerce.DomainModelLayer.Carts;
using eCommerce.DomainModelLayer.Services;

namespace eCommerce.ApplicationLayer.Carts
{
    public class CartService : ICartService
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Cart> _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TaxService _taxDomainService;
        private readonly CheckoutService _checkoutDomainService; 

        public CartService(IRepository<Customer> customerRepository, 
            IRepository<Product> productRepository, IRepository<Cart> cartRepository, 
            IUnitOfWork unitOfWork, TaxService taxDomainService, CheckoutService checkoutDomainService)
        {
            this._customerRepository = customerRepository;
            this._productRepository = productRepository;
            this._cartRepository = cartRepository;
            this._unitOfWork = unitOfWork;
            this._taxDomainService = taxDomainService;
            this._checkoutDomainService = checkoutDomainService;
        }

        public CartDto Add(Guid customerId, CartProductDto productDto)
        {
            CartDto cartDto = null;

            Customer customer = this._customerRepository.FindById(customerId);
            if (customer == null)
                throw new Exception(String.Format("Customer was not found with this Id: {0}", customerId));

            Cart cart = this._cartRepository.FindOne(new CustomerCartSpec(customerId));
            if(cart == null)
            {
                cart = Cart.Create(customer);
                this._cartRepository.Add(cart);
            }

            Product product = this._productRepository.FindById(productDto.ProductId);
            this.ValidateProduct(product.Id, product);

            //Double Dispatch Pattern
            cart.Add(CartProduct.Create(customer, cart, product, 
                productDto.Quantity, _taxDomainService));

            cartDto = Map.Instance.Mapper.Map<Cart, CartDto>(cart);
            this._unitOfWork.Commit();
            return cartDto;
        }

        public CartDto Remove(Guid customerId, Guid productId)
        {
            CartDto cartDto = null;

            Cart cart = this._cartRepository.FindOne(new CustomerCartSpec(customerId));
            this.ValidateCart(customerId, cart);

            Product product = this._productRepository.FindById(productId);
            this.ValidateProduct(productId, product);

            cart.Remove(product);
            cartDto = Map.Instance.Mapper.Map<Cart, CartDto>(cart);
            this._unitOfWork.Commit();
            return cartDto;
        }

        public CartDto Get(Guid customerId)
        {
            Cart cart = this._cartRepository.FindOne(new CustomerCartSpec(customerId));
            this.ValidateCart(customerId, cart);

            return Map.Instance.Mapper.Map<Cart, CartDto>(cart);
        }

        public CheckOutResultDto CheckOut(Guid customerId)
        {
            CheckOutResultDto checkOutResultDto = new CheckOutResultDto();

            Cart cart = this._cartRepository.FindOne(new CustomerCartSpec(customerId));
            this.ValidateCart(customerId, cart);

            Customer customer = this._customerRepository.FindById(cart.CustomerId);

            Nullable<CheckOutIssue> checkOutIssue = 
                this._checkoutDomainService.CanCheckOut(customer, cart);

            if (!checkOutIssue.HasValue)
            {
                Purchase purchase = this._checkoutDomainService.Checkout(customer, cart);
                checkOutResultDto = Map.Instance.Mapper.Map<Purchase, CheckOutResultDto>(purchase);
                this._unitOfWork.Commit();
            }
            else
            {
                checkOutResultDto.CheckOutIssue = checkOutIssue;
            }

            return checkOutResultDto;
        }

        private void ValidateCart(Guid customerId, Cart cart)
        {
            if (cart == null)
                throw new Exception(String.Format("Customer was not found with this Id: {0}", customerId));
        }

        private void ValidateProduct(Guid productId, Product product)
        {
            if (product == null)
                throw new Exception(String.Format("Product was not found with this Id: {0}", productId));
        }
    }
}
