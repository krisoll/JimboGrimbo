using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
    public int nextLevel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Input.GetAxis("Vertical") > 0.01)
            {
                Application.LoadLevel(nextLevel);
            }
        }
    }
}
