using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop: MonoBehaviour
{
    public ItemObject item;

    [Header("Item Drop Settings")]
    [Tooltip("Can the item be picked up?")]
    public bool pickupable = false;
    [Tooltip("A duration of -1 is an infinite duration item drop. Used most commonly for quest objects.")]
    public float duration = -1f;
}
