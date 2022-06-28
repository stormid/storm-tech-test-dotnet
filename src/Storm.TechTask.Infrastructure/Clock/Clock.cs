using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.Infrastructure.Clock
{
    public class Clock : IClock
    {
        public DateTime CurrentDateTime => DateTime.Now;
        public DateOnly CurrentDate => DateOnly.FromDateTime(DateTime.Now);
        public TimeOnly CurrentTime => TimeOnly.FromDateTime(DateTime.Now);
    }
}
