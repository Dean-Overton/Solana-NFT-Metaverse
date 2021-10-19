using UnityEngine;
using UnityEngine.UI;

public class ItemPickupTrigger : MonoBehaviour
{
    public ItemObject item;

    public Text itemTextLabel;

    private void Start()
    {
        itemTextLabel.gameObject.SetActive(false);
    }
    public void ItemPickTip(bool on)
    {
        if (on)
        {
            itemTextLabel.text = "Tap To Pickup...";
            itemTextLabel.gameObject.SetActive(true);
        } else
        {
            itemTextLabel.gameObject.SetActive(false);
        }
    }
}
