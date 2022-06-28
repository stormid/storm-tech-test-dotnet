namespace Storm.TechTask.SharedKernel.Entities
{
    public class BusinessRuleException : Exception
    {
        protected BusinessRuleException() { }

        protected BusinessRuleException(string message) : base(message) { }

        protected BusinessRuleException(string message, Exception inner) : base(message, inner) { }
    }

}
