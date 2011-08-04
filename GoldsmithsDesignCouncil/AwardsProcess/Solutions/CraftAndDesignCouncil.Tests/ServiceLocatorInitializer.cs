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
    using System.Linq;
    using CraftAndDesignCouncil.Domain;
    using System.Reflection;
    using SharpArch.Domain.DomainModel;

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

            RegisterDynamicMocksFor(container,
                                    GetRepositoryTypesForEntitiesInAssembly(typeof(Applicant).Assembly));
                                    

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));

        }

        private static IEnumerable<Type> GetRepositoryTypesForEntitiesInAssembly(Assembly assembly)
        {
            var entities = from x in assembly.GetTypes()
                          where typeof(Entity).IsAssignableFrom(x)
                          select typeof(INHibernateRepository<>).MakeGenericType(x);

            return entities;
        }
  

        private static void RegisterDynamicMocksFor(IWindsorContainer container, IEnumerable<Type> serviceTypes)
        {
            foreach (Type serviceType in serviceTypes)
            {
                var mock = new NUnit.Mocks.DynamicMock(serviceType);
                container.Register(Component.For(serviceType).Instance(mock.MockInstance));

            }
        }
        
    }
}
