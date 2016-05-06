
namespace Code.SkillSystem.Runtime
{
    public class TimeEventDelegate : TimeEvent
    {
        public System.Action eventDelegate;

        public TimeEventDelegate()
        : base()
        { }

        public void Add(System.Action dele)
        {
            eventDelegate += dele;
        }

        public void Remove(System.Action dele)
        {
            eventDelegate -= dele;
        }

        public override void Trigger()
        {
            base.Trigger();
            if (eventDelegate != null)
            {
                eventDelegate();
            }
        }
    }
}