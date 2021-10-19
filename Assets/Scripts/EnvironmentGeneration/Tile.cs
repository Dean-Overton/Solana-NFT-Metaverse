using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileHeight;
    public float perlinSeed;
    public int xChord; public int yChord;
    public TileObject linkedObject;
}
[System.Serializable]
public class TileObject
{
    public GameObject obj;
    public int poolId = -1;

    public TileObject (GameObject obj, int listIndex)
    {
        this.obj = obj;
        poolId = listIndex;
    }
}