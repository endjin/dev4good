namespace CraftAndDesignCouncil.Domain
{
    #region Using Directives

    using System;
    using System.ComponentModel.DataAnnotations;

    using SharpArch.Domain.DomainModel;

    #endregion

    public class Applicant : Entity
    {
        [Required(ErrorMessage = "Please fill in your first name")]
        public virtual string FirstName { get; set; }

        [Required(ErrorMessage = "Please fill in your last name")]
        public virtual string LastName { get; set; }

        [Required(ErrorMessage = "Please fill in your email address")]
        public virtual string Email { get; set; }

        [Required(ErrorMessage = "Please fill in your address")]
        public virtual Address Address { get; set; }

        public virtual DateTime ModifiedDate { get; set; }
    }
}