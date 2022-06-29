using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Storm.TechTask.UnitTests.Utilities.Builders
{
    public abstract class Builder<T>
        where T : notnull
    {
        #region Properties

        public T Target { get; protected set; }

        #endregion


        #region Constructors

        protected Builder(T target)
        {
            this.Target = target;
        }

        #endregion


        #region Mutators

        public Builder<T> Set<TValue>(Expression<Func<T, TValue>> expression, TValue value)
        {
            this.Target.Set(expression, value);

            return this;
        }

        public Builder<T> Fill(Expression<Func<T, string>> expression, char c, int count)
        {
            return Set(expression, new string(c, count));
        }

        #endregion


        #region Building


        public T Build()
        {
            return this.Target;
        }

        public Builder<T> BuildFrom(T toCopy)
        {
            Target = ObjectCopier.Copy(toCopy);

            return this;
        }

        public Builder<T> BuildAs(T toUse)
        {
            this.Target = toUse;

            return this;
        }

        #endregion
    }
}
