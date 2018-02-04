using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCreation {
	public static MazeLocation CreateMaze(int size)
	{
		if (size <1)
		{
			Debug.LogError("Cannot create a maze smaller than 1.");
		}
		List<MazeLocation> mazeLocations = new List<MazeLocation>();
		MazeLocation root = new MazeLocation(0);
		mazeLocations.Add(root);
		for (int i = 1; i < size; i++)
		{
			MazeLocation newLocation = new MazeLocation(i);
			bool added = false;
			while (!added)
			{
				MazeLocation connectTo = mazeLocations[Random.Range(0, mazeLocations.Count)];
				int addedDirection = connectTo.Add(newLocation);
				added = addedDirection >= 0;
				if (added)
				{
					newLocation.SetConnected(connectTo, OppositeDirection(addedDirection));
					mazeLocations.Add(newLocation);
				}
			}
		}
		int connectionsAdded = 0;
		int iterations = 0;
		while (connectionsAdded < size && iterations < size*2)
		{
			iterations++;
			Shuffle(mazeLocations);
			MazeLocation current = mazeLocations[0];
			bool connected = false;
			for (int addIndex = 0; addIndex < 4 && !connected; addIndex++)
			{
				if (current.connected[addIndex] != null)
				{
					if (addIndex == 3)
					{
						mazeLocations.RemoveAt(0); // Fully connected;
					}
					continue;
				}
				int direction = current.GetIndexDirection(addIndex);
				for (int i = 1; i < mazeLocations.Count && !connected; i++)
				{
					MazeLocation connectTo = mazeLocations[i];
					int oppositeDirection = OppositeDirection(direction);
					if (connectTo.GetConnected(oppositeDirection) == null)
					{
						connectTo.SetConnected(current, oppositeDirection);
						current.SetConnected(connectTo, direction);
						connected = true;
					}
				}
			}
			if (connected)
			{
				connectionsAdded++;
			}
		}			
		return root;
	}

	private static int OppositeDirection(int direction)
	{
		return (direction + 2) % 4;
	}

	private static void Shuffle(List<MazeLocation> list)
	{
		for (int t = 0; t < list.Count; t++)
		{
			MazeLocation tmp = list[t];
			int r = Random.Range(t, list.Count);
			list[t] = list[r];
			list[r] = tmp;
		}
	}
}
