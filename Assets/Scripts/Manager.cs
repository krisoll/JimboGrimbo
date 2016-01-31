using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
    public static Manager gManager;
    public Grimbo player;
    public GameObject asignedPlayer;
    public bool startCountdown;
    private float cont;
    private float countdowTime;
	// Use this for initialization
	void Awake () {
        startCountdown = false;
        countdowTime = 4;
        cont = 0;
        if (gManager == null)gManager = this;
        else if (gManager != this) Destroy(gameObject);    
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if (startCountdown)
        {
            cont += Time.deltaTime;
            if (cont >= countdowTime)
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
	}
}
