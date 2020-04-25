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
        SetUIData(item.name, item.image, item.description, item.cost);
    }

    public void SetUIData(string title, string image, string description, int cost)
    {
        this.title.text = title;
        this.image.sprite = Resources.Load<Sprite>(image);
        this.description.text = description;
        this.cost.text = $"${cost}";
    }
}
