using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.TechTask.SharedKernel.Entities
{
    // This can be modified to BaseEntity<TId> to support multiple key types (e.g. Guid)
    [Serializable]
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
    }

}
