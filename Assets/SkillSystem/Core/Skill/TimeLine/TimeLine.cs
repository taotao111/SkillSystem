using UnityEngine;
using System.Collections.Generic;

public class TimeLine
{
    public uint id;
    public float time;
    public float curTime;
    public List<TimeEvent> timeEvents = new List<TimeEvent>();
    public TimeLine(uint id)
    {
        this.id = id;
    }
    public void Reset()
    {
        for (int i = timeEvents.Count - 1; i >= 0; i--)
        {
            timeEvents[i].Reset();
        }
        curTime = time;
    }

    public void Update(float elapsed_sec)
    {
        for (int i = timeEvents.Count - 1; i >= 0;i-- )
        {
            //timeEvents[i].Update(elapsed_sec);
            //if(timeEvents[i].IsExpire)
            //{
            //    timeEvents.RemoveAt(i);
            //}
            
            if (!timeEvents[i].IsExpire)
            {
                timeEvents[i].Update(elapsed_sec);
            }
        }

        curTime -= elapsed_sec;
        if (curTime <= 0) { Reset(); }

    }

    public void AddEvent(TimeEvent time_event)
    {
        time_event.holder = this;
        timeEvents.Add(time_event);
    }

    public void AddEvent(List<TimeEvent> time_event)
    {
        for (int i = 0; i < time_event.Count; i++)
        {
            AddEvent(time_event[i]);
        }
    }

    public virtual object GetHolder()
    {
        return null;
    }
}
public class TimeLine<H> : TimeLine
{
    public H holder;

    public TimeLine(uint id,H holder,float time):base(id)
    {
        this.time = time;
        this.holder = holder;
        Reset();
    }

    public void SetTime(float time)
    {
        this.time = time;
        Reset();
    }
    public override object GetHolder()
    {
        return holder;
    }
#if UNITY_EDITOR
    public void Draw(int count_pre_second,int cur_count)
    {
        for (int i = 0; i < timeEvents.Count; i++ )
        {

        }
    }
#endif
}

