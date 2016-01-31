using UnityEngine;
using System.Collections;
[System.Serializable]
public class Menu : MonoBehaviour {
    public GameObject pressStart;
    private Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!anim.GetBool("Run")&&Input.GetKeyDown(KeyCode.Return))
        {
            Destroy(pressStart.gameObject);
            anim.SetBool("Run", true);
        }
	}
    private void changeLevel()
    {
        Application.LoadLevel("Intro cutscene");
    }
}
