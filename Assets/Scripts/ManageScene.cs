﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayGame(){
		SceneManager.LoadScene ("Loading");
	}

	public void GoHome(){
		SceneManager.LoadScene ("MainMenu");

	}
}
