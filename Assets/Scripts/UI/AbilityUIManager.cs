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

    private void Start()
    {
        UpdateGoldText(0);
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
}
