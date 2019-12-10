using System;
using System.Collections.Generic;

namespace function.Tests.EntryPoints
{
    public abstract class AbstractBuilder<T> : IBuilder<T>
    {
        private IList<Action<T>> _actions = new List<Action<T>>();

        public void WithAction(Action<T> action)
        {
            _actions.Add(action);
        }

        public T Build()
        {
            var item = InitialiseEmpty();

            foreach (var action in _actions)
            {
                action.Invoke(item);
            }

            return item;
        }

        protected abstract T InitialiseEmpty();
    }
}