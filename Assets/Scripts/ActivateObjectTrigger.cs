using UnityEngine;
using System.Collections;

public class ActivateObjectTrigger : MonoBehaviour {
    public GameObject[] activate;
	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            for (int i = 0; i < activate.Length; i++)
            {
                activate[i].SetActive(true);
                Destroy(this.gameObject);
            }
        }
    }
}
