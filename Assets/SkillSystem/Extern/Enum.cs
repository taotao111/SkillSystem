using UnityEngine;
using System.Collections;

public enum eAudioLayer
{
    BACKGROUND = 0, //背景音效
    UIAUDIO = 1,//UI音效
    EFFECTAUDIO = 2,//效果音效
}

/// <summary>
/// 角色详细阵营
/// </summary>
public enum eCharacterLayer
{
    None = -1,
    Player = 1,
    NetPlayer = 2,
    Monster = 4,
    Neutral = 8,
}

/// <summary>
/// 
/// </summary>
public enum eCharacterExtensiveLayer
{
    Friend = 1,//友方
    Enemy = 2,//敌方
    Neutral = 4,//中立
}

/// <summary>
/// 游戏模式
/// </summary>
public enum eGameMode
{
    PVE,
}
/// <summary>
/// 角色阵营
/// </summary>
public class CharacterLayer
{
    public static eGameMode gameMode = eGameMode.PVE;
    /// <summary>
    /// 当前属于玩家阵营的层
    /// </summary>
    public static int PlayerLayer
    {
        get
        {
            switch (gameMode)
            {
                case eGameMode.PVE:
                    {
                        return (int)eCharacterLayer.Player + (int)eCharacterLayer.NetPlayer;
                    }
            }


            return 0;
        }
    }
    /// <summary>
    /// 当前属于怪物阵营的层
    /// </summary>
    public static int MonsterLayer
    {
        get
        {
            switch (gameMode)
            {
                case eGameMode.PVE:
                    {
                        return (int)eCharacterLayer.Monster;
                    }
            }

            return 0;
        }
    }
    /// <summary>
    /// 当前属于中立阵营的层
    /// </summary>
    public static int NeutralLayer
    {
        get
        {
            switch (gameMode)
            {
                case eGameMode.PVE:
                    {
                        return (int)eCharacterLayer.Neutral;
                    }
            }
            return 0;
        }
    }

    public static int GetTargetLayer(int own_layer, int target_effect_layer)
    {
        int target_layer = 0;

        if ((own_layer & PlayerLayer) != 0)
        {
            if ((target_effect_layer & (int)eCharacterExtensiveLayer.Enemy) != 0)
            {
                target_layer += MonsterLayer;
            }
            if ((target_effect_layer & (int)eCharacterExtensiveLayer.Neutral) != 0)
            {
                target_layer += NeutralLayer;
            }
            if ((target_effect_layer & (int)eCharacterExtensiveLayer.Friend) != 0)
            {
                target_layer += PlayerLayer;
            }
        }

        if ((own_layer & MonsterLayer) != 0)
        {
            if ((target_effect_layer & (int)eCharacterExtensiveLayer.Enemy) != 0)
            {
                target_layer += PlayerLayer;
            }
            if ((target_effect_layer & (int)eCharacterExtensiveLayer.Neutral) != 0)
            {
                target_layer += NeutralLayer;
            }
            if ((target_effect_layer & (int)eCharacterExtensiveLayer.Friend) != 0)
            {
                target_layer += MonsterLayer;
            }
        }

        if ((own_layer & NeutralLayer) != 0)
        {
            if ((target_effect_layer & (int)eCharacterExtensiveLayer.Enemy) != 0)
            {
                target_layer += MonsterLayer;
            }
            if ((target_effect_layer & (int)eCharacterExtensiveLayer.Neutral) != 0)
            {
                target_layer += NeutralLayer;
            }
            if ((target_effect_layer & (int)eCharacterExtensiveLayer.Friend) != 0)
            {
                target_layer += PlayerLayer;
            }
        }

        return target_layer;
    }
    public static int GetTargetLayer(int own_layer, params eCharacterExtensiveLayer[] target_effect_layer)
    {
        int target_layer = 0;
        for (int i = 0; i < target_effect_layer.Length; i++)
        {
            target_layer += (int)target_effect_layer[i];
        }
        return GetTargetLayer(own_layer, target_layer);
    }
    public static int GetTargetLayer(eCharacterLayer own_layer, int target_effect_layer)
    {
        return GetTargetLayer((int)own_layer, target_effect_layer);
    }
    public static int GetTargetLayer(eCharacterLayer own_layer,params eCharacterExtensiveLayer[] target_effect_layer)
    {
        int target_layer = 0;
        for (int i = 0; i < target_effect_layer.Length; i++)
        {
            target_layer += (int)target_effect_layer[i];
        }
        return GetTargetLayer((int)own_layer,target_layer);
    }
}