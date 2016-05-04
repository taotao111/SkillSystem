using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Code.StateMachine
{
    public class Machine<H>
    {
        private H Holder { get; set; }
        private State<H> m_Current;
        private Dictionary<string, State<H>> m_States = new Dictionary<string, State<H>>();

        public Machine(H holder)
        {
            Holder = holder;
        }

        public bool IsState<S>() where S : State<H>, new()
        {
            return m_Current is S;
        }

        public void Register<S>() where S : State<H>, new()
        {
            string key = TypeUtil.ToKey<S>();
            S state = new S();
            state.Init(this, Holder);
            m_States.Add(key, state);
        }

        public void Switch<S>() where S : State<H>, new()
        {
            State<H> state = m_States[TypeUtil.ToKey<S>()];
            if (m_Current != null)
            {
                m_Current.Exit();
            }
            state.Enter();
            m_Current = state;
        }

        public void Update(float elapsed_sec)
        {
            if (m_Current == null) return;
            m_Current.Update(elapsed_sec);
        }

        public void HanHandleMessage(IMessage message)
        {
            m_Current.HanHandleMessage(message);
        }

        public override string ToString()
        {
            return Holder.ToString() + ":" + m_Current == null ? "<none>" : m_Current.GetType().Name;
        }
    }
}