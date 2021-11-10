using System.Collections.Generic;
using UnityEngine;

public class JewelCollector : Quest
{
    public BiomeTerrainListObject jewelPrefab;

    private int jewelsCollectedSoFar;
    [SerializeField] private int amountToCollect = 3;

    void Start()
    {
        for (int a = 0; a < amountToCollect; a++)
            FindObjectOfType<GroundTileGenerationPerlin>().importantSpecialObjects.Add(new KeyValuePair<BiomeTerrainListObject, bool>(jewelPrefab, false));

        GameEvents.current.onItemPickup += OnJewelCollected;
    }

    public void OnJewelCollected (ItemObject item)
    {
        if (item.name == "Jewel") //Because we havnet actually checked if the item is a jewel yet
        {
            jewelsCollectedSoFar += 1;

            UpdateQuestProgress((float)jewelsCollectedSoFar / amountToCollect * 100f);

            if (jewelsCollectedSoFar == amountToCollect)
                OnQuestComplete();
        }
    }
    public void OnQuestComplete ()
    {
        GameEvents.current.CompleteQuest();


        Destroy(gameObject);
    }
}
