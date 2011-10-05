using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.Domain.PersistenceSupport;
using System.ComponentModel.DataAnnotations;

namespace CraftAndDesignCouncil.Infrastructure.Queries
{
    public interface IQueryRunner
    {
        IList<TResult> RunQuery<TResult>(IQuery<TResult> query);
    }

    public class QueryRunner : IQueryRunner
    {
        public IList<TResult> RunQuery<TResult>(IQuery<TResult> query)
        {
            Validator.ValidateObject(query, new ValidationContext(query, null,null),true);

            return query.ExecuteQuery();
        }

    }
}
