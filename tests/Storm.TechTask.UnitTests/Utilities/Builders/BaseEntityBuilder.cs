using System;
using System.Linq.Expressions;

using Storm.TechTask.SharedKernel.Entities;

namespace Storm.TechTask.UnitTests.Utilities.Builders
{
    public abstract class BaseEntityBuilder<T> : Builder<T>
        where T : BaseEntity
    {
        #region Constructors

        protected BaseEntityBuilder(T target) : base(target)
        {
        }

        #endregion


        #region Mutators

        public new BaseEntityBuilder<T> Set<TValue>(Expression<Func<T, TValue>> expression, TValue value)
        {
            return (BaseEntityBuilder<T>)base.Set(expression, value);
        }

        #endregion


        #region Build

        public new BaseEntityBuilder<T> BuildFrom(T toCopy)
        {
            return (BaseEntityBuilder<T>)base.BuildFrom(toCopy);
        }

        public new BaseEntityBuilder<T> BuildAs(T toUse)
        {
            return (BaseEntityBuilder<T>)base.BuildAs(toUse);
        }

        #endregion
    }
}

