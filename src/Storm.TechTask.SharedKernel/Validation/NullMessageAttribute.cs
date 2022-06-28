using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.TechTask.SharedKernel.Validation
{
    public class NullMessageAttribute : RequiredAttribute
    {
        public NullMessageAttribute(string message)
        {
            this.ErrorMessage = message;
        }
    }
}
