using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 主要负责场景角色管理
/// </summary>
public class SceneManager {
    private Dictionary<eCharacterLayer, Dictionary<uint, Character>> m_LayerCharacters = new Dictionary<eCharacterLayer, Dictionary<uint, Character>>();
    private Dictionary<uint, Character> m_Characters = new Dictionary<uint, Character>();
    /// <summary>
    /// 场景中的角色
    /// 包含主角，怪物，npc等等
    /// </summary>
    public Dictionary<uint, Character> senceCharacters
    {
        get
        {
            return m_Characters;
        }
    }
    
    public void Init() { }

    /// <summary>
    /// 获得对应层次的角色
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public Dictionary<uint,Character> GetCharactersDic(params eCharacterLayer[] layers)
    {
        Dictionary<uint, Character> get_value = new Dictionary<uint, Character>();

        int layer_value = 0;

        for (int i = 0; i < layers.Length; i++)
        {
            layer_value += (int)layers[i];
        }

        foreach (var it in m_Characters)
        {
            if (((int)it.Value.Layer & layer_value) != 0)
            {
                get_value.Add(it.Value.id, it.Value);
            }
        }

        return get_value;
    }
    public Dictionary<uint,Character> GetCharactersDic(int layer_value)
    {
        Dictionary<uint, Character> get_value = new Dictionary<uint, Character>();

        foreach (var it in m_Characters)
        {
            if (((int)it.Value.Layer & layer_value) != 0)
            {
                get_value.Add(it.Value.id, it.Value);
            }
        }

        return get_value;
    }

    public List<Character> GetCharactersList(params eCharacterLayer[] layers)
    {
        List<Character> get_value = new List<Character>();

        int layer_value = 0;

        for (int i = 0; i < layers.Length; i++)
        {
            layer_value += (int)layers[i];
        }

        foreach (var it in m_Characters)
        {
            if (((int)it.Value.Layer & layer_value) != 0)
            {
                get_value.Add(it.Value);
            }
        }

        return get_value;
    }
    public List<Character> GetCharactersList(int layer_value)
    {
        List<Character> get_value = new List<Character>();

        foreach (var it in m_Characters)
        {
            if (((int)it.Value.Layer & layer_value) != 0)
            {
                get_value.Add(it.Value);
            }
        }

        return get_value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="character"></param>
    public void Add(Character character)
    {
        m_Characters.Add(character.id, character);
        if (!m_LayerCharacters.ContainsKey(character.Layer))
        {
            m_LayerCharacters.Add(character.Layer, new Dictionary<uint, Character>());
        }

        m_LayerCharacters[character.Layer].Add(character.id, character);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="character"></param>
    public void Remove(Character character)
    {
        if (m_LayerCharacters.ContainsKey(character.Layer))
        {
            m_LayerCharacters[character.Layer].Remove(character.id);
        }
        Remove(character.id);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    public void Remove(uint id)
    {
        if (m_Characters.ContainsKey(id))
        {
            m_Characters.Remove(id);
        }
    }
}
