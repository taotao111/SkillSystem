using UnityEngine;
using System.Collections;

public class DataManager {
    public Code.SkillSystem.SkillDB skillDB;
    public Code.SkillSystem.SkillSummonDB skillSummonDB;
    public Code.SkillSystem.SkillMotionDB skillMotionDB;
    public Code.SkillSystem.SkillActionDB skillActionDB;
    public TimeLineDB timelineDB;

    public void Init()
    {        //技能数据加载
        skillDB = new Code.SkillSystem.SkillDB("skill_common");
        skillSummonDB = new Code.SkillSystem.SkillSummonDB("skill_summon");
        skillMotionDB = new Code.SkillSystem.SkillMotionDB("skill_motion");
        skillActionDB = new Code.SkillSystem.SkillActionDB("skill_action");

        timelineDB = new TimeLineDB("time_events");
    }
}
