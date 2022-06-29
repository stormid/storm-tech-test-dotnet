using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Storm.TechTask.SharedKernel.Entities;
using Storm.TechTask.SharedKernel.Interfaces;
using Storm.TechTask.UnitTests.Utilities.Builders;

namespace Storm.TechTask.Api.IntegrationTests.Utilities
{
    public static class EntityBuilderUtils
    {
        public static async Task<T> BuildAndPersist<T>(this BaseEntityBuilder<T> builder)
            where T : BaseEntity, IAggregateRoot
        {
            await BaseIntegrationFixture.NewRepository().AddAsync(builder.Target, default);

            return builder.Target;
        }
    }
}
