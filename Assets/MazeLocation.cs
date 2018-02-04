using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeLocation {

	public MazeLocation[] connected = new MazeLocation[4];
	private int[] indices = new int[] { 0, 1, 2, 3 };
	public const int UP = 0;
	public const int RIGHT = 1;
	public const int DOWN = 2;
	public const int LEFT = 3;
	public int ID;
	public int visited =-1;

	public MazeLocation(int ID)
	{
		this.ID = ID;
		for (int t = 0; t < indices.Length; t++)
		{
			int tmp = indices[t];
			int r = Random.Range(t, indices.Length);
			indices[t] = indices[r];
			indices[r] = tmp;
		}
	}

	public MazeLocation GetConnected(int direction)
	{
		return connected[indices[direction]];
	}

	public void SetConnected(MazeLocation other, int direction)
	{
		if (other == this)
		{
			Debug.Log("why?");
		}
		//Debug.Log(ID + " connects to " + other.ID + " at " + direction);
		connected[indices[direction]] = other;
	}

	public int Add(MazeLocation other)
	{
		if (other == this)
		{
			Debug.Log("why?");
		}
		for (int i = 0; i < connected.Length; i++)
		{
			if (connected[i] == null)
			{
				connected[i] = other;
				//Debug.Log(ID + " connects to " + other.ID + " at " + GetIndexDirection(i));
				return GetIndexDirection(i);
			}
		}
		return -1;
	}

	public int GetIndexDirection(int index)
	{
		for (int direction = 0; direction < indices.Length; direction++)
		{
			if (indices[direction] == index)
			{
				return direction;
			}
		}
		return -1;
	}

	public int GetBitmask()
	{
		int n = connected[indices[UP]] == null ? 0 : 1;
		int w = connected[indices[LEFT]] == null ? 0 : 2;
		int e = connected[indices[RIGHT]] == null ? 0 : 4;
		int s = connected[indices[DOWN]] == null ? 0 : 8;
		string upID = connected[indices[UP]] == null ? "-" : ""+connected[indices[UP]].ID;
		string rightID = connected[indices[RIGHT]] == null ? "-" : "" + connected[indices[RIGHT]].ID;
		string downID = connected[indices[DOWN]] == null ? "-" : "" + connected[indices[DOWN]].ID;
		string leftID = connected[indices[LEFT]] == null ? "-" : "" + connected[indices[LEFT]].ID;

		//Debug.Log(ID + " " + upID + " " + rightID + " " + downID + " " + leftID);
		return n + w + e + s;
	}

}
