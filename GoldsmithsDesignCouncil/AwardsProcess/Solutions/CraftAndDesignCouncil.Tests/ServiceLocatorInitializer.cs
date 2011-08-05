namespace CraftAndDesignCouncil.Tests
{
    #region Using Directives

    using System.Collections.Generic;
    using System;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using SharpArch.Domain.PersistenceSupport;
    using SharpArch.NHibernate;

    #endregion

    public class ServiceLocatorInitializer
    {
        public static void Init<TSut,TContract>() where TSut : TContract
        {
            IWindsorContainer container = new WindsorContainer();

            container.Register(
                    Component
                        .For(typeof(IEntityDuplicateChecker))
                        .ImplementedBy(typeof(EntityDuplicateChecker))
                        .Named("entityDuplicateChecker"));
        }
    }
}
