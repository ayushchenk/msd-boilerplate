using System;
using System.Linq;
using Microsoft.Xrm.Sdk;

namespace MSD.Shared.Extensions
{
    public static class EntityExtensions
    {
        /// <summary>
        /// Returns a value of the aliased attribute of the entity cast to T in the safe manner 
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="attributeName">Attribute name</param>
        /// <returns>Value of the attribute of the entity, default if not found</returns>
        public static T SafeGetAliasedValue<T>(this Entity entity, string attributeName)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (!entity.Attributes.Contains(attributeName))
            {
                return default;
            }

            return !(entity[attributeName] is AliasedValue attributeValue) ? default : (T)attributeValue.Value;
        }

        /// <summary>
        /// Returns a value of the attribute of the entity cast to T
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="attributeName">Attribute name</param>
        /// <returns>Value of the attribute of the entity</returns>
        public static T GetAttribute<T>(this Entity entity, string attributeName)
        {
            return entity.GetAttribute<T>(null, attributeName);
        }

        /// <summary>
        /// Returns a value of the attribute of the entity cast to T, if null then a value of image if null - default value of @T
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="image">Image</param>
        /// <param name="attributeName">Attribute name</param>
        /// <exception cref="ArgumentException">Argument exception</exception>
        /// <returns>Value of the attribute of the entity or image</returns>
        public static T GetAttribute<T>(this Entity entity, Entity image, string attributeName)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (attributeName == null)
            {
                throw new ArgumentNullException(nameof(attributeName));
            }

            object returnValue = default(T);

            if (entity.Contains(attributeName))
            {
                returnValue = entity.Attributes[attributeName];
            }
            else if (image != null && image.Contains(attributeName))
            {
                returnValue = image.Attributes[attributeName];
            }

            return (T)returnValue;
        }

        /// <summary>
        /// Gets option set field's value and casts it to a specific enum
        /// </summary>
        /// <typeparam name="T">Outgoing enum type</typeparam>
        /// <param name="entity">Entity instance</param>
        /// <param name="image">Pre operation entity instance</param>
        /// <param name="fieldName">Attribute name</param>
        /// <param name="defaultValue">Default value of T type</param>
        /// <returns>Enum value</returns>
        public static T GetOptionSetValue<T>(this Entity entity, Entity image, string fieldName, T defaultValue) where T : Enum
        {
            var value = defaultValue;
            var tempValue = GetAttribute<OptionSetValue>(entity, image, fieldName);

            if (tempValue != null)
            {
                value = (T)Enum.ToObject(typeof(T), tempValue.Value);
            }

            return value;
        }

        /// <summary>
        /// Gets option set field's value and casts it to a specific enum
        /// </summary>
        /// <typeparam name="T">Outgoing enum type</typeparam>
        /// <param name="entity">Entity instance</param>
        /// <param name="image">Pre operation entity instance</param>
        /// <param name="fieldName">Attribute name</param>
        /// <returns>Enum value</returns>
        public static T GetOptionSetValue<T>(this Entity entity, Entity image, string fieldName) where T : Enum
        {
            return GetOptionSetValue<T>(entity, image, fieldName, default);
        }
        /// <summary>
        /// Gets option set field's value and casts it to a specific enum
        /// </summary>
        /// <typeparam name="T">Outgoing enum type</typeparam>
        /// <param name="entity">Entity instance</param>
        /// <param name="fieldName">Attribute name</param>
        /// <returns>Enum value</returns>
        public static T GetOptionSetValue<T>(this Entity entity, string fieldName) where T : Enum
        {
            return GetOptionSetValue<T>(entity, null, fieldName);
        }

        /// <summary>
        /// Gets option set field's value and casts it to a specific enum
        /// </summary>
        /// <typeparam name="T">Outgoing enum type</typeparam>
        /// <param name="entity">Entity instance</param>
        /// <param name="fieldName">Filed name</param>
        /// <param name="defaultValue">Default value of T type</param>
        /// <returns>Enum value</returns>
        public static T GetOptionSetValue<T>(this Entity entity, string fieldName, T defaultValue) where T : Enum
        {
            return GetOptionSetValue(entity, null, fieldName, defaultValue);
        }
        /// <summary>
        /// Gets int value of an enum value
        /// </summary>
        /// <param name="enumValue">Enum value</param>
        /// <returns>Int value</returns>
        public static int ToInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }

        /// <summary>
        /// Converts Enum value to an Option Set value 
        /// </summary>
        /// <param name="enumValue">Enum value</param>
        /// <returns>Option set value</returns>
        public static OptionSetValue ToOptionSetValue(this Enum enumValue)
        {
            return new OptionSetValue(enumValue.ToInt());
        }

        /// <summary>
        /// Gets Aliased typed value
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="entity">Entity instance</param>
        /// <param name="attributeName">Attribute name</param>
        /// <returns>Typed value of an attribute</returns>
        /// <exception cref="InvalidCastException">Invalid cast exception</exception>
        public static T GetAliasedValue<T>(this Entity entity, string attributeName)
        {
            if (entity.Contains(attributeName) && entity[attributeName] is AliasedValue)
            {
                try
                {
                    return (T)entity.GetAttributeValue<AliasedValue>(attributeName).Value;
                }
                catch (InvalidCastException)
                {
                    throw new InvalidCastException($"Unable to cast attribute {attributeName} to {typeof(T).Name}");
                }
            }

            return default;
        }

        public static string GetFormattedValue(this Entity entity, string attributeName)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity.FormattedValues.ContainsKey(attributeName))
            {
                return entity.FormattedValues[attributeName].ToString();
            }

            return null;
        }

        public static EntityReference GetPartyWithCheck(this Entity entity, string partylistFieldName, string partyEntityName)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var partylist = entity.GetAttribute<EntityCollection>(partylistFieldName);

            if (partylist == null || partylist.Entities.Count != 1)
            {
                return null;
            }

            var party = partylist.Entities.First().GetAttribute<EntityReference>("partyid");

            if (party == null || party.LogicalName != partyEntityName)
            {
                return null;
            }

            return party;
        }
    }
}