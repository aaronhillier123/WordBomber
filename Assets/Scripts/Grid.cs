using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour {

	public static Dictionary<string, int> Dic = new Dictionary<string, int>();
	public GameObject LetterObject;
	public GameObject EndGameOb;
	public int rows = 9;
	int cols = 9;
	public Letter activeLetter;
	public List<List<Point>> grid = new List<List<Point>>();
	public List<Point> allPoints = new List<Point>();
	public List<Word> words = new List<Word>();
	int prevCol = 0;
	public TextAsset wordsList;
	private string[] mylist;
	public Word selectedWord;
	private int wordNdx = 0;
	public List<Letter> allLetters = new List<Letter> ();
	public int score = 0;
	public GameObject scoreOb;
	public GameObject diffWordsOb;
	public List<string> differentWords = new List<string> ();

	void Start () {
		mylist = wordsList.text.Split ("\n" [0]);
		Dic.Clear ();
		for (int i = 0; i < mylist.Length; ++i) {
			Dic.Add (mylist [i], mylist [i].Length);
		}
		createGrid ();
		createNewLetter();
	}
	
	void Update () {
		if (activeLetter != null) {
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				moveLetterLeft ();
			}
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				moveLetterRight ();
			}
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				moveLetterDown ();
			}
			if (Input.GetKeyDown (KeyCode.Space)) {
				DropLetter ();
			}
			if (Input.GetKeyDown (KeyCode.A)) {
				selectNextWord ();
			}
			if (Input.GetKeyDown (KeyCode.S)) {
				if (selectedWord != null) {
					RemoveWord (selectedWord);
				}
			}
		}
	}

	public void createGrid(){
		for (int i = 0; i < cols; ++i) {
			List<Point> newRow = new List<Point> ();
			grid.Add (newRow);
			for (int j = 0; j < rows; ++j) {
				float xpos = (((float)i) / 2.0f) + 0.8f;
				float ypos = (((float)j) / 2.0f) + 3.0f;
				Point point = new Point ((i * rows) + j, xpos, ypos, j, i);
				grid [i].Add (point);
				allPoints.Add (point);
			}
		}
		assignGrid ();
	}

	public void assignGrid(){
		foreach(Point p in allPoints){
			//if (p.row == 0) {
			//	p.bottom = true;
			//}
			p.left = getThisPoint (p.row, p.col - 1);
			p.right = getThisPoint(p.row, p.col+1);
			p.up = getThisPoint (p.row + 1, p.col);
			p.down = getThisPoint (p.row - 1, p.col);
		}
	}

	public Point getThisPoint(int row, int col){
		if (row >= 0 && col >= 0 && row < rows && col < cols) {
			return grid [col] [row];
		}
		return null;
	}
		
	public void createNewLetter(){
		Point p = getPoint (prevCol, rows - 1);
		Vector3 newPos = p.getPos ();

		GameObject newLetter = Instantiate (LetterObject, newPos, Quaternion.identity, gameObject.transform) as GameObject;
		Letter newL = newLetter.GetComponent<Letter> ();

		newL.myPoint = p;
		newL.row = p.row;
		newL.col = p.col;
		if (newL.myPoint.right != null) {
			newL.right = newL.myPoint.right.letter;
		}
		if (newL.myPoint.left != null) {
			newL.left = newL.myPoint.left.letter;
		}
		if (newL.myPoint.up != null) {
			newL.up = newL.myPoint.up.letter;
		}
		if (newL.myPoint.down != null) {
			newL.down = newL.myPoint.down.letter;
		}

		allLetters.Add (newL);
		setLetterAsActive (newL);
	}

	public void setLetterAsActive(Letter l){
		activeLetter = l;
	}

	public void moveLetterLeft(){
		Letter l = activeLetter;
		if(l.myPoint.left!=null){
			l.SetPoint (l.myPoint.left);
		}
	}

	public void moveLetterRight(){
		Letter l = activeLetter;
		if (l.myPoint.right != null) {
			l.SetPoint (l.myPoint.right);
		}
	}
		
	public void moveLetterDown(){
		Letter l = activeLetter;
		if (l.myPoint.down != null) {
			l.SetPoint (l.myPoint.down);
		}
	}

	public void moveLetterDown(Letter l){
		if (l.myPoint.down != null) {
			l.SetPoint (l.myPoint.down);
		}
	}
		
	public Point findLowestEmptyPoint(int colu){
		Point currentLow = null;
		int lowNdx = rows;
		foreach (Point p in allPoints) {
			if (p.col == colu) {
				if (p.row < lowNdx && p.full==false) {
					lowNdx = p.row;
					currentLow = p;
				}
			}
		}
		return currentLow;
	}

	public void DropLetter(){
		Letter l = activeLetter;
		Point p = findLowestEmptyPoint (activeLetter.myPoint.col);
		prevCol = l.col;
		l.SetPoint (p);
		ResetList ();
	}

	public void selectNextWord(){
		if (selectedWord != null) {
			highlightWord (selectedWord);
		}
		incrementNdx ();
		if (words.Count > 0) {
			selectedWord = words [wordNdx];
			selectlightWord (selectedWord);
		}
	}

	public void incrementNdx(){
		if(wordNdx == words.Count-1){
			wordNdx = 0;
		} else {
			wordNdx++;
		}
	}

	public void selectWord(Word w){
		selectedWord = w;
		selectlightWord (w);
	}

	public Word findHAffected(Point p){
		Word affected = new Word();
		Point start = p;
		while (start.col>0 && start.left.getMyLetter () != '0') {
			start = start.left;
		}
		while (start.col<(cols-1) && start.right.getMyLetter () != '0') {
			
			affected.Letters.Add (start.letter);
			start = start.right;
		}
		affected.Letters.Add (start.letter);
		return affected;
	}

	public Word findVAffected(Point p){
		Word affected = new Word();
		Point start = p;
		while (start.row<rows-1 && start.up.getMyLetter () != '0') {
			start = start.up;
		}
		while (start.row>0 && start.down.getMyLetter () != '0') {
			
			affected.Letters.Add (start.letter);
			start = start.down;
		}
		affected.Letters.Add (start.letter);
		return affected;
	}
				
	public Point getPoint(int col, int row){
		
		foreach (Point p in allPoints) {
			if (p.col == col && p.row == row) {
				return p;
			}
		}
		Debug.Log ("Point doesnt exist");
		return null;
	}
		
	public void highlightWord(Word a){
		foreach(Letter lt in a.Letters){
			lt.highlight ();
		}
	}

	public void selectlightWord(Word a){
		foreach(Letter lt in a.Letters){
			lt.selectlight ();
		}
	}

	public bool isNewWord(List<Word> a, Word b){
		foreach (Word w in a) {
			if (w.getWord () == b.getWord ()) {
				return false;
			}
		}
		return true;
	}
		
			
	public List<string> SplitSubStrings(string a){
		List<string> myStrings = new List<string> ();
		string current = "";
		for (int i = 0; i < a.Length; ++i) {
			if (a [i] != '0') {
				current = current + a [i];
			} else {
				myStrings.Add (current);
				current = "";
			}
		}
		return myStrings;
	}

	public List<Word> allSubStrings(Word a){
		List<Word> allWords = new List<Word> ();
		for (int length = 1; length < a.Letters.Count; length++)
		{
			for (int start = 0; start <= a.Letters.Count - length; start++)
			{
				Word substring = a.subWord(start, length);
				allWords.Add(substring);
			}
		}
		allWords.Add (a);
		return allWords;
	}
		

	public void endGame(){
		Dic.Clear ();
		grid.Clear ();
		CancelInvoke ();
		activeLetter.CancelInvoke ();
		GameObject EndGameObj = Instantiate (EndGameOb) as GameObject;
		EndGameObj.transform.SetParent (GameObject.Find ("Canvas").transform, false);
		EndGameObj.transform.Find ("EndGamePanel").Find ("EndScore").GetComponent<Text> ().text = "Score: " + score;
		int highscore = PlayerPrefs.GetInt ("HighScore");
		if (score > highscore) {
			PlayerPrefs.SetInt ("HighScore", score);
		}
		highscore = PlayerPrefs.GetInt ("HighScore");
		EndGameObj.transform.Find ("EndGamePanel").Find ("HighScore").GetComponent<Text> ().text = "HighScore: " + highscore;
	}

	public void RemoveWord(Word a){
		score += GetScore (a.getWord ());
		Debug.Log("Score was " + GetScore(a.getWord()));
		updateScore ();

		if(!differentWords.Contains(a.getWord())){
			differentWords.Add(a.getWord());
		}
		updateWords ();
		List<Letter> removeThese = new List<Letter> ();
		selectedWord = null;
		//REMOVE EACH LETTER AND REMOVE LETTER FROM THEIR POINTS
		foreach (Letter lt in a.Letters) {

			//set adjacent letters adjacent letters
			if (lt.left != null) {
				lt.left.right = null;
			}
			if (lt.right != null) {
				lt.right.left = null;
			}
			if (lt.up != null) {
				lt.up.down = null;
			}
			if (lt.down != null) {
				lt.down.up = null;
			}

			removeThese.Add (lt);

			Point b = lt.myPoint;
			b.full = false;
			b.letter = null;
			allLetters.Remove (lt);
			Destroy (lt.gameObject);
		}
		wordNdx = 0;
		SynchGrid ();
		ResetList ();
	}

	public void SynchGrid(){
		foreach (Letter lt in allLetters) {
			while (lt.down == null && lt!=activeLetter && lt.row != 0) {
				moveLetterDown (lt);
			}
		}
	}

	public void setList(){
		List<Word> allCurrentWords = findAllHoriWords();
		allCurrentWords.AddRange (findAllVertWords ());

		List<Word> allCurrentSubWords = new List<Word> ();
		foreach (Word w in allCurrentWords) {
			allCurrentSubWords.AddRange (allSubStrings (w));
		}
		FindIfWord (allCurrentSubWords);
	}

	public void ResetList(){
		words.Clear ();
		UnHighLightAllLetters ();
		List<Word> allCurrentWords = findAllHoriWords();
		allCurrentWords.AddRange (findAllVertWords ());
		List<Word> allCurrentSubWords = new List<Word> ();
		foreach (Word w in allCurrentWords) {
			allCurrentSubWords.AddRange (allSubStrings (w));
		}
		FindIfWord (allCurrentSubWords);
	}

	public List<Word> findAllHoriWords(){
		List<Word> allWordCases = new List<Word> ();
		List<Letter> startLetters = new List<Letter> ();
		foreach (Letter lt in allLetters) {
			if (lt.isFarLeft()) {
				startLetters.Add (lt);
			}
		}

		foreach (Letter lt in startLetters) {
			Word myWord = findWord (lt);
			allWordCases.Add (myWord);
		}

		return allWordCases;
	}

	public List<Word> findAllVertWords(){
		List<Word> allWordCases = new List<Word> ();
		List<Letter> startLetters = new List<Letter> ();
		foreach (Letter lt in allLetters) {
			if (lt.isTop()) {
				startLetters.Add (lt);
			}
		}

		foreach (Letter lt in startLetters) {
			Word myWord = findDownWord (lt);
			allWordCases.Add (myWord);
		}

		return allWordCases;
	}

	public Word findWord(Letter l){
		Letter a = l;
		Word myWord = new Word();

		while(a.right!=null){
			myWord.Letters.Add(a);
			a = a.right;
		}
		myWord.Letters.Add(a);

		return myWord;
	}

	public Word findDownWord(Letter l){
		Letter a = l;
		Word myWord = new Word();

		while(a.down!=null){
			myWord.Letters.Add(a);
			a = a.down;
		}
		myWord.Letters.Add(a);

		return myWord;
	}

	public void UnHighLightAllLetters(){
		foreach(Point p in allPoints){
			if(p.letter!=null){
				p.letter.unHighLight ();
				if (p.letter.myWords != null) {
					p.letter.myWords.Clear ();
				}
			}
		}
	}

	public void FindIfWord(List<Word> ACSW){
		foreach (Word w in ACSW) {
			int len = -1;
			if (Dic.TryGetValue (w.getWord (), out len)) {
				if (len > 2 && isNewWord (words, w)) {
					if (words.Count == 0) {
						foreach (Letter l in w.Letters) {
							l.myWords.Add (w);
						}
						words.Add (w);
						selectWord (w);
					} else {
						foreach (Letter l in w.Letters) {
							l.myWords.Add (w);
						}
						words.Add (w);
						highlightWord (w);
						selectWord (selectedWord);
					}
				}
			}
		}
	}

	public List<Letter> getRow(int row){
		List<Letter> thisRow = new List<Letter> ();
		foreach (Point p in allPoints) {
			if (p.row == row) {
				if (p.letter != null && p.letter!=activeLetter) {
					thisRow.Add (p.letter);
				}
			}
		}
		return thisRow;
	}

	public List<Letter> getColumn(int col){
		List<Letter> thisCol = new List<Letter> ();
		foreach (Point p in allPoints) {
			if (p.col == col) {
				thisCol.Add (p.letter);
			}
		}
		return thisCol;
	}

	public void updateScore(){
		scoreOb.GetComponent<Text> ().text = "Score: " + score;
	}

	public void updateWords(){
		diffWordsOb.GetComponent<Text> ().text = "Unique Words: " + differentWords.Count;
	}

	public int GetScore(string a){
		int points = 0;
		foreach (char b in a) {
			switch (b) {
			case 'a':
				points = points + 1;
				break;
			case 'b':
				points = points + 3;
				break;
			case 'c':
				points = points + 3;
				break;
			case 'd':
				points = points + 2;
				break;
			case 'e':
				points = points + 1;
				break;
			case 'f':
				points = points + 4;
				break;
			case 'g':
				points = points + 2;
				break;
			case 'h':
				points = points + 4;
				break;
			case 'i':
				points = points + 1;
				break;
			case 'j':
				points = points + 8;
				break;
			case 'k':
				points = points + 5;
				break;
			case 'l':
				points = points + 1;
				break;
			case 'm':
				points = points + 3;
				break;
			case 'n':
				points = points + 1;
				break;
			case 'o':
				points = points + 1;
				break;
			case 'p':
				points = points + 3;
				break;
			case 'q':
				points = points + 10;
				break;
			case 'r':
				points = points + 1;
				break;
			case 's':
				points = points + 1;
				break;
			case 't':
				points = points + 1;
				break;
			case 'u':
				points = points + 1;
				break;
			case 'v':
				points = points + 4;
				break;
			case 'w':
				points = points + 4;
				break;
			case 'x':
				points = points + 12;
				break;
			case 'y':
				points = points + 4;
				break;
			case 'z':
				points = points + 10;
				break;
			default:
				break;
			}

		}
		int multiplyer = 1;
		int count = a.Length;
		multiplyer = (count / 3) + 1;
		return points * multiplyer;
	}
}
