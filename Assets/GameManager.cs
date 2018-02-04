using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private MazeLocation current;
	public TileDisplay tileDisplayPrefab;
	public TileDisplay[] tileDisplays = new TileDisplay[5];
	private Vector2[] relativePositions = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
	public float inputDelay;
	private float elapsedSinceInput;
	private int state = 0;
	private const int INPUT = 0;
	private const int ANIMATION = 1;
	private Vector2 destination = Vector2.zero;
	public float smoothTime = 0.3F;
	public float maxSpeed = 0.3F;
	private Vector2 velocity = Vector2.zero;
	private int visistedCount = 1;
	public int mazeSize;
	private int currentMazeSize;
	public Text display;
	public GameObject victoryDisplay;

	void Start () {
		for (int i = 0; i < tileDisplays.Length; i++)
		{
			tileDisplays[i] = Instantiate(tileDisplayPrefab);
		}
		Setup();
	}

	public void Setup()
	{
		victoryDisplay.SetActive(false);
		display.text = "WASD or arrows to move.\nFind a path through every room without visiting the same room twice.\nThe maze does not change, but it may be a little tricky to navigate.";
		visistedCount = 1;
		currentMazeSize = mazeSize;
		Camera.main.transform.position = Vector2.zero;
		SetCurrent(MazeCreation.CreateMaze(currentMazeSize));
		state = INPUT;
		DisplayLocation();
		elapsedSinceInput = inputDelay;
	}
	
	void Update () {
		if (elapsedSinceInput < inputDelay)
		{
			elapsedSinceInput += Time.deltaTime;
			return;
		}
		if (state == INPUT)
		{
			DoInput();
		} else if (state == ANIMATION)
		{
			DoAnimation();
		}
	}

	private void DoInput()
	{
		if (current.GetConnected(MazeLocation.UP) != null && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
		{
			SetCurrent(current.GetConnected(MazeLocation.UP));
			destination = (Vector2) Camera.main.transform.position + Vector2.up;
			state = ANIMATION;
		}
		else if (current.GetConnected(MazeLocation.RIGHT) != null && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)))
		{
			SetCurrent(current.GetConnected(MazeLocation.RIGHT));
			destination = (Vector2)Camera.main.transform.position + Vector2.right;
			state = ANIMATION;
		}
		else if (current.GetConnected(MazeLocation.DOWN) != null && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
		{
			SetCurrent(current.GetConnected(MazeLocation.DOWN));
			destination = (Vector2)Camera.main.transform.position + Vector2.down;
			state = ANIMATION;
		}
		else if (current.GetConnected(MazeLocation.LEFT) != null && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)))
		{
			SetCurrent(current.GetConnected(MazeLocation.LEFT));
			destination = (Vector2)Camera.main.transform.position + Vector2.left;
			state = ANIMATION;
		}
	}

	private void DoAnimation()
	{

		Camera.main.transform.position = Vector2.SmoothDamp(Camera.main.transform.position, destination, ref velocity, smoothTime, maxSpeed, Time.deltaTime);
		if (((Vector2)Camera.main.transform.position - destination).magnitude < 0.01)
		{
			state = INPUT;
			DisplayLocation();
		}
	}


	private void DisplayLocation()
	{
		SetTileDisplay(tileDisplays[4], current, Vector2.zero);
		for (int i = 0; i < 4; i++)
		{
			if (current.GetConnected(i) != null)
			{
				SetTileDisplay(tileDisplays[i], current.GetConnected(i), relativePositions[i]);
			} else
			{
				tileDisplays[i].gameObject.SetActive(false);
			}
		}
	}

	private void SetTileDisplay(TileDisplay tile, MazeLocation mazeLocation, Vector2 position)
	{
		if (mazeLocation.visited >= visistedCount)
		{
			mazeLocation.visited = -1;
		}
		tile.gameObject.SetActive(true);
		tile.SetSprite(mazeLocation.GetBitmask());
		string display = mazeLocation.visited >= 0 ? mazeLocation.visited + "" : "";
		tile.GetComponentInChildren<Text>().text = display;
		tile.transform.position = (Vector2) Camera.main.transform.position + position;
		tile.transform.position = tile.transform.position + Vector3.forward*2;
	}

	private void SetCurrent(MazeLocation set)
	{
		current = set;
		if (current.visited < 0 || current.visited >= visistedCount )
		{
			current.visited = visistedCount++;
			if (visistedCount > currentMazeSize)
			{
				display.text = "Victory!\nYou've successfully navigated a non euclidiean maze of size " + currentMazeSize + "!";
				victoryDisplay.SetActive(true);
			}
		} else
		{
			visistedCount = current.visited + 1;
			Debug.Log(visistedCount);
		}
	}

	public void SetMazeSize(Slider slider)
	{
		mazeSize = (int)slider.value;
	}
}
