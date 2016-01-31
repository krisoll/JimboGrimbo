using UnityEngine;
using System.Collections;

public class CameraG : MonoBehaviour {
    public float velocity;
    public float damping;
    public float offsetY;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = Vector3.Slerp(transform.position, new Vector3(Manager.gManager.asignedPlayer.transform.position.x, Manager.gManager.asignedPlayer.transform.position.y+offsetY, transform.position.z), velocity * Time.deltaTime * (damping / 10));
	}
}
