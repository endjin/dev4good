namespace CraftAndDesignCouncil.Infrastructure.Queries
{
    #region Using Directives
    using System;
    using CraftAndDesignCouncil.Domain;
    using CraftAndDesignCouncil.Domain.Contracts.Queries;
    using NHibernate.Linq;
    using SharpArch.NHibernate;
    using System.Collections.Generic;
    using System.Linq;
    #endregion

    public class AllQuestionsQuery : NHibernateQuery<Question>, IAllQuestionsQuery
    {
        public override IList<Question> ExecuteQuery()
        {
            return Session.Query<Question>().ToList();
        }
    }
}