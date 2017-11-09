using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour {

	public Sprite A;
	public Sprite B;
	public Sprite C;
	public Sprite D;
	public Sprite E;
	public Sprite F;
	public Sprite G;
	public Sprite H;
	public Sprite I;
	public Sprite J;
	public Sprite K;
	public Sprite L;
	public Sprite M;
	public Sprite N;
	public Sprite O;
	public Sprite P;
	public Sprite Q;
	public Sprite R;
	public Sprite S;
	public Sprite T;
	public Sprite U;
	public Sprite V;
	public Sprite W;
	public Sprite X;
	public Sprite Y;
	public Sprite Z;

	public int id;
	public char value;
	public bool Placed = false;
	public int row;
	public int col;
	public List<Word> myWords;

	public Letter right;
	public Letter left;
	public Letter up;
	public Letter down;

	public Point myPoint;
	public char letter;

	public Letter(Point p){
		myPoint = p;
	}


	void OnMouseDown(){
		if (myWords.Count > 0) {
			try{
				GameObject.Find ("Grid").GetComponent<Grid> ().RemoveWord (GetLongestWord());
			} catch{
			}
		}

	}

	void Start () {
		myWords = new List<Word> ();
		row = myPoint.row;
		col = myPoint.col;
		id = GenerateLetter ();
		SetSpriteAndValue ();
		InvokeRepeating ("fall", 1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void highlight(){
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (0.5f, .9f, 0.5f);
	}

	public void selectlight(){
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (0.5f, .9f, 0.5f);
	}

	public void unHighLight(){
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f);
	}

	public Word GetLongestWord(){
		Word longest = null;
		int length = 0;
		foreach(Word w in myWords){
			if(w.Letters.Count>length){
				longest = w;
				length = w.Letters.Count;
			}
		}
		return longest;
	}

	public bool isFarLeft(){
		if (left == null) {
			return true;
		} 
		return false;
	}

	public bool isTop(){
		if (up == null) {
			return true;
		}
		return false;
	}

	public void SetSpriteAndValue(){
		SpriteRenderer rend = GetComponent<SpriteRenderer> ();
		switch (id) {
		case 0:
			rend.sprite = A;
			value = 'a';
			break;
		case 1:
			rend.sprite = B;
			value = 'b';
			break;
		case 2:
			rend.sprite = C;
			value = 'c';
			break;
		case 3:
			rend.sprite = D;
			value = 'd';
			break;
		case 4:
			rend.sprite = E;
			value = 'e';
			break;
		case 5:
			rend.sprite = F;
			value = 'f';
			break;
		case 6:
			rend.sprite = G;
			value = 'g';
			break;
		case 7:
			rend.sprite = H;
			value = 'h';
			break;
		case 8:
			rend.sprite = I;
			value = 'i';
			break;
		case 9:
			rend.sprite = J;
			value = 'j';
			break;
		case 10:
			rend.sprite = K;
			value = 'k';
			break;
		case 11:
			rend.sprite = L;
			value = 'l';
			break;
		case 12:
			rend.sprite = M;
			value = 'm';
			break;
		case 13:
			rend.sprite = N;
			value = 'n';
			break;
		case 14:
			rend.sprite = O;
			value = 'o';
			break;
		case 15:
			rend.sprite = P;
			value = 'p';
			break;
		case 16:
			rend.sprite = Q;
			value = 'q';
			break;
		case 17:
			rend.sprite = R;
			value = 'r';
			break;
		case 18:
			rend.sprite = S;
			value = 's';
			break;
		case 19:
			rend.sprite = T;
			value = 't';
			break;
		case 20:
			rend.sprite = U;
			value = 'u';
			break;
		case 21:
			rend.sprite = V;
			value = 'v';
			break;
		case 22:
			rend.sprite = W;
			value = 'w';
			break;
		case 23:
			rend.sprite = X;
			value = 'x';
			break;
		case 24:
			rend.sprite = Y;
			value = 'y';
			break;
		case 25:
			rend.sprite = Z;
			value = 'z';
			break;
		default:
			rend.sprite = A;
			value = 'A';
			break;
		}
	}

	public void SetPoint(Point p){
		
		//set previous points letter to null
		myPoint.setMyLetter('0');
		myPoint.letter = null;
		myPoint.full = false;

		if (left != null) {
			left.right = null;
		}
		if (right != null) {
			right.left = null;
		}
		if (up != null) {
			up.down = null;
		}
		if (down != null) {
			down.up = null;
		}


		//set my point to the new Point
		myPoint = p;

		//set my new letters new rows, cols, and adjacent letters
		row = myPoint.row;
		col = myPoint.col;

		if (myPoint.right != null) {
			right = myPoint.right.letter;
			if (right != null) {
				right.left = this;
			}
		}
		if (myPoint.left != null) {
			left = myPoint.left.letter;
			if (left != null) {
				left.right = this;
			}
		}
		if (myPoint.up != null) {
			up = myPoint.up.letter;
			if (up != null) {
				up.down = this;
			}
		}
		if (myPoint.down != null) {
			down = myPoint.down.letter;
			if (down != null) {
				down.up = this;
			}
		}

		myPoint.setMyLetter (value);
		myPoint.letter = this;
		myPoint.full = true;

		//move letter to point
		gameObject.transform.position = myPoint.getPos ();


		if (myPoint.down==null || down!=null) {
			Land ();
		}

		}

	public void fall(){
		if (Placed == false) {
			if (myPoint.down==null || down!=null) {
				Land ();
			} else {
				SetPoint (myPoint.down);
			}
		}
	}

	public void Land(){
		int HighRow = GameObject.Find ("Grid").GetComponent<Grid> ().rows;
		if (this.row == HighRow-1) {
			Debug.Log ("ENDGAME");
			GameObject.Find ("Grid").GetComponent<Grid> ().endGame ();
		}
		else if (Placed == false) {
			myPoint.full = true;
			Placed = true;

			GameObject.Find ("Grid").GetComponent<Grid> ().createNewLetter ();
		}
	}

	public int GenerateLetter(){
		float rand = Random.Range (0.0f, 100.0f);
		float[] percentages = new float[]{8.4966f, 2.0720f,
			4.5388f, 3.3844f, 11.1607f, 1.8121f, 2.4705f, 
			3.0034f, 7.5448f, 0.1965f, 1.1016f, 5.4893f, 
			3.0129f, 6.6544f, 7.1635f, 3.1671f, 0.1962f, 
			7.5809f, 5.7351f, 6.9509f, 3.6308f, 1.0074f, 
			1.2899f, 0.2902f, 1.7779f, 0.2722f};
		float min;
		float max = 0.0f;
		for (int i = 0; i < 26; ++i) {
			min = max;
			max = max + percentages [i];
			if (rand >= min && rand < max) {
				return i;
			}
		}
		return 1;
	}

	/*
			E	11.1607%	56.88	M	3.0129%	15.36
			A	8.4966%	43.31	H	3.0034%	15.31
			R	7.5809%	38.64	G	2.4705%	12.59
			I	7.5448%	38.45	B	2.0720%	10.56
			O	7.1635%	36.51	F	1.8121%	9.24
			T	6.9509%	35.43	Y	1.7779%	9.06
			N	6.6544%	33.92	W	1.2899%	6.57
			S	5.7351%	29.23	K	1.1016%	5.61
			L	5.4893%	27.98	V	1.0074%	5.13
			C	4.5388%	23.13	X	0.2902%	1.48
			U	3.6308%	18.51	Z	0.2722%	1.39
			D	3.3844%	17.25	J	0.1965%	1.00
			P	3.1671%	16.14	Q	0.1962%	(1)
*/

}
