using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePooler : MonoBehaviour
{
    private List<GameObject> terrainTiles = new List<GameObject>();
    public GameObject terrainTilePrefab;
    public int amountOfTerrainTiles;

    public static TilePooler instance;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < amountOfTerrainTiles; i++)
        {
            GameObject tile = (GameObject)Instantiate(terrainTilePrefab);
            tile.SetActive(false);
            terrainTiles.Add(tile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < terrainTiles.Count; i++)
        {
            if (!terrainTiles[i].activeInHierarchy)
            {
                return terrainTiles[i];
            }
        }  
        return null;
    }
}
