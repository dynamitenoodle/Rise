using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AbilityPanel
{
    public GameObject panel;
    public GameObject image;
    public GameObject cooldownPanel;
}

public class AbilityUIManager : MonoBehaviour
{
    public AbilityPanel[] abilityPanels;

    public GameObject abilityUpgradeUI;
    public Image abilityUpgradeImage;
    public Text abilityUpgradeTitle;

    public Text goldText;

    public GameObject buttons;

    public RectTransform healthBar;
    public Text healthText;

    private float healthBarMaxWidth;
    private float healthBarMaxPos;

    private void Start()
    {
        UpdateGoldText(0);

        healthBarMaxWidth = healthBar.rect.width;
        healthBarMaxPos = healthBar.rect.x;

    }

    public void SetAbilityPanel(AbilityPanel abilityPanel, int abilityNum)
    {
        abilityPanels[abilityNum]=abilityPanel;
    }

    public void SetAbilityCooldown(int abilityNum, float coolDownTime)
    {
        abilityPanels[abilityNum].cooldownPanel.SetActive(true);
        StartCoroutine(UpdateAbilityCooldownAnimation(Time.time, coolDownTime, abilityPanels[abilityNum].cooldownPanel));
    }

    IEnumerator UpdateAbilityCooldownAnimation(float startTime, float coolDownTime, GameObject panel)
    {
        bool runCoroutine = true;
        while(runCoroutine)
        {
            float timeScale = (Time.time - startTime) / coolDownTime;

            if (timeScale > 1)
            {
                panel.SetActive(false);
                runCoroutine = false;
                yield return null;
            }
            else
            {
                float scale = Mathf.Lerp(1f, 0f, timeScale);
                panel.transform.localScale = new Vector3(1, scale, 0);
                yield return new WaitForSeconds(Constants.UI_ABILITY_COOLDOWN_UPDATE_TIME);
            }
        }
    }

    public void AbilityUpgradeUIActive(bool active)
    {
        abilityUpgradeUI.SetActive(active);
    }

    public void AbilityUpgradeUISetData(string title, string image)
    {
        if (abilityUpgradeUI.activeSelf) { return; }

        abilityUpgradeTitle.text = title;
        abilityUpgradeImage.sprite = Resources.Load<Sprite>(image);

        AbilityUpgradeUIActive(true);

        for (int i = 0; i < abilityPanels.Length; i++)
        {
            buttons.transform.GetChild(i).gameObject.SetActive(abilityPanels[i].panel.activeInHierarchy);
        }

    }

    public void UpdateGoldText(int gold)
    {
        goldText.text = $"${gold}";
    }

    public void UpdateHealth(int health, int healthMax)
    {
        healthText.text = $"{health} / {healthMax}";
        float percentage = ((float)health / (float)healthMax);
        Debug.Log($"health: {health} / healthMax: {healthMax}");
        float x = healthBarMaxPos - (healthBarMaxPos * percentage);
        float width = healthBarMaxWidth * percentage;

        Debug.Log($"percentage: {percentage}, x: {x}, width: {width}");

        Vector2 pos = new Vector2(x, healthBar.localPosition.y);
        Vector2 size = new Vector2(width, healthBar.sizeDelta.y);

        healthBar.localPosition = pos;
        healthBar.sizeDelta = size;
    }
}
