using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ardalis.GuardClauses;

namespace Storm.TechTask.SharedKernel.Utilities
{
    [AttributeUsage(AttributeTargets.All)]
    public class EnumDescriptorAttribute : Attribute
    {
        public string DisplayText { get; set; }
        public string AbbreviatedDisplayText { get; set; }
        public bool IsActive { get; set; }

        public EnumDescriptorAttribute() : this(String.Empty)
        {
        }

        public EnumDescriptorAttribute(string displayText)
        {
            this.DisplayText = displayText;
            this.AbbreviatedDisplayText = String.Empty;
            this.IsActive = true;
        }
    }

    public static class EnumUtils
    {
        public static T? GetEnumValueAttribute<T>(this object enumValue) where T : Attribute
        {
            T? attribute = null;

            var fieldInfo = enumValue.GetType().GetField(Guard.Against.NullOrEmpty(enumValue.ToString(), nameof(enumValue)));
            if (fieldInfo == null)
            {
                throw new ArgumentException($"{enumValue} has no FieldInfo");
            }
            else
            {
                var attributes = Attribute.GetCustomAttributes(fieldInfo, typeof(T), false);
                if (attributes.Length > 0)
                {
                    attribute = (T)attributes[0];
                }

                return attribute;
            }
        }

        public static bool HasFlagSet<T>(this T instanceValue, T flag)
        {
            Guard.Against.Null(instanceValue);
            if (Enum.GetUnderlyingType(instanceValue.GetType()) == typeof(ulong))
            {
                return (Convert.ToUInt64(instanceValue) & Convert.ToUInt64(flag)) == Convert.ToUInt64(flag);
            }
            return (Convert.ToInt64(instanceValue) & Convert.ToInt64(flag)) == Convert.ToInt64(flag);
        }

        public static IList<T> GetAllFlaggedEnumValuesAsList<T>()
        {
            return Enum.GetValues(typeof(T).GetNullableTypeBaseType()).Cast<T>().ToList();
        }

        public static IList<T> GetFlaggedEnumValueAsList<T>(this T instanceValue)
        {
            return GetAllFlaggedEnumValuesAsList<T>().Where(enumValue => instanceValue.HasFlagSet(enumValue)).ToList();
        }

        private static string GetDisplayText<T>(T value, bool abbreviated)
        {
            Guard.Against.Null(value);
            var valueAsString = value.ToString();
            Guard.Against.Null(valueAsString);

            // Get a list of enum values to display based on whether this is a flagged enum or not.
            var enumValues = Attribute.IsDefined(typeof(T).GetNullableTypeBaseType(), typeof(FlagsAttribute)) ? value.GetFlaggedEnumValueAsList() : new List<T> { value };

            // Get the DisplayText for each of these values.
            IList<string> displayTextElements = new List<string>();
            foreach (var enumValue in enumValues)
            {
                Guard.Against.Null(enumValue);
                var attribute = enumValue.GetEnumValueAttribute<EnumDescriptorAttribute>();

                // Where no attribute has been set, use the enum's symbolic name and break it into words if it is pascal case.
                displayTextElements.Add((attribute == null)
                                            ? Inflector.Separate(valueAsString)
                                            : (abbreviated ? attribute.AbbreviatedDisplayText : attribute.DisplayText));
            }

            // Return a CSV list of these.
            return string.Join(", ", displayTextElements.ToArray());
        }

        public static string GetDisplayText<T>(this T value)
        {
            return GetDisplayText(value, false);
        }

        public static string GetAbbreviatedDisplayText<T>(this T value)
        {
            return GetDisplayText(value, true);
        }

        public static bool IsActive<T>(this T value)
        {
            Guard.Against.Null(value, nameof(value));

            var attribute = value.GetEnumValueAttribute<EnumDescriptorAttribute>();
            if (attribute == null)
            {
                return true;
            }
            else
            {
                return attribute.IsActive;
            }
        }
    }
}
