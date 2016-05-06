using UnityEngine;
using System.Collections;
namespace Code.SkillSystem.Runtime
{
    public enum eTimeEvent
    {
        SummonEvent,
        ActionEvent,
    }
    public class TimeEventBuilder : TBuilder<TimeEvent, TimeLine>
    {

        public TimeEventBuilder() { }

        void Add<T>(eTimeEvent id) where T : TimeEvent, new()
        {
            //Add<T>((uint)id);
        }
    }
}