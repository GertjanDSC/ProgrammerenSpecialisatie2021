using eCommerce.Helpers.Repository;
using eCommerce.Helpers.Specification;
using eCommerce.DomainModelLayer.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using eCommerce.DomainModelLayer.Countries;
using eCommerce.DomainModelLayer.Purchases;

namespace eCommerce.ApplicationLayer.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<Purchase> _purchaseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(ICustomerRepository customerRepository,
            IRepository<Country> countryRepository, IRepository<Purchase> purchaseRepository, IUnitOfWork unitOfWork)
        {
            this._customerRepository = customerRepository;
            this._countryRepository = countryRepository;
            this._purchaseRepository = purchaseRepository;
            this._unitOfWork = unitOfWork;
        }

        public bool IsEmailAvailable(string email)
        {
            ISpecification<Customer> alreadyRegisteredSpec =
                new CustomerAlreadyRegisteredSpec(email);

            Customer existingCustomer = this._customerRepository.FindOne(alreadyRegisteredSpec);

            if (existingCustomer == null)
                return true;

            return false;
        }

        public CustomerDto Add(CustomerDto customerDto)
        {
            ISpecification<Customer> alreadyRegisteredSpec =
                new CustomerAlreadyRegisteredSpec(customerDto.Email);

            Customer existingCustomer = this._customerRepository.FindOne(alreadyRegisteredSpec);

            if (existingCustomer != null)
                throw new Exception("Customer with this email already exists");

            Country country = this._countryRepository.FindById(customerDto.CountryId);

            Customer customer =
                Customer.Create(customerDto.FirstName, customerDto.LastName, customerDto.Email, country);

            this._customerRepository.Add(customer);
            this._unitOfWork.Commit();

            return Map.Instance.Mapper.Map<Customer, CustomerDto>(customer);
        }

        public void Update(CustomerDto customerDto)
        {
            if (customerDto.Id == Guid.Empty)
                throw new Exception("Id can't be empty");

            ISpecification<Customer> registeredSpec =
                new CustomerRegisteredSpec(customerDto.Id);

            Customer customer = this._customerRepository.FindOne(registeredSpec);

            if (customer == null)
                throw new Exception("No such customer exists");

            customer.ChangeEmail(customerDto.Email);
            this._unitOfWork.Commit();
        }

        public void Remove(Guid customerId)
        {
            ISpecification<Customer> registeredSpec =
                new CustomerRegisteredSpec(customerId);

            Customer customer = this._customerRepository.FindOne(registeredSpec);

            if (customer == null)
                throw new Exception("No such customer exists");

            this._customerRepository.Remove(customer);
            this._unitOfWork.Commit();
        }

        public CustomerDto Get(Guid customerId)
        {
            ISpecification<Customer> registeredSpec =
                new CustomerRegisteredSpec(customerId);

            Customer customer = this._customerRepository.FindOne(registeredSpec);

            return Map.Instance.Mapper.Map<Customer, CustomerDto>(customer);
        }


        public CreditCardDto Add(Guid customerId, CreditCardDto creditCardDto)
        {
            ISpecification<Customer> registeredSpec =
                new CustomerRegisteredSpec(customerId);

            Customer customer = this._customerRepository.FindOne(registeredSpec);

            if (customer == null)
                throw new Exception("No such customer exists");

            CreditCard creditCard =
                CreditCard.Create(customer, creditCardDto.NameOnCard,
                creditCardDto.CardNumber, creditCardDto.Expiry);

            customer.Add(creditCard);

            this._unitOfWork.Commit();

            return Map.Instance.Mapper.Map<CreditCard, CreditCardDto>(creditCard);
        }

        //Approach 1 - Domain Model DTO Projection 
        public List<CustomerPurchaseHistoryDto> GetAllCustomerPurchaseHistoryV1()
        {
            IEnumerable<Guid> customersThatHavePurhcasedSomething =
                 this._purchaseRepository.Find(new PurchasedNProductsSpec(1))
                                        .Select(purchase => purchase.CustomerId)
                                        .Distinct();

            IEnumerable<Customer> customers =
                this._customerRepository.Find(new CustomerBulkIdFindSpec(customersThatHavePurhcasedSomething));

            List<CustomerPurchaseHistoryDto> customersPurchaseHistory =
                new List<CustomerPurchaseHistoryDto>();

            foreach (Customer customer in customers)
            {
                IEnumerable<Purchase> customerPurchases =
                    this._purchaseRepository.Find(new CustomerPurchasesSpec(customer.Id));

                CustomerPurchaseHistoryDto customerPurchaseHistory = new CustomerPurchaseHistoryDto();
                customerPurchaseHistory.CustomerId = customer.Id;
                customerPurchaseHistory.FirstName = customer.FirstName;
                customerPurchaseHistory.LastName = customer.LastName;
                customerPurchaseHistory.Email = customer.Email;
                customerPurchaseHistory.TotalPurchases = customerPurchases.Count();
                customerPurchaseHistory.TotalProductsPurchased =
                    customerPurchases.Sum(purchase => purchase.Products.Sum(product => product.Quantity));
                customerPurchaseHistory.TotalCost = customerPurchases.Sum(purchase => purchase.TotalCost);
                customersPurchaseHistory.Add(customerPurchaseHistory);

            }
            return customersPurchaseHistory;
        }

        //Approach 2 - Infrastructure Read Model Projection (Preferred)
        public List<CustomerPurchaseHistoryDto> GetAllCustomerPurchaseHistoryV2()
        {
            IEnumerable<CustomerPurchaseHistoryReadModel> customersPurchaseHistory =
                this._customerRepository.GetCustomersPurchaseHistory();

            return Map.Instance.Mapper.Map<IEnumerable<CustomerPurchaseHistoryReadModel>, List<CustomerPurchaseHistoryDto>>(customersPurchaseHistory);
        }
    }
}
