using System;

namespace function.Tests.EntryPoints
{
    public interface IBuilder<out T>
    {
        void WithAction(Action<T> action);
        T Build();
    }
}