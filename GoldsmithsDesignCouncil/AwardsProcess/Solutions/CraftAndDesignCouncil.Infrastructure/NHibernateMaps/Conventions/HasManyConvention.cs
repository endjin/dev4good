namespace CraftAndDesignCouncil.Infrastructure.NHibernateMaps.Conventions
{
    #region Using Directives

    using FluentNHibernate.Conventions;

    #endregion

    public class HasManyConvention : IHasManyConvention
    {
        public void Apply(FluentNHibernate.Conventions.Instances.IOneToManyCollectionInstance instance)
        {
            instance.Key.Column(instance.EntityType.Name + "Id");
            instance.Cascade.AllDeleteOrphan();
            //not sure about this choosing not inverse for now as it seems easier to manage 
            //collections from the collection owning side 
            //but would be easily convinced in the other direction
            //Howard or Mark - Happy to discuss if either of you have strong prefeerences here
            //instance.Inverse();  
        }
    }
}