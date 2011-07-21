namespace CraftAndDesignCouncil.Domain
{
    using System;
    using System.Reflection;
    using SharpArch.Domain.DomainModel;

    public static class EntityExtensions
    {
        private static bool TrySetPropertyValue(object from, object to, PropertyInfo fromProp, PropertyInfo toProp)
        {
            if (from != null && to != null
                && fromProp != null && toProp != null
                && fromProp.CanRead && toProp.CanWrite
                && toProp.PropertyType.IsAssignableFrom(fromProp.PropertyType))
            {
                toProp.SetValue(to, fromProp.GetValue(from, null), null);
            }
            else
            {
                return false;
            }
            return true;
        }

        private static void CopyPropertiesSameType(Entity from, Entity to)
        {
            PropertyInfo[] props = GetPropertiesToCopy(from.GetType());
            foreach (PropertyInfo prop in props)
            {
                TrySetPropertyValue(from, to, prop, prop);
            }
        }
  
        private static PropertyInfo[] GetPropertiesToCopy(Type type)
        {
            PropertyInfo[] props = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            return props;
        }

        private static void CopyProperties(Entity from, Entity to)
        {
            Type fromType = from.GetType();
            Type toType = to.GetType();
            if (fromType == toType)
            {
                CopyPropertiesSameType(from, to);
                return;
            }

            if (!fromType.IsAssignableFrom(toType))
            {
                throw new InvalidOperationException("Cannot copy properties betwwen incompatible types");
            }

            PropertyInfo[] props = GetPropertiesToCopy(fromType);
            foreach (PropertyInfo prop in props)
            {
                PropertyInfo targetProp = toType.GetProperty(prop.Name, prop.PropertyType);
                TrySetPropertyValue(from, to, prop, targetProp);
            }

        }

        public static void CopyAllPropertiesFrom(this Entity to, Entity from)
        {
            CopyProperties(from, to);
        }

        public static void CopyAllPropertiesTo(this Entity from, Entity to)
        {
            CopyProperties(from, to);
        }

        public static T CloneTo<T>(this T from) where T:Entity
        {
            T newEntity = Activator.CreateInstance<T>();
            newEntity.CopyAllPropertiesFrom(from);
            return newEntity;
        }
    }

}
