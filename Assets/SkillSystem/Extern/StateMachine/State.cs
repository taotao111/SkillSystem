using UnityEngine;
using System.Collections;
namespace Code.StateMachine
{
    public abstract class State<H>
    {
        protected Machine<H> Machine { get; set; }
        protected H Holder { get; private set; }

        public void Init(Machine<H> machine, H holder)
        {
            Machine = machine;
            Holder = holder;
            _Init();
        }

        public void Enter()
        {
            _Enter();
        }

        public void HanHandleMessage(IMessage message)
        {

        }

        public void Update(float elapsed_sec)
        {
            _Update(elapsed_sec);
        }

        public void Exit()
        {
            _Exit();
        }

        protected virtual void _Init() { }

        public abstract void _Enter();

        public abstract void _Update(float elapsed_sec);

        public abstract void _Exit();

        protected virtual void _HandleMessage(IMessage message) { }

        protected void Switch<S>() where S : State<H>, new()
        {
            Machine.Switch<S>();
        }
    }
}