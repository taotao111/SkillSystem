#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

public class EditorMonoUpdate : MonoBehaviour {

    public delegate void UpdateDele(float elapsed_sec);
    public UpdateDele updateDele;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (updateDele != null)
        {
            updateDele(Time.deltaTime);
        }
	}
}
#endif