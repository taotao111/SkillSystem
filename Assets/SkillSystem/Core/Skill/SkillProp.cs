using UnityEngine;
using System.Collections;

public class SkillProp  {
#if UNITY_EDITOR
    public enum DrawSkillType
    {
        Skill,
        Summon,
        Action,
    }
    public static DrawSkillType right_draw = DrawSkillType.Skill;

    public delegate void CreateSummonDele(uint owner,uint id);
    public static CreateSummonDele CreateSummon;
    
    //移除
    public delegate void RemoveSummonDele(uint owner, uint id);
    public static RemoveSummonDele RemoveSummon;
#endif
}
