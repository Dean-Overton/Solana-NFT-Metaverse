using System.Collections.Generic;
using UnityEngine;
using Malee.List;

public class GroundTileGenerationPerlin : MonoBehaviour
{
    [Tooltip("How many tiles offset outside the camera boungs")]
    [SerializeField] private int offset = 2;

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
    [SerializeField] private float currentSeed;
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
    void Start()
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

        currentSeed = Random.Range(1, 100000);
        GenerateFirstTiles();
    }
    private void Update()
    {
        LoadingNewTerrain();
        //Records movement of player
        moveVertical = Input.GetAxis("Horizontal");
        moveHorizontal = Input.GetAxis("Vertical");
    }

    //################# TILE GENERATION #################
    List<Column> columnTiles = new List<Column>();
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
    void GenerateFirstTiles ()
    {
        //Decides where the first tile is placed bases of camera and tile dimensions and offset
        //Equal to bottom left
        Vector3 firstTilePos = new Vector3(cam.transform.position.x - camHorizontalExtend - (tileXYSize * (offset - 0.5f)), cam.transform.position.y - camVerticalExtend - (tileXYSize * (offset - 0.5f)), 0);

        //Loops through all the columns from left to right
        for (int x = 0; x < tilesFitWidth; x++)
        {
            //Creates var to store the currrent list of tiles in
            Column currentColumn = new Column();

            //Loops through all rows from top to bottom
            for (int y = 0; y < tilesFitHeight; y++)
            {
                int thisTileHeight = CalculateTileHeight(-1, currentSeed, x, y); //So then we can decide the biome to give the tile

                //Finds the position of the current tile
                Vector3 thisTilePos = firstTilePos + new Vector3(x * tileXYSize, y * tileXYSize, 0);

                //Instantiates the current tile
                GameObject tile = Instantiate(HeightToBiome(thisTileHeight).whole, thisTilePos, Quaternion.identity, groundTileParent);
                Tile tilescript = tile.GetComponent<Tile>();
                tilescript.xChord = x;
                tilescript.yChord = y;
                tilescript.perlinSeed = currentSeed;

                //Adds tile to this column
                currentColumn.rowTiles.Add(tile);
            }

            //Adds column to the columns list
            columnTiles.Add(currentColumn);
        }

        //Makes variables to help figure where the camera will be when its seeing the
        //edge of the terrain
        topTileYPos = columnTiles[0].rowTiles[columnTiles[0].rowTiles.Count-1].transform.position.y;
        edgeVisiblePositionTop = (topTileYPos + tileXYSize / 2) - camVerticalExtend;
        bottomTileYPos = columnTiles[0].rowTiles[0].transform.position.y;
        edgeVisiblePositionBottom = (bottomTileYPos - tileXYSize / 2) + camVerticalExtend;
        rightTileXPos = columnTiles[columnTiles.Count - 1].rowTiles[0].transform.position.x;
        edgeVisiblePositionRight = (rightTileXPos + tileXYSize / 2) - camHorizontalExtend;
        leftTileXPos = columnTiles[0].rowTiles[0].transform.position.x;
        edgeVisiblePositionLeft = (leftTileXPos - tileXYSize / 2) + camHorizontalExtend;
    }
    void LoadingNewTerrain()
    {
        if (cam.velocity.y > 0f) //These if statements are to minimise computing. Checks if player is moving up
        {
            //Check if camera position is close to seeing the top edge
            if (cam.transform.position.y >= edgeVisiblePositionTop - tileXYSize * offset)
            {
                NewRow(true);
            }
        }
        else if (cam.velocity.y < 0f) //Check if player moving down
        {
            //Check if camera position is close to seeing the bottom edge
            if (cam.transform.position.y <= edgeVisiblePositionBottom + tileXYSize * offset)
            {
                NewRow(false);
            }
        }
        if (cam.velocity.x > 0f)
        {
            //Check if camera position is close to seeing the right edge
            if (cam.transform.position.x >= edgeVisiblePositionRight - tileXYSize * offset)
            {
                NewColumn(true);
            }
        }
        else if (cam.velocity.x < 0f)
        {
            //Check if camera position is close to seeing the left edge
            if (cam.transform.position.x <= edgeVisiblePositionLeft + tileXYSize * offset)
            {
                NewColumn(false);
            }
        }
    }
    void NewRow(bool top) //When top = true; instantiate a top edge row
    {
        //If making row on top
        if (top)
        {
            //Loops through all rows
            for (int x = 0; x < tilesFitWidth; x++)
            {
                //Set above tile to same row but column to left
                Tile belowTile = columnTiles[x].rowTiles[columnTiles[x].rowTiles.Count-1].GetComponent<Tile>();
                int belowTileHeight = belowTile.tileHeight;
                float belowTileSeed = belowTile.perlinSeed;
                int belowTilexChord = belowTile.xChord;
                int y = belowTile.yChord + 1;

                //Randomly chooses first biome
                int thisTileHeight = CalculateTileHeight(-1, belowTileSeed, x, y);

                //Finds the position of the current tile
                Vector3 thisTilePos = belowTile.transform.position + new Vector3(0, tileXYSize, 0);

                //Use top row which is out of view and just move it over to save computing time on instantiation
                GameObject tile = columnTiles[x].rowTiles[0];
                tile.transform.position = thisTilePos;
                tile.GetComponent<SpriteRenderer>().color = TileBiomeColour(thisTileHeight);
                Tile tilescript = tile.GetComponent<Tile>();
                DestroyTileObject(tilescript.linkedObject);
                tilescript.xChord = belowTilexChord;
                tilescript.yChord = y;
                tilescript.perlinSeed = belowTileSeed;

                TileSpecialObject(thisTilePos, thisTileHeight, tile);

                //Adds tile to this row
                columnTiles[x].rowTiles.RemoveAt(0);
                columnTiles[x].rowTiles.Add(tile);
            }
            bottomTileYPos = columnTiles[0].rowTiles[0].transform.position.y;
            topTileYPos = columnTiles[0].rowTiles[columnTiles[0].rowTiles.Count - 1].transform.position.y;
            edgeVisiblePositionTop = (topTileYPos + tileXYSize / 2) - camVerticalExtend;
            edgeVisiblePositionBottom = (bottomTileYPos - tileXYSize / 2) + camVerticalExtend;
        }
        else //If making bottom row
        {
            //Loops through all rows
            for (int x = 0; x < tilesFitWidth; x++)
            {
                //Set above tile to same row but column to left
                Tile aboveTile = columnTiles[x].rowTiles[0].GetComponent<Tile>();
                int aboveTileHeight = aboveTile.tileHeight;
                float aboveTileSeed = aboveTile.perlinSeed;
                int aboveTileXChord = aboveTile.xChord;
                
                int y = aboveTile.yChord - 1;

                //Randomly chooses first biome
                int thisTileHeight = CalculateTileHeight(-1, aboveTileSeed, x, y);

                //Finds the position of the current tile
                Vector3 thisTilePos = aboveTile.transform.position - new Vector3(0, tileXYSize, 0);

                //Use top row which is out of view and just move it over to save computing time on instantiation
                GameObject tile = columnTiles[x].rowTiles[columnTiles[x].rowTiles.Count - 1];
                tile.transform.position = thisTilePos;
                tile.GetComponent<SpriteRenderer>().color = TileBiomeColour(thisTileHeight);
                Tile tilescript = tile.GetComponent<Tile>();
                DestroyTileObject(tilescript.linkedObject);
                tilescript.xChord = aboveTileXChord;
                tilescript.yChord = y;
                tilescript.perlinSeed = aboveTileSeed;

                TileSpecialObject(thisTilePos, thisTileHeight, tile);

                //Adds tile to this row
                columnTiles[x].rowTiles.RemoveAt(columnTiles[x].rowTiles.Count - 1);
                columnTiles[x].rowTiles.Insert(0, tile);
            }
            bottomTileYPos = columnTiles[0].rowTiles[0].transform.position.y;
            topTileYPos = columnTiles[0].rowTiles[columnTiles[0].rowTiles.Count - 1].transform.position.y;
            edgeVisiblePositionBottom = (bottomTileYPos - tileXYSize / 2) + camVerticalExtend;
            edgeVisiblePositionTop = (bottomTileYPos - tileXYSize / 2) - camVerticalExtend;
        }
    }
    void NewColumn(bool right)//When right = true; instantiate a right edge column
    {
        if (right)
        {
            //To save computing space we simply move the tiles not in view over
            //Need position to set the tiles position
            Vector3 columnStart = columnTiles[columnTiles.Count - 1].rowTiles[0].transform.position + new Vector3(tileXYSize, 0, 0);

            int x = columnTiles[columnTiles.Count - 1].rowTiles[0].GetComponent<Tile>().xChord + 1;

            //Creates new column object
            Column currentColumn = new Column();

            //Loops through all rows
            for (int y = 0; y < tilesFitHeight; y++)
            {
                //Set left tile to same row but column to left
                Tile leftTile = columnTiles[columnTiles.Count - 1].rowTiles[y].GetComponent<Tile>();
                int leftTileHeight = leftTile.tileHeight;
                float leftTileSeed = leftTile.perlinSeed;
                int yChord = leftTile.yChord;

                //Sets height to zero by default
                int thisTileHeight = CalculateTileHeight(-1, leftTileSeed, x, y); //## This needs to be changed to use the previous tiles height and seed ##

                //Finds the position of the current tile
                Vector3 thisTilePos = columnStart + new Vector3(0, y * tileXYSize, 0);

                //Use left column which is out of view and just move it over to save computing time on instantiation
                GameObject tile = columnTiles[0].rowTiles[y];
                tile.transform.position = thisTilePos;
                tile.GetComponent<SpriteRenderer>().color = TileBiomeColour(thisTileHeight);
                Tile tilescript = tile.GetComponent<Tile>();
                DestroyTileObject(tilescript.linkedObject);
                tilescript.xChord = x;
                tilescript.yChord = yChord;
                tilescript.perlinSeed = leftTileSeed;

                TileSpecialObject(thisTilePos, thisTileHeight, tile);

                //Adds tile to this column
                currentColumn.rowTiles.Add(tile);
            }
            //Replace the left column with the current
            columnTiles.Add(currentColumn);
            columnTiles.RemoveAt(0);

            rightTileXPos = columnStart.x;
            edgeVisiblePositionRight = (rightTileXPos + tileXYSize / 2) - camHorizontalExtend;
            edgeVisiblePositionLeft = (rightTileXPos - tileXYSize / 2) - camHorizontalExtend;
        } else
        {
            //To save computing space we simply move the tiles not in view over
            //Need position to set the tiles position
            Vector3 columnStart = columnTiles[0].rowTiles[0].transform.position - new Vector3(tileXYSize, 0, 0);

            int x = columnTiles[0].rowTiles[0].GetComponent<Tile>().xChord - 1;

            //Creates new column object
            Column currentColumn = new Column();

            //Loops through all rows
            for (int y = 0; y < tilesFitHeight; y++)
            {
                //Set left tile to same row but column to left
                Tile rightTile = columnTiles[0].rowTiles[y].GetComponent<Tile>();
                int rightTileHeight = rightTile.tileHeight;
                float rightTileSeed = rightTile.perlinSeed;
                int yChord = rightTile.yChord;

                //Sets height to zero by default
                int thisTileHeight = CalculateTileHeight(-1, rightTileSeed, x, y); //## This needs to be changed to use the previous tiles height and seed ##

                //Finds the position of the current tile
                Vector3 thisTilePos = columnStart + new Vector3(0, y * tileXYSize, 0);

                //Use left column which is out of view and just move it over to save computing time on instantiation
                GameObject tile = columnTiles[columnTiles.Count-1].rowTiles[y];
                tile.transform.position = thisTilePos;
                tile.GetComponent<SpriteRenderer>().color = TileBiomeColour(thisTileHeight);
                Tile tilescript = tile.GetComponent<Tile>();
                DestroyTileObject(tilescript.linkedObject);
                tilescript.xChord = x;
                tilescript.yChord = yChord;
                tilescript.perlinSeed = rightTileSeed;

                TileSpecialObject(thisTilePos, thisTileHeight, tile);

                //Adds tile to this column
                currentColumn.rowTiles.Add(tile);
            }
            //Replace the left column with the current
            columnTiles.Insert(0, currentColumn);
            columnTiles.RemoveAt(columnTiles.Count - 1);

            leftTileXPos = columnStart.x;
            edgeVisiblePositionLeft = (leftTileXPos - tileXYSize / 2) + camHorizontalExtend;
            edgeVisiblePositionRight = (leftTileXPos + tileXYSize / 2) + camHorizontalExtend;
        }
    }
    int CalculateTileHeight(int startingHeight, float seed, int x, int y)
    {
        if (startingHeight == -1) //Checking if its the first tile to be generated
        {
            float xChord = (float)x / tilesFitWidth * 0.3f + seed; //Replace 1 with a scale value
            float yChord = (float)y / tilesFitHeight * 0.3f + seed; //Replace 1 with a scale value
            float perlin = Mathf.PerlinNoise(xChord, yChord);
            return Mathf.RoundToInt(perlin * maxHeightBiome); //Map perlin noise between 0 and maxHeight
        }
        return 0;
    }
    private BiomeListChild HeightToBiome(int height)
    {
        for (int biome = 0; biome < biomeList.Count; biome++)
        {
            //If first biome set start to 0
            //Otherwise set it the the last biomeList end
            int biomeStartHeight = biomeList[biome].startingHeight;
            int biomeEnd = biome <= biomeList.Count - 2 ? biomeList[biome + 1].startingHeight : maxHeightBiome;

            if (height >= biomeStartHeight && height < biomeEnd)
            {
                return biomeList[biome];
            }
        }
        Debug.LogError("A biome is missing in the biomeList list!");
        return biomeList[0];
    }
    private Color TileBiomeColour(int height)
    {
        for (int biome = 0; biome < biomeList.Count; biome++)
        {
            //If first biome set start to 0
            //Otherwise set it the the last biomeList end
            int biomeStartHeight = biomeList[biome].startingHeight;
            int biomeEnd = biome <= biomeList.Count - 2 ? biomeList[biome + 1].startingHeight : maxHeightBiome;

            if (height >= biomeStartHeight && height < biomeEnd)
            {
                return biomeList[biome].tileColour;
            }
        }
        Debug.LogError("A biome is missing in the biomeList list!");
        return biomeList[0].tileColour;
    }
    public void RespawnTerrain ()
    {
        DeleteTerrain();
        GenerateFirstTiles();
    }
    private void DeleteTerrain()
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

    //##################### TILE OBJECT GENERATION ################################
    public void TileSpecialObject(Vector2 pos, int height, GameObject linkedTile)
    {
        //Create a terrain object variable
        BiomeTerrainListObject terrainObject = null;

        //Allow for Custom objects maybe from quests
        if (importantSpecialObjects.Count>0) { //No point computing this code if there is nothing important that hasnt been spawned
            for (int i = 0; i < importantSpecialObjects.Count; i++)
            {
                KeyValuePair<BiomeTerrainListObject, bool> obj = importantSpecialObjects[i];
                if (obj.Value == false)
                {
                    terrainObject = obj.Key;
                    if (terrainObject.terrainObjectPrefab != null)
                    {
                        //Do random to check if should spawn depending on objects spawn chance
                        float randomChance = Random.Range(0, 100);
                        if (randomChance < terrainObject.chancePercentage)
                        {
                            TileObject tileobj = new TileObject(terrainObject.terrainObjectPrefab, i);
                            //Spawn Object
                            InstantiateTerrainObject(pos, tileobj, linkedTile);

                            importantSpecialObjects[i] = new KeyValuePair<BiomeTerrainListObject, bool>(obj.Key, true);

                            return; //We cant have two objects spawning on the same tile
                        }
                    }
                }
            }
        }

        for (int biome = 0; biome < biomeList.Count; biome++)
        {
            if (height >= biomeList[biome].startingHeight && height < biomeList[biome + 1].startingHeight)
            {
                if (biomeList[biome].biomeTerrainObjects.Count != 0)
                {
                    //Select a random terrain object in that biome
                    int amountBiomeObjects = biomeList[biome].biomeTerrainObjects.Count;
                    terrainObject = biomeList[biome].biomeTerrainObjects[UnityEngine.Random.Range(0, amountBiomeObjects)];
                    if (terrainObject != null)
                    {
                        //Do random to check if should spawn depending on objects spawn chance
                        float randomChance = Random.Range(0, 100);
                        if (randomChance < terrainObject.chancePercentage)
                        {
                            TileObject tileobj = new TileObject(terrainObject.terrainObjectPrefab, -1);

                            //Spawn Object
                            InstantiateTerrainObject(pos, tileobj, linkedTile);
                        }
                    }
                }
            }
        }
    }
    public void InstantiateTerrainObject(Vector3 position, TileObject obj, GameObject linkedTile)
    {
        if (obj == null || linkedTile == null || position == null)
            return;
        GameObject terrainGameobject = Instantiate(obj.obj, position, Quaternion.identity, terrainObjectsParent);
        TileObject tileObj = new TileObject(terrainGameobject, obj.poolId);
        linkedTile.GetComponent<Tile>().linkedObject = tileObj;
    }

    [HideInInspector] //Objects added to this dictionary are responsible for removing themselves otherwise they will keep respawning
    public List<KeyValuePair<BiomeTerrainListObject, bool>> importantSpecialObjects = new List<KeyValuePair<BiomeTerrainListObject, bool>>(); //Object : If its been spawned

    private void DestroyTileObject (TileObject obj)
    {
        if (obj != null)
        {
            if (obj.obj.gameObject != null)
            {
                Destroy(obj.obj);
                return;
            }
            if (obj.poolId >= 0)
            { //Important Object?
                importantSpecialObjects[obj.poolId] = new KeyValuePair<BiomeTerrainListObject, bool>(importantSpecialObjects[obj.poolId].Key, false);
                Debug.Log(importantSpecialObjects[obj.poolId].ToString());
            }
           
        }
    }

    //########################## NPC GENERATION ###################################


}