using UnityEngine;
using System.Collections;
[System.Serializable]
public class Menu : MonoBehaviour {
    public GameObject pressStart;
    public AudioSource source;
    public AudioSource music;
    public AudioClip scratch;
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
            music.Stop();
            music.clip = scratch;
            music.loop = false;
            music.Play();
        }
	}
    private void changeLevel()
    {
        Application.LoadLevel("Intro cutscene");
    }
    private void playSound(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }
}
