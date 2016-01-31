using UnityEngine;
using System.Collections;

public class Cutscene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    private void LoadLevel(int level)
    {
        Application.LoadLevel(level);
    }
}
