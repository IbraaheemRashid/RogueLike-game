using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

	public GameObject enemyPrefab;

	enum gridSpace { empty, floor, wall };
	gridSpace[,] grid;
	int roomHeight, roomWidth;
	Vector2 roomSizeWorldUnits = new Vector2(30, 30);
	float UnitsinCell = 1;
	struct walker
	{
		public Vector2 dir;
		public Vector2 pos;
	}
	List<walker> walkers;
	float chanceWalkerChangeDir = 0.65f, chanceWalkerSpawn = 0.15f;
	float chanceWalkerDestroy = 0.01f;
	int maxWalkers = 15;
	float percentToFill = 0.35f;
	public GameObject wallObj, floorObj;
	// Start is called before the first frame update
	void Start()
	{
		Setup();
		CreateFloors();
		CreateWalls();

		SpawnLevel();
		SpawnObjects();



	}

	void Setup()
	{
		//find grid size
		roomHeight = Mathf.RoundToInt(roomSizeWorldUnits.x / UnitsinCell);
		roomWidth = Mathf.RoundToInt(roomSizeWorldUnits.y / UnitsinCell);
		//create grid
		grid = new gridSpace[roomWidth, roomHeight];
		//set grid's default state
		for (int x = 0; x < roomWidth - 1; x++)
		{
			for (int y = 0; y < roomHeight - 1; y++)
			{
				//make every cell "empty"
				grid[x, y] = gridSpace.empty;
			}
		}
		//set first walker
		//init list
		walkers = new List<walker>();
		//create a walker 
		walker newWalker = new walker();
		newWalker.dir = RandomDirection();
		//find center of grid
		Vector2 spawnPos = new Vector2(Mathf.RoundToInt(roomWidth / 2.0f),
										Mathf.RoundToInt(roomHeight / 2.0f));
		newWalker.pos = spawnPos;
		//add walker to list
		walkers.Add(newWalker);
	}
	void CreateFloors()
	{
		int iterations = 0;//loop will not run forever
		do
		{
			//create floor at position of every walker
			foreach (walker myWalker in walkers)
			{
				grid[(int)myWalker.pos.x, (int)myWalker.pos.y] = gridSpace.floor;
			}
			//chance: destroy walker
			int numberChecks = walkers.Count; //might modify count while in this loop
			for (int i = 0; i < numberChecks; i++)
			{
				//only if its not the only one, and at a low chance
				if (Random.value < chanceWalkerDestroy && walkers.Count > 1)
				{
					walkers.RemoveAt(i);
					break; //only destroy one per iteration
				}
			}
			//chance: walker pick new direction
			for (int i = 0; i < walkers.Count; i++)
			{
				if (Random.value < chanceWalkerChangeDir)
				{
					walker thisWalker = walkers[i];
					thisWalker.dir = RandomDirection();
					walkers[i] = thisWalker;
				}
			}
			//chance: spawn new walker
			numberChecks = walkers.Count; //might modify count while in this loop
			for (int i = 0; i < numberChecks; i++)
			{
				//only if # of walkers < max, and at a low chance
				if (Random.value < chanceWalkerSpawn && walkers.Count < maxWalkers)
				{
					//create a walker 
					walker newWalker = new walker();
					newWalker.dir = RandomDirection();
					newWalker.pos = walkers[i].pos;
					walkers.Add(newWalker);
				}
			}
			//move walkers
			for (int i = 0; i < walkers.Count; i++)
			{
				walker thisWalker = walkers[i];
				thisWalker.pos += thisWalker.dir;
				walkers[i] = thisWalker;
			}
			//avoid boarder of grid
			for (int i = 0; i < walkers.Count; i++)
			{
				walker thisWalker = walkers[i];
				//clamp x,y to leave a 1 space boarder: leave room for walls
				thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, 1, roomWidth - 2);
				thisWalker.pos.y = Mathf.Clamp(thisWalker.pos.y, 1, roomHeight - 2);
				walkers[i] = thisWalker;
			}
			//check to exit loop
			if ((float)NumberOfFloors() / (float)grid.Length > percentToFill)
			{
				break;
			}
			iterations++;
		} while (iterations < 100000);
	}
	void CreateWalls()
	{
		//loop though every grid space
		for (int x = 0; x < roomWidth - 1; x++)
		{
			for (int y = 0; y < roomHeight - 1; y++)
			{
				//if theres a floor, check the spaces around it
				if (grid[x, y] == gridSpace.floor)
				{
					//if any surrounding spaces are empty, place a wall
					if (grid[x, y + 1] == gridSpace.empty)
					{
						grid[x, y + 1] = gridSpace.wall;
					}
					if (grid[x, y - 1] == gridSpace.empty)
					{
						grid[x, y - 1] = gridSpace.wall;
					}
					if (grid[x + 1, y] == gridSpace.empty)
					{
						grid[x + 1, y] = gridSpace.wall;
					}
					if (grid[x - 1, y] == gridSpace.empty)
					{
						grid[x - 1, y] = gridSpace.wall;
					}
				}
			}
		}
	}

	void SpawnLevel()
	{
		for (int x = 0; x < roomWidth; x++)
		{
			for (int y = 0; y < roomHeight; y++)
			{
				switch (grid[x, y])
				{
					case gridSpace.empty:
						break;
					case gridSpace.floor:
						Spawn(x, y, floorObj);
						break;
					case gridSpace.wall:
						Spawn(x, y, wallObj);
						break;
				}
			}
		}
	}
	Vector2 RandomDirection()
	{
		//pick random int between 0 and 3
		int choice = Mathf.FloorToInt(Random.value * 3.99f);
		//use that int to chose a direction
		switch (choice)
		{
			case 0:
				return Vector2.down;
			case 1:
				return Vector2.left;
			case 2:
				return Vector2.up;
			default:
				return Vector2.right;
		}
	}
	int NumberOfFloors()
	{
		int count = 0;
		foreach (gridSpace space in grid)
		{
			if (space == gridSpace.floor)
			{
				count++;
			}
		}
		return count;
	}
	void Spawn(float x, float y, GameObject toSpawn)
	{
		//find the position to spawn
		Vector2 offset = roomSizeWorldUnits / 2.0f;
		Vector2 spawnPos = new Vector2(x, y) * UnitsinCell - offset;
		//spawn object
		Instantiate(toSpawn, spawnPos, Quaternion.identity);

	}

	public float chanceToSpawnBase;
	public float chanceToSpawnFast;
	public GameObject fastPrefab;
	public GameObject potionPrefab;
	public float chanceToSpawnPot;


	void SpawnObjects()
	{

		for (int x = 0; x < roomWidth; x++)
		{
			for (int y = 0; y < roomHeight; y++)
			{
				if (grid[x, y] == gridSpace.floor &&
				   Random.value < chanceToSpawnBase)
				{
					Spawn(x, y, enemyPrefab);

				}
			}
		}
		for (int x = 0; x < roomWidth; x++)
		{
			for (int y = 0; y < roomHeight; y++)
			{
				if (grid[x, y] == gridSpace.floor &&
				   Random.value < chanceToSpawnFast)
				{
					Spawn(x, y, fastPrefab);

				}
			}
		}
		for (int x = 0; x < roomWidth; x++)
		{
			for (int y = 0; y < roomHeight; y++)
			{
				if (grid[x, y] == gridSpace.floor &&
				   Random.value < chanceToSpawnPot)
				{
					Spawn(x, y, potionPrefab);

				}
			}
		}
	}
}
		//_floorTiles = GameObject.FindGameObjectsWithTag("floorTile");
		//_randomValidFloorTile = _floorTiles[Random.Range(0, _floorTiles.Length)];
		//Instantiate(voidPrefab, new Vector3(_randomValidFloorTile.transform.position.x, _randomValidFloorTile.transform.position.y, 0f), Quaternion.identity);










	


