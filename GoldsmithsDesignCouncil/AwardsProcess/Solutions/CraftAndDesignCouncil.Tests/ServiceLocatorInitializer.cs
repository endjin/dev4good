namespace CraftAndDesignCouncil.Tests
{
    #region Using Directives

    using CraftAndDesignCouncil.Tests;
    using SharpArch.Domain;
    using System.Collections.Generic;
    using System;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using CommonServiceLocator.WindsorAdapter;
    using Microsoft.Practices.ServiceLocation;
    using SharpArch.Domain.PersistenceSupport;
    using SharpArch.NHibernate;
    using SharpArch.NHibernate.Contracts.Repositories;
    using CraftAndDesignCouncil.Domain;

    #endregion

    public class ServiceLocatorInitializer
    {
        public static void Init() 
        {
            IWindsorContainer container = new WindsorContainer();

            container.Register(
                    Component
                        .For(typeof(IEntityDuplicateChecker))
                        .ImplementedBy(typeof(EntityDuplicateChecker))
                        .Named("entityDuplicateChecker"));

                RegisterDynamicMocksFor(container, new List<Type> {
                                        typeof(INHibernateRepository<ApplicationFormSection>)});

              ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));

        }
  

        private static void RegisterDynamicMocksFor(IWindsorContainer container, IEnumerable<Type> serviceTypes)
        {
            foreach (Type serviceType in serviceTypes)
            {
                var mock = new NUnit.Mocks.DynamicMock(serviceType);
                container.Register(Component.For(serviceType).Instance(mock.MockInstance).Named(serviceType.Name));

            }
        }
        
    }
}
