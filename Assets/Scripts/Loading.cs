using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Loading : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("WaitLoad", 0.5f);
	}
	void Awake(){
	}
	// Update is called once per frame
	void Update () {
		
	}

	public void WaitLoad(){
		
		SceneManager.LoadScene ("GameMode");
	}
}
