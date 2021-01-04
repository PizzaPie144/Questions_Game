using System;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie.Events
{
    public interface IEventAggregatorService 
    {
        void Subscribe<TEventArgs>(ISubscriber<TEventArgs> sub) where TEventArgs : EventArgs;
        void Unsubscribe<TEventArgs>(ISubscriber<TEventArgs> sub) where TEventArgs : EventArgs;
        void Invoke<TEventArgs>(object sender, TEventArgs args) where TEventArgs : EventArgs;

        void Invoke(object sender, EventArgs args, Type argsType);
    }
}
