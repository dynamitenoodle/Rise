using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    public void SetAbilityPanel(AbilityPanel abilityPanel, int abilityNum)
    {
        abilityPanels[abilityNum]=abilityPanel;
    }

    public void SetAbilityCooldown(int abilityNum, float coolDownTime)
    {
        Debug.Log("RUN");
        abilityPanels[abilityNum].cooldownPanel.SetActive(true);
        StartCoroutine(UpdateAbilityCooldownAnimation(Time.time, coolDownTime, abilityPanels[abilityNum].cooldownPanel));
    }

    IEnumerator UpdateAbilityCooldownAnimation(float startTime, float coolDownTime, GameObject panel)
    {
        bool runCoroutine = true;
        while(runCoroutine)
        {
            Debug.Log("Running coroutine");
            float timeScale = (Time.time - startTime) / coolDownTime;

            if (timeScale > 1)
            {
                Debug.Log("stopping");
                panel.SetActive(false);
                runCoroutine = false;
                yield return null;
            }
            else
            {
                Debug.Log("doing");
                float scale = Mathf.Lerp(1f, 0f, timeScale);
                panel.transform.localScale = new Vector3(1, scale, 0);
                yield return new WaitForSeconds(Constants.UI_ABILITY_COOLDOWN_UPDATE_TIME);
            }
        }
    }
}
