namespace CraftAndDesignCouncil.Domain
{
    #region Using Directives

    using System;
    using System.ComponentModel.DataAnnotations;

    using SharpArch.Domain.DomainModel;

    #endregion

    public class Address : Entity
    {
        [Required(ErrorMessage = "Please fill in the first line of your address")]
        public virtual string AddressLine1 { get; set; }

        public virtual string AddressLine2 { get; set; }

        [Required(ErrorMessage = "Please fill in your city")]
        public virtual string City { get; set; }

        [Required(ErrorMessage = "Please fill in your State / Province / County")]
        public virtual string StateProvince { get; set; }

        [Required(ErrorMessage = "Please fill in your Post Code")]
        public virtual string PostCode { get; set; }

        [Required(ErrorMessage = "Please fill in your Country")]
        public virtual string Country { get; set; }

        public virtual DateTime ModifiedDate { get; set; }
    }
}