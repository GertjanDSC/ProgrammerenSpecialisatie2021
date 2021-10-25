using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using eCommerce.ApplicationLayer.Carts;
using eCommerce.ApplicationLayer.Customers;
using eCommerce.ApplicationLayer.History;
using eCommerce.ApplicationLayer.Products;
using eCommerce.DomainModelLayer;
using eCommerce.DomainModelLayer.Carts;
using eCommerce.DomainModelLayer.Countries;
using eCommerce.DomainModelLayer.Customers;
using eCommerce.DomainModelLayer.Email;
using eCommerce.DomainModelLayer.Newsletter;
using eCommerce.DomainModelLayer.Products;
using eCommerce.DomainModelLayer.Tax;
using eCommerce.Helpers.Domain;
using eCommerce.Helpers.Repository;
using eCommerce.InfrastructureLayer;
using System;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Castle.Windsor.IWindsorContainer container = new WindsorContainer();
            //Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store;
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            // Infrastructure
            container.Register(Component.For<IEmailDispatcher>().ImplementedBy<SmtpEmailDispatcher>().LifeStyle.Singleton);
            container.Register(Component.For<INewsletterSubscriber>().ImplementedBy<WSNewsletterSubscriber>().LifeStyle.Singleton);
            container.Register(Component.For<IEmailGenerator>().ImplementedBy<StubEmailGenerator>().LifeStyle.Singleton);

            container.Register(Component.For(typeof(IRepository<>), typeof(MemoryRepository<>)).ImplementedBy(typeof(MemoryRepository<>)).LifestyleSingleton());
            container.Register(Component.For<IUnitOfWork>().ImplementedBy<MemoryUnitOfWork>().LifestyleSingleton());

            container.Register(Component.For<IRepository<ProductCode>>().ImplementedBy<StubDataProductCodeRepository>().LifestyleSingleton());
            container.Register(Component.For<IRepository<Country>>().ImplementedBy<StubDataCountryRepository>().LifestyleSingleton());
            container.Register(Component.For<IRepository<CountryTax>>().ImplementedBy<StubDataCountryTaxRepository>().LifestyleSingleton());
            container.Register(Component.For<IRepository<Product>>().ImplementedBy<StubDataProductRepository>().LifestyleSingleton());
            container.Register(Component.For<ICustomerRepository>().ImplementedBy<StubDataCustomerRepository>().LifestyleSingleton());
            container.Register(Component.For<IDomainEventRepository>().ImplementedBy<MemDomainEventRepository>().LifestyleSingleton());

            // Application
            container.Register(Classes.FromAssembly(typeof(eCommerce.ApplicationLayer.Customers.ICustomerService).Assembly)
                .Where(x => !x.IsInterface && !x.IsAbstract && !x.Name.EndsWith("Dto") && x.Namespace.Contains("ApplicationLayer"))
                .WithService.DefaultInterfaces()
                .Configure(c => c.LifestyleTransient()));

            container.Register(Classes.FromAssemblyNamed("ECommerce.Domain")
                .BasedOn(typeof(Handles<>))
                .WithService.FromInterface(typeof(Handles<>))
                .Configure(c => c.LifestyleTransient()));

            container.Register(Classes.FromAssemblyNamed("ECommerce.Domain")
                .BasedOn<IDomainService>()
                .WithService.DefaultInterfaces()
                .Configure(c => c.LifestyleTransient()));

            container.Register(Component.For<Settings>()
                .Instance(new Settings(Country.Create(new Guid("229074BD-2356-4B5A-8619-CDEBBA71CC21"), "United Kingdom"))
                    )
               );

            // Domain
            container.Register(Component.For<Handles<CartCreated>>().ImplementedBy<DomainEventHandle<CartCreated>>().LifestyleSingleton());
            container.Register(Component.For<Handles<ProductAddedCart>>().ImplementedBy<DomainEventHandle<ProductAddedCart>>().LifestyleSingleton());
            container.Register(Component.For<Handles<ProductRemovedCart>>().ImplementedBy<DomainEventHandle<ProductRemovedCart>>().LifestyleSingleton());
            container.Register(Component.For<Handles<CountryCreated>>().ImplementedBy<DomainEventHandle<CountryCreated>>().LifestyleSingleton());
            container.Register(Component.For<Handles<CreditCardAdded>>().ImplementedBy<DomainEventHandle<CreditCardAdded>>().LifestyleSingleton());
            container.Register(Component.For<Handles<CustomerChangedEmail>>().ImplementedBy<DomainEventHandle<CustomerChangedEmail>>().LifestyleSingleton());
            container.Register(Component.For<Handles<CustomerCheckedOut>>().ImplementedBy<DomainEventHandle<CustomerCheckedOut>>().LifestyleSingleton());
            container.Register(Component.For<Handles<CustomerCreated>>().ImplementedBy<DomainEventHandle<CustomerCreated>>().LifestyleSingleton());
            container.Register(Component.For<Handles<ProductCodeCreated>>().ImplementedBy<DomainEventHandle<ProductCodeCreated>>().LifestyleSingleton());
            container.Register(Component.For<Handles<ProductCreated>>().ImplementedBy<DomainEventHandle<ProductCreated>>().LifestyleSingleton());
            container.Register(Component.For<Handles<CountryTaxCreated>>().ImplementedBy<DomainEventHandle<CountryTaxCreated>>().LifestyleSingleton());

            DomainEvents.Init(container);

            var settingsRoot = container.Resolve<Settings>();
            var cartRoot = container.Resolve<ICartService>();
            var historyRoot = container.Resolve<IHistoryService>();
            var productRoot = container.Resolve<IProductService>();
            var customerRoot = container.Resolve<ICustomerService>();
            customerRoot.Add(new CustomerDto { Id = Guid.NewGuid(), FirstName = "Luc", LastName = "Vervoort", Email = "luc.vervoort@hogent.be", CountryId = settingsRoot.BusinessCountry.Id });
        }
    }
}
