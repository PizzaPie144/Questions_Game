using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PizzaPie.QuestionsGame.Events
{
    public class EventAggregator : IEventAggregatorService      
    {

        Dictionary<Type, IEvent<EventArgs>> mEvents = new Dictionary<Type, IEvent<EventArgs>>();

        public void Subscribe<TEventArgs>(ISubscriber<TEventArgs> sub) where TEventArgs : EventArgs
        {
            IEvent<TEventArgs> ev;
            if (!mEvents.ContainsKey(typeof(TEventArgs)))
            {
                ev = new Event<TEventArgs>();
                mEvents.Add(typeof(TEventArgs), ev);
            }
            ev = mEvents[typeof(TEventArgs)] as IEvent<TEventArgs>;
            ev.Subscribe(sub);
        }

        public void Unsubscribe<TEventArgs>(ISubscriber<TEventArgs> sub) where TEventArgs : EventArgs
        {
            if (mEvents.ContainsKey(typeof(TEventArgs)))
            {
                var e = mEvents[typeof(TEventArgs)] as IEvent<TEventArgs>;
                e.Unsubscribe(sub);
            }
        }

        public void Invoke<TEventArgs>(object sender, TEventArgs args) where TEventArgs : EventArgs
        {
            if (mEvents.ContainsKey(typeof(TEventArgs)))
            {
                mEvents[typeof(TEventArgs)].Invoke(sender, args);
            }
        }

        public void Invoke(object sender, EventArgs args, Type argsType)
        {
            if (mEvents.ContainsKey(argsType))
            {
                mEvents[argsType].Invoke(sender, args);
            }
        }

        private interface IEvent<out TEventArgs> where TEventArgs : EventArgs
        {
            void Subscribe(ISubscriber<TEventArgs> sub);
            void Unsubscribe(ISubscriber<TEventArgs> sub);

            void Invoke(object sender, EventArgs e);
        }

        private class Event<T> : IEvent<T> where T : EventArgs
        {
            protected List<ISubscriber<T>> subs = new List<ISubscriber<T>>();

            public void Invoke(object sender, EventArgs e)
            {
                for (int i = 0; i < subs.Count; i++)
                    subs[i].Handler(sender, e as T);
            }

            public void Subscribe(ISubscriber<T> sub)
            {
                subs.Add(sub);
            }

            public void Unsubscribe(ISubscriber<T> sub)
            {
                subs.Remove(sub);
            }
        }

    }


}
