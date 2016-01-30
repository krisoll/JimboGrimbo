using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
    public static Manager gManager;
    public Grimbo player;
    public GameObject asignedPlayer;

	// Use this for initialization
	void Awake () {
        gManager = new Manager();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
