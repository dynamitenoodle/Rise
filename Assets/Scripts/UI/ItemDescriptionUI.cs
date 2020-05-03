using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionUI : MonoBehaviour
{
    //main obj
    public GameObject modifierUI;
    //specifics
    public Text title;
    public Image image;
    public Text description;
    public Text cost;

    public void ShowUI(bool active)
    {
        modifierUI.SetActive(active);
    }

    public void SetUIData(Item item)
    {
        Sprite image = item.obj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        SetUIData(item.name, image, item.description, item.cost);
    }

    public void SetUIData(string title, Sprite image, string description, int cost)
    {
        this.title.text = title;
        this.image.sprite = image;
        this.description.text = description;
        this.cost.text = $"${cost}";
    }

    public void SetCostCanBuy(bool canBuy)
    {
        Color color = (canBuy) ? Color.yellow : Color.red;
        this.cost.color = color;
    }
}
