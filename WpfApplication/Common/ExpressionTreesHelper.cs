using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace MaCompta.Common
{
    class ExpressionTreesHelper
    {
        public static Expression BuilNotifyPropertyChangedAspect<T>(Expression instance,
                              string propertyName, Expression value)
        {
            return BuilNotifyPropertyChangedAspect(typeof(T), instance, propertyName, value);
        }

        public static Expression BuilNotifyPropertyChangedAspect(Type type, Expression expression,
                                                                                              string propertyName, Expression value)
        {
            Expression obj = Expression.Convert(expression, type);
            EventInfo propertyChangedEvent = type.GetEvent("PropertyChanged");
            FieldInfo eventField = GetField(type, propertyChangedEvent.Name);
            MethodInfo m = propertyChangedEvent.EventHandlerType.GetMethod("Invoke");

            return
                Expression.Block(
                //_prop = value;
                    Expression.Assign(Expression.Property(obj, propertyName), value),
                    Expression.AndAssign(Expression.Property(obj, "IsModified"), Expression.Constant(true)),
                //if (PropertyChanged != null)
                    Expression.Condition(Expression.NotEqual(Expression.Field(obj, eventField),
                    Expression.Constant(null)),
                //PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                    Expression.Call(Expression.Field(obj, eventField), m, obj,
                    Expression.Constant(new PropertyChangedEventArgs(propertyName))),
                    Expression.Empty()),
                    Expression.Convert(value, typeof(object)));
        }

        public static FieldInfo GetField(Type t, string fieldName)
        {
            const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Default | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
            return (t == null) ? null : (t.GetField(fieldName, flags)) ??
                GetField(t.BaseType, fieldName);
        }
    }
}
