using UnityEngine;
using System.Collections;

public class DataManager {
    public Code.SkillSystem.Runtime.SkillDB skillDB;
    public Code.SkillSystem.Runtime.SkillSummonDB skillSummonDB;
    public Code.SkillSystem.Runtime.SkillMotionDB skillMotionDB;
    public Code.SkillSystem.Runtime.SkillActionDB skillActionDB;
    public TimeLineDB timelineDB;

    public void Init()
    {        //技能数据加载
        skillDB = new Code.SkillSystem.Runtime.SkillDB("skill_common");
        skillSummonDB = new Code.SkillSystem.Runtime.SkillSummonDB("skill_summon");
        skillMotionDB = new Code.SkillSystem.Runtime.SkillMotionDB("skill_motion");
        skillActionDB = new Code.SkillSystem.Runtime.SkillActionDB("skill_action");

        timelineDB = new TimeLineDB("time_events");
    }
}
