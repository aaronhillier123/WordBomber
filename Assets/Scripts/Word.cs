using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word {

	public List<Letter> Letters = new List<Letter>();
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public Word subWord(int startNdx, int len){
		Word myWord = new Word ();
		for (int i = startNdx; i < (startNdx + len); ++i) {
			myWord.Letters.Add (Letters [i]);
		}
		return myWord;
	}

	public string getWord(){
		string s = "";
		foreach (Letter lt in Letters) {
			s += lt.value;
		}
		return s;
	}




}
