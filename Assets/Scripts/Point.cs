using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point{

	public int id;
	private float xPos;
	private float yPos;
	public Letter letter = null;
	public int row;
	public int col;
	public bool full = false;
	public bool bottom = false;
	private char myLetter = '0';
	public Point left;
	public Point right;
	public Point down;
	public Point up;

	public Point(int id1, float a, float b, int myRow, int myCol){
		id = id1;
		xPos = a;
		yPos = b;
		row = myRow;
		col = myCol;
	}

	public Vector3 getPos(){
		Vector3 pos = new Vector3 (xPos, yPos, 0);
		return pos;
	}

	public char getMyLetter(){
		return myLetter;
	}

	public void setMyLetter(char l){
		myLetter = l;
	}
	// Use this for initialization

}
