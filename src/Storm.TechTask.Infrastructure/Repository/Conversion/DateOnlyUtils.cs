using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Storm.TechTask.SharedKernel.Utilities;

namespace Storm.TechTask.Infrastructure.Repository.Conversion
{
    public static class DateOnlyUtils
    {

        public static ModelBuilder RegisterDateConverters(this ModelBuilder modelBuilder)
        {
            //
            // TODO When EfCore 6 is released, this method can hopefully be replaced by something much simpler, based on this: https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-6.0/whatsnew#pre-convention-model-configuration
            //

            // Iterate over all entities, then over all properties of each entity, seeking DateOnly-valued properties.
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    // Handle both DateOnly & nullable DateOnly.
                    var propertyType = property.ClrType;
                    if (propertyType.IsNullable())
                    {
                        propertyType = Nullable.GetUnderlyingType(propertyType);
                    }

                    if (propertyType == typeof(DateOnly))
                    {
                        property.SetValueConverter(new DateOnlyConverter());
                        property.SetValueComparer(new DateOnlyComparer());
                    }
                }
            }

            return modelBuilder;
        }
    }

    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter() : base(
                dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
                dateTime => DateOnly.FromDateTime(dateTime))
        {
        }
    }

    public class DateOnlyComparer : ValueComparer<DateOnly>
    {
        public DateOnlyComparer() : base(
            (d1, d2) => d1.DayNumber == d2.DayNumber,
            d => d.GetHashCode())
        {
        }
    }
}
