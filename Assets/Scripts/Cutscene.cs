﻿using UnityEngine;
using System.Collections;

public class Cutscene : MonoBehaviour {
    public AudioSource source;
	// Use this for initialization
	void Start () {
	
	}
    private void LoadLevel(string nivel)
    {
        Application.LoadLevel(nivel);
    }
    void playSound(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }
}
