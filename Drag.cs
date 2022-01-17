using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public HeroInventory heroInventory;
    public Item item;
    public string ownerItem;
    public int countItem;

    public Image image;
    public Sprite defaultSprite;
    public Text count;

    public GameObject descriptionObj;
    public Text descriptionItem;


    Image img;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData evenData)
    {
        if (ownerItem == "myItem")
        {
            img.color = new Color(66f / 255, 66f / 255, 66f / 255, 100f / 255);
            descriptionObj.SetActive(true);
            descriptionItem.text = item.description;
        }
    }

    public void OnPointerExit(PointerEventData evenData)
    {
        if (ownerItem == "myItem")
        {
            img.color = new Color(255f / 255, 255f / 255, 255f / 255, 100f / 255);
            descriptionObj.SetActive(false);
            descriptionItem.text = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
         if (ownerItem != "")
         {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (ownerItem == "myItem")
                    heroInventory.RemoveItem(this);
            }

            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (ownerItem == "myItem")
                    heroInventory.UseItem(this);

                else if (ownerItem == "DealerItem")
                {
                    heroInventory.Buy(this);
                }
            }
         }
    }
}
