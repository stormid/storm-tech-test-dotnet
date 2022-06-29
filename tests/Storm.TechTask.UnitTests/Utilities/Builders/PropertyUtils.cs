using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Storm.TechTask.UnitTests.Utilities.Builders
{
    public static class PropertyUtils
    {
        public static T Set<T, TValue>(this T target, Expression<Func<T, TValue>> expression, TValue value)
            where T : notnull
        {
            var body = expression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("The provided lambda expression must refer to a property of : " + typeof(T).Name);
            }

            var member = body.Member as PropertyInfo;
            if (member == null)
            {
                throw new ArgumentException("The provided lambda expression must refer to a property of : " + typeof(T).Name);
            }

            var nestedTarget = GetNestedTarget(target, body.Expression!); // Expression may be e.g. myObj => myObj.NestedObj.MyProp
            member.SetValue(nestedTarget, value, null);

            return target;
        }

        private static object? GetNestedTarget(object target, [DisallowNull] Expression expr)
        {
            switch (expr.NodeType)
            {
                case ExpressionType.Parameter:
                    return target;
                case ExpressionType.MemberAccess:
                    MemberExpression mex = (MemberExpression)expr ?? throw new ArgumentException();
                    PropertyInfo pi = mex.Member as PropertyInfo ?? throw new ArgumentException();
                    target = GetNestedTarget(target, mex.Expression!) ?? throw new ArgumentException();
                    return pi.GetValue(target, null);
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}

