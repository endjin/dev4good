namespace CraftAndDesignCouncil.Infrastructure.Queries
{
    using CraftAndDesignCouncil.Domain;
    using CraftAndDesignCouncil.Domain.Contracts.Queries;
    using System.Collections.Generic;
    using SharpArch.NHibernate;
    using NHibernate.Linq;
    using System.Linq;

    public class GetOrderedListOfSectionsQuery : NHibernateQuery<ApplicationFormSection>,IGetOrderedListOfSectionsQuery
    {
        public override IList<ApplicationFormSection> ExecuteQuery()
        {
            var res =  from x in  Session.Query<ApplicationFormSection>() orderby x.OrderingKey select x;
            return res.ToList();
                        
                      
        }
    }
}