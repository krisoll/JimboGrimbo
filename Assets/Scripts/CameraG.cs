using UnityEngine;
using System.Collections;

public class CameraG : MonoBehaviour {
    public float velocity;
    public float damping;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = Vector3.Slerp(transform.position, new Vector3(Manager.gManager.player.transform.position.x, Manager.gManager.player.transform.position.y, transform.position.z), velocity * Time.deltaTime * (damping / 10));
	}
}
