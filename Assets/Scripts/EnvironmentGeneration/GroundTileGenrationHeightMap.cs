using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Malee.List;

public class GroundTileGenrationHeightMap : MonoBehaviour
{
    [Tooltip("How many tiles offset outside the camera boungs")]
    [SerializeField] private int offset = 2;

    [Space]
    [Tooltip("Decides the smallest step possible a height can change")]
    [SerializeField] private int stepRange = 10;
    //[Tooltip("The height at which everything leans towards")]
    //[SerializeField] private int defaultheight = 50;
    [Tooltip("Decides the strength towards the center. The lower the more strong.")]
    //Max has to be lower than half of the max height
    [Range(1, 49)]
    [SerializeField] private int chanceOfHighY = 5;

    [Space]
    [Reorderable]
    public BiomeList biomeList;
    //public List<Biome> biomes = new List<Biome>();
    [SerializeField] private int maxHeightBiome = 100;

    [Space]
    //[SerializeField] private Joystick movementJoystick;

    [SerializeField] private Transform groundTileParent;
    [SerializeField] private Transform terrainObjectsParent;
    [SerializeField] private Transform nPCParent;
    private Camera cam;
    private float camHorizontalExtend;
    private float camVerticalExtend;
    //Var to hold how many tiles can span across width
    private int tilesFitWidth;
    private int tilesFitHeight;

    private float tileXYSize;

    private void Awake()
    {
        //initialise
        cam = Camera.main;
    }
    private void Start()
    {
        //USES FIRST TILE FOUND IN BIOME LIST FOR TILE SIZE
        //! Considers all tiles the same size !
        SpriteRenderer sRenderer = biomeList[0].whole.GetComponent<SpriteRenderer>();
        tileXYSize = sRenderer.sprite.bounds.size.x * sRenderer.gameObject.transform.localScale.x;

        //Find how tiles fit across and up camera view to stop player from seeing void
        camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;//Calculates falf horizontal width of camera
        camVerticalExtend = cam.orthographicSize;//Calculates half vertical width of camera
        tilesFitWidth = Mathf.CeilToInt(camHorizontalExtend * 2 / tileXYSize) + offset * 2; //Calculates amounts of tiles to fill horizontal
        tilesFitHeight = Mathf.CeilToInt(camVerticalExtend * 2 / tileXYSize) + offset * 2; //Calculates amounts of tiles to fill vertical

        SpawnTerrain();
    }

    public List<Column> columnTiles = new List<Column>();

    private void Update()
    {
        LoadNewTerrain();
        //Records movement of player
        moveVertical = Input.GetAxis("Horizontal");;
        moveHorizontal = Input.GetAxis("Vertical");
    }

    public void SpawnTerrain()
    {
        //Decides where the first tile is placed bases of camera and tile dimensions and offset
        //Equal to bottom left
        Vector3 firstTilePos = new Vector3(cam.transform.position.x - camHorizontalExtend - (tileXYSize * (offset - 0.5f)), cam.transform.position.y - camVerticalExtend - (tileXYSize * (offset - 0.5f)), 0);

        //Makes to variables to store tiles to the left and above
        int leftTileHeight = 0;
        int belowTileHeight = 0;

        //Loops through all the column
        for (int x = 0; x < tilesFitWidth; x++)
        {
            //Creates var to store the currrent list of tiles in
            Column currentColumn = new Column();

            //Loops through all rows
            for (int y = 0; y < tilesFitHeight; y++)
            {
                //Randomly chooses first tiles height
                int thisTileHeight = Mathf.RoundToInt(UnityEngine.Random.Range(0, maxHeightBiome));

                //Check if column is not first column
                if (x != 0)
                {
                    //If so then:
                    //Set left tile variable
                    leftTileHeight = columnTiles[x - 1].rowTiles[y].GetComponent<Tile>().tileHeight;
                    if (y != 0)
                    {
                        belowTileHeight = currentColumn.rowTiles[y - 1].GetComponent<Tile>().tileHeight;
                        //Then properly chooses tile
                        thisTileHeight = TileHeightDecider(leftTileHeight, belowTileHeight);
                    }
                    else
                    {
                        //Then properly chooses tile
                        thisTileHeight = TileHeightDecider(leftTileHeight, -1);
                    }

                }
                else
                {
                    if (y != 0)
                    {
                        belowTileHeight = currentColumn.rowTiles[y - 1].GetComponent<Tile>().tileHeight;
                        //Then properly chooses tile
                        thisTileHeight = TileHeightDecider(belowTileHeight, -1);
                    }
                }

                //Finds the position of the current tile
                Vector3 thisTilePos = firstTilePos + new Vector3(x * tileXYSize, y * tileXYSize, 0);

                //Instantiates the current tile
                GameObject tile = Instantiate(TileBiome(thisTileHeight), thisTilePos, Quaternion.identity, groundTileParent);

                //Adds tile to this column
                currentColumn.rowTiles.Add(tile);
            }

            //Adds column to the columns list
            columnTiles.Add(currentColumn);
        }

        //Makes variables to help figure where the camera will be when its seeing the
        //edge of the terrain
        topTileYPos = columnTiles[0].rowTiles[columnTiles[0].rowTiles.Count - 1].transform.position.y;
        edgeVisiblePositionTop = (topTileYPos + tileXYSize / 2) - camVerticalExtend;
        bottomTileYPos = columnTiles[0].rowTiles[0].transform.position.y;
        edgeVisiblePositionBottom = (bottomTileYPos - tileXYSize / 2) + camVerticalExtend;
        rightTileXPos = columnTiles[columnTiles.Count - 1].rowTiles[0].transform.position.x;
        edgeVisiblePositionRight = (rightTileXPos + tileXYSize / 2) - camHorizontalExtend;
        leftTileXPos = columnTiles[0].rowTiles[0].transform.position.x;
        edgeVisiblePositionLeft = (leftTileXPos - tileXYSize / 2) + camHorizontalExtend;
    }

    private float moveVertical;
    private float moveHorizontal;
    private float topTileYPos;
    private float edgeVisiblePositionTop;
    private float bottomTileYPos;
    private float edgeVisiblePositionBottom;
    private float rightTileXPos;
    private float edgeVisiblePositionRight;
    private float leftTileXPos;
    private float edgeVisiblePositionLeft;
    void LoadNewTerrain()
    {
        //If player moving up
        if (moveVertical > 0)
        {
            //Check if camera position is close to seeing the right edge
            if (cam.transform.position.y >= edgeVisiblePositionTop - tileXYSize * offset)
            {
                InstantiateNewRow(1);
            }
        }
        else if (moveVertical < 0) //Check if player moving down
        {
            //Check if camera position is close to seeing the right edge
            if (cam.transform.position.y <= edgeVisiblePositionBottom + tileXYSize * offset)
            {
                InstantiateNewRow(-1);
            }
        }
        //If player moving to the right
        if (moveHorizontal > 0)
        {
            //Check if camera position is close to seeing the right edge
            if (cam.transform.position.x >= edgeVisiblePositionRight - tileXYSize * offset)
            {
                InstantiateNewColumn(1);
            }
        }
        else if (moveHorizontal < 0)
        {
            //Check if camera position is close to seeing the right edge
            if (cam.transform.position.x <= edgeVisiblePositionLeft + tileXYSize * offset)
            {
                InstantiateNewColumn(-1);
            }
        }
    }
    void InstantiateNewColumn(int leftOrRight)
    {
        //If making column to the right
        if (leftOrRight > 0)
        {
            DestroyColumn(-1);

            //Need position to set the tiles position
            Vector3 columnStart = columnTiles[columnTiles.Count - 1].rowTiles[0].transform.position + new Vector3(tileXYSize, 0, 0);

            //Creates var to store the currrent list of tiles in
            Column currentColumn = new Column();

            //Loops through all rows
            for (int y = 0; y < tilesFitHeight; y++)
            {
                //Sets height to zero by default
                int thisTileHeight = 0;

                int leftTileHeight = 0;
                int belowTileHeight = 0;

                //Set left tile to same row but column to left
                leftTileHeight = columnTiles[columnTiles.Count - 1].rowTiles[y].GetComponent<Tile>().tileHeight;

                //Check if row is not first row
                if (y != 0)
                {
                    //If so then:
                    //Set below tile to same column but row below
                    belowTileHeight = currentColumn.rowTiles[y - 1].GetComponent<Tile>().tileHeight;
                    thisTileHeight = TileHeightDecider(leftTileHeight, belowTileHeight);
                }
                else
                {
                    thisTileHeight = TileHeightDecider(leftTileHeight, -1);
                }
                //Finds the position of the current tile
                Vector3 thisTilePos = columnStart + new Vector3(0, y * tileXYSize, 0);

                //Instantiates the current tile
                GameObject tile = Instantiate(TileBiome(thisTileHeight), thisTilePos, Quaternion.identity, groundTileParent);

                TileSpecialObject(thisTilePos, thisTileHeight, tile);

                //Adds tile to this column
                currentColumn.rowTiles.Add(tile);
            }
            //Adds column to the columns list
            columnTiles.Add(currentColumn);

            rightTileXPos = columnTiles[columnTiles.Count - 1].rowTiles[0].transform.position.x;
            edgeVisiblePositionRight = (rightTileXPos + tileXYSize / 2) - camHorizontalExtend;
        }
        else if (leftOrRight < 0) //If making column to the left
        {
            DestroyColumn(1); //Destroy most right column

            //Workout out x position of column
            Vector3 columnStart = columnTiles[0].rowTiles[0].transform.position + new Vector3(-tileXYSize, 0, 0);

            //Creates var to store the currrent list of tiles in
            Column currentColumn = new Column();

            //Loops through all rows
            for (int y = 0; y < tilesFitHeight; y++)
            {
                //Randomly chooses first biome
                int thisTileHeight = 0;

                int rightTileHeight = 0;
                int belowTileHeight = 0;

                //Set right tile to same row but column to right
                rightTileHeight = columnTiles[0].rowTiles[y].GetComponent<Tile>().tileHeight;

                //Check if row is not first row
                if (y != 0)
                {
                    //If so then:
                    //Set below tile to same column but row below
                    belowTileHeight = currentColumn.rowTiles[y - 1].GetComponent<Tile>().tileHeight;
                    thisTileHeight = TileHeightDecider(rightTileHeight, belowTileHeight);
                }
                else
                {
                    thisTileHeight = TileHeightDecider(rightTileHeight, -1);
                }

                //Finds the position of the current tile
                Vector3 thisTilePos = columnStart + new Vector3(0, y * tileXYSize, 0);

                //Instantiates the current tile
                GameObject tile = Instantiate(TileBiome(thisTileHeight), thisTilePos, Quaternion.identity, groundTileParent);

                TileSpecialObject(thisTilePos, thisTileHeight, tile);

                //Adds tile to this column
                currentColumn.rowTiles.Add(tile);
            }
            //Adds column to the columns list
            columnTiles.Insert(0, currentColumn);

            leftTileXPos = columnTiles[0].rowTiles[0].transform.position.x;
            edgeVisiblePositionLeft = (leftTileXPos - tileXYSize / 2) + camHorizontalExtend;
        }
    }
    void InstantiateNewRow(int topOrBottom)
    {
        //If making row on top
        if (topOrBottom > 0)
        {
            DestroyRow(-1);

            //Workout out the first position of the row
            Vector3 RowStart = columnTiles[0].rowTiles[columnTiles[0].rowTiles.Count - 1].transform.position + new Vector3(0, tileXYSize, 0);

            //Loops through all rows
            for (int x = 0; x < tilesFitWidth; x++)
            {
                //Randomly chooses first biome
                int thisTileHeight = 0;

                int leftTileHeight = 0;
                int belowTileHeight = 0;

                belowTileHeight = columnTiles[x].rowTiles[columnTiles[x].rowTiles.Count - 1].GetComponent<Tile>().tileHeight;

                //Check if row is not first row
                if (x != 0)
                {
                    //If so then:
                    //Set left tile to same row but column to left
                    leftTileHeight = columnTiles[x - 1].rowTiles[columnTiles[x - 1].rowTiles.Count - 1].GetComponent<Tile>().tileHeight;
                    thisTileHeight = TileHeightDecider(leftTileHeight, belowTileHeight);
                }
                else
                {
                    thisTileHeight = TileHeightDecider(belowTileHeight, -1);
                }

                //Finds the position of the current tile
                Vector3 thisTilePos = RowStart + new Vector3(x * tileXYSize, 0, 0);

                //Instantiates the current tile
                GameObject tile = Instantiate(TileBiome(thisTileHeight), thisTilePos, Quaternion.identity, groundTileParent);

                TileSpecialObject(thisTilePos, thisTileHeight, tile);

                //Adds tile to this column
                columnTiles[x].rowTiles.Add(tile);
            }
            //Does these to reset variables for new row positions
            topTileYPos = columnTiles[0].rowTiles[columnTiles[0].rowTiles.Count - 1].transform.position.y;
            edgeVisiblePositionTop = (topTileYPos + tileXYSize / 2) - camVerticalExtend;
        }
        else if (topOrBottom < 0) //If making row below
        {
            DestroyRow(1);

            //Workout out the first position of the row
            Vector3 RowStart = columnTiles[0].rowTiles[0].transform.position + new Vector3(0, -tileXYSize, 0);

            //Loops through all rows
            for (int x = 0; x < tilesFitWidth; x++)
            {
                //Randomly chooses first biome
                int thisTileHeight = 0;

                int leftTileHeight = 0;
                int aboveTileHeight = columnTiles[x].rowTiles[0].GetComponent<Tile>().tileHeight; ;

                //Check if row is not first row
                if (x != 0)
                {
                    //If so then:
                    //Set left tile to same row but column to left
                    leftTileHeight = columnTiles[x - 1].rowTiles[columnTiles[x - 1].rowTiles.Count - 1].GetComponent<Tile>().tileHeight;
                    thisTileHeight = TileHeightDecider(leftTileHeight, aboveTileHeight);
                }
                else
                {
                    thisTileHeight = TileHeightDecider(aboveTileHeight, -1);
                }

                //Finds the position of the current tile
                Vector3 thisTilePos = RowStart + new Vector3(x * tileXYSize, 0, 0);

                //Instantiates the current tile
                GameObject tile = Instantiate(TileBiome(thisTileHeight), thisTilePos, Quaternion.identity, groundTileParent);

                TileSpecialObject(thisTilePos, thisTileHeight, tile);

                //Adds tile to this column
                columnTiles[x].rowTiles.Insert(0, tile);
            }
            bottomTileYPos = columnTiles[0].rowTiles[0].transform.position.y;
            edgeVisiblePositionBottom = (bottomTileYPos - tileXYSize / 2) + camVerticalExtend;
        }
    }
    void DestroyColumn(int leftOrRight)
    {
        if (leftOrRight < 0)
        {
            //Delete column to the most left
            for (int y = 0; y < tilesFitHeight; y++)
            {
                //Delete terrain tile object
                //Destroy(columnTiles[0].rowTiles[y].GetComponent<Tile>().linkedObject);

                Destroy(columnTiles[0].rowTiles[y]);
            }
            columnTiles.RemoveAt(0);
        }
        else if (leftOrRight > 0)
        {
            //Delete column to the most right
            for (int y = 0; y < tilesFitHeight; y++)
            {
                //Delete terrain tile object
                //Destroy(columnTiles[columnTiles.Count - 1].rowTiles[y].GetComponent<Tile>().linkedObject);

                Destroy(columnTiles[columnTiles.Count - 1].rowTiles[y]);
            }
            columnTiles.RemoveAt(columnTiles.Count - 1);
        }
    }
    void DestroyRow(int topOrBottom)
    {
        if (topOrBottom < 0)
        {
            //Cyle through each column
            for (int x = 0; x < tilesFitWidth; x++)
            {
                //Delete terrain tile object
                //Destroy(columnTiles[x].rowTiles[0].GetComponent<Tile>().linkedObject);

                //Delete bottom tile on each column
                Destroy(columnTiles[x].rowTiles[0]);

                //Remove from columns list
                columnTiles[x].rowTiles.RemoveAt(0);
            }
        }
        else if (topOrBottom > 0)
        {
            //Cyle through each column
            for (int x = 0; x < tilesFitWidth; x++)
            {
                //Delete terrain tile object
                //Destroy(columnTiles[x].rowTiles[columnTiles[x].rowTiles.Count - 1].GetComponent<Tile>().linkedObject);

                //Delete top tile on each column
                Destroy(columnTiles[x].rowTiles[columnTiles[x].rowTiles.Count - 1]);

                //Remove from column tiles variable
                columnTiles[x].rowTiles.RemoveAt(columnTiles[x].rowTiles.Count - 1);
            }
        }
    }
    private int TileHeightDecider(int height1, int height2)
    {
        //default value = 0
        int lowestPossibleHeight = 0;
        //default value = 0
        int highestPossibleHeight = 100;

        float average = height2 != -1 ? (height1 + height2) / 2 : height1;

        if (average >= maxHeightBiome - stepRange)
        {
            //If bigger than biggest clip
            highestPossibleHeight = maxHeightBiome;
            lowestPossibleHeight = Mathf.RoundToInt(average - 2 * stepRange);
        }
        else if (average <= stepRange)
        {
            //If smaller than smallest clip
            lowestPossibleHeight = 0;
            highestPossibleHeight = Mathf.RoundToInt(average + 2 * stepRange);
        }
        else
        {
            //If height anywhere between

            //x = chance
            //y = currentAverageHeight
            //Gradient between two points (-111.11)
            //y = -111.11x
            //x = y/-111.11
            float graphGradient = -(maxHeightBiome / ((maxHeightBiome - chanceOfHighY * 2f) / maxHeightBiome));
            float graphUpYIntercept = (1 - (chanceOfHighY / maxHeightBiome)) * graphGradient;

            float graphDownYIntercept = (chanceOfHighY / maxHeightBiome) * graphGradient;

            //graph point depending what the height is
            float chanceGoingDown = (average + graphDownYIntercept) / graphGradient;
            float chanceGoingUp = (average + graphUpYIntercept) / graphGradient;

            Debug.Log("Terrain Max Height: " + maxHeightBiome + ", Graph Max Height: " + (maxHeightBiome - chanceOfHighY * 2) + " = " + graphGradient);
            Debug.Log(chanceGoingDown.ToString() + "<" + chanceGoingUp.ToString());

            //Set the lower height
            lowestPossibleHeight = Mathf.RoundToInt(average + (stepRange * chanceGoingDown));
            //Set the higher height
            highestPossibleHeight = Mathf.RoundToInt(average + (stepRange * chanceGoingUp));
        }

        //Random between the possible height numbers (lower inclusive and higher inclusive)
        int tileHeight = Mathf.RoundToInt(UnityEngine.Random.Range(lowestPossibleHeight, highestPossibleHeight));
        Debug.Log(lowestPossibleHeight + " - " + tileHeight + " - " + highestPossibleHeight);
        return tileHeight;
    }

    private GameObject TileBiome(int height)
    {
        GameObject tile;
        int biome = 0;
        for (biome = 0; biome < biomeList.Count; biome++)
        {
            //If first biome set start to 0
            //Otherwise set it the the last biomeList end
            int biomeStartHeight = biomeList[biome].startingHeight;
            int biomeEnd = biome <= biomeList.Count - 2 ? biomeList[biome + 1].startingHeight : maxHeightBiome;

            if (height >= biomeStartHeight && height < biomeEnd)
            {
                tile = biomeList[biome].whole;
                tile.GetComponent<Tile>().tileHeight = height;
                return tile;
            }
        }
        Debug.LogError("A biome is missing in the biomeList list! Tried to get biome[" + biome + "]");
        return biomeList[0].whole;
    }
    public void TileSpecialObject(Vector2 pos, int height, GameObject linkedTile)
    {
        Debug.Log(pos + "  " + height + "  " + linkedTile);
        //Create a terrain object variable
        BiomeTerrainListObject terrainObject = null;

        for (int biome = 0; biome < biomeList.Count; biome++)
        {
            if (biome < biomeList.Count - 1)
            {
                if (height >= biomeList[biome].startingHeight && height < biomeList[biome + 1].startingHeight)
                {
                    if (biomeList[biome].biomeTerrainObjects.Count != 0)
                    {
                        //Select a random terrain object in that biome
                        int amountBiomeObjects = biomeList[biome].biomeTerrainObjects.Count;
                        terrainObject = biomeList[biome].biomeTerrainObjects[UnityEngine.Random.Range(0, amountBiomeObjects)];
                    }
                }
            }
            else
            {
                if (biomeList[biome].biomeTerrainObjects.Count != 0)
                {
                    //Select a random terrain object in that biome
                    int amountBiomeObjects = biomeList[biome].biomeTerrainObjects.Count;
                    terrainObject = biomeList[biome].biomeTerrainObjects[UnityEngine.Random.Range(0, amountBiomeObjects)];
                }
            }
        }
        if (terrainObject != null)
        {
            //Do random to check if should spawn depending on objects spawn chance
            float randomChance = UnityEngine.Random.Range(0, 1000);
            Debug.Log(randomChance + " out of " + terrainObject.chancePercentage);
            if (randomChance < terrainObject.chancePercentage)
            {
                //Spawn Object
                InstantiateTerrainObject(pos, terrainObject.terrainObjectPrefab, linkedTile);
            }
        }
    }
    public void InstantiateTerrainObject(Vector3 position, GameObject obj, GameObject linkedTile)
    {
        GameObject terrainGameobject = Instantiate(obj, position, Quaternion.identity, terrainObjectsParent);
        //linkedTile.GetComponent<Tile>().linkedObject = terrainGameobject;
    }

    public void DeleteTerrain ()
    {
        for (int col = 0; col < tilesFitWidth; col++)
        {
            for (int row = 0; row < tilesFitHeight; row++)
            {
                Destroy(columnTiles[col].rowTiles[row].gameObject);
            }
        }
        columnTiles.Clear();
    }
    void TileInstantiate ()
    {
        GameObject tile = TilePooler.instance.GetPooledObject();
        if (tile != null)
        {
            //tile.transform.position = turret.transform.position;
            //tile.transform.rotation = turret.transform.rotation;
            tile.SetActive(true);
        }
    }
}
[System.Serializable]
public class Column
{
    public List<GameObject> rowTiles = new List<GameObject>();
}
[System.Serializable]
public class BiomeList : ReorderableArray<BiomeListChild> {}
[System.Serializable]
public class BiomeListChild
{
    public string nameOfBiome;
    [Range(0, 100)]
    public int startingHeight;
    [Header("Tile")]
    public GameObject whole;
    public Color tileColour;
    [Header("Biome Objects")]
    [Reorderable]
    public TerrainObjectsList biomeTerrainObjects;
}
[System.Serializable]
public class TerrainObjectsList : ReorderableArray<BiomeTerrainListObject> { }
[System.Serializable]
public class BiomeTerrainListObject
{
    public string terrainObjectName;
    [Range(0, 100)]
    public float chancePercentage;
    public GameObject terrainObjectPrefab;
}
