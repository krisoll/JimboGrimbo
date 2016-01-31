using UnityEngine;
using System.Collections;

public class Cutscene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    private void LoadLevel(string nivel)
    {
        Application.LoadLevel(nivel);
    }
}
