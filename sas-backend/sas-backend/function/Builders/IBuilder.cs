using System;

namespace function.Builders
{
    public interface IBuilder<out T>
    {
        void AppendAction(Action<T> action);
        T Build();
    }
}