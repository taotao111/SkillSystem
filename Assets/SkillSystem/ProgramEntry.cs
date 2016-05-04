using UnityEngine;
using System.Collections;

public class ProgramEntry : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameCenter.Instance.Init();
        int count = 5;
        for (int i = 0; i < count; i++)
        {
            GameObject go = new GameObject("enemy_" + i.ToString());
            go.transform.position = new Vector3(-2 * ((float)(count - 1) / 2.0f) + 2 * i, 0, 5);
            Character ch = go.AddComponent<Character>();
            ch.Layer = eCharacterLayer.Monster;
            ch.id = (uint)i;
            GameCenter.Instance.SceneManager.Add(ch);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
