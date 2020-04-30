using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public GameObject arrow;
    private GameObject trader;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find(Constants.GAMEOBJECT_NAME_PLAYER);        
    }

    public void ShowTraderArrow(GameObject trader)
    {
        this.trader = trader;
        arrow.SetActive(true);
        StartCoroutine(UpdateArrow());
    }

    public void HideTraderArrow()
    {
        trader = null;
        arrow.SetActive(false);
        StopCoroutine(UpdateArrow());
    }

    IEnumerator UpdateArrow()
    {
        do
        {
            Vector2 direction = (trader.transform.position - player.transform.position);

            if (direction.magnitude < Constants.MINIMAP_ARROW_MIN_DISTANCE)
                arrow.SetActive(false);
            else
                arrow.SetActive(true);

            direction = direction.normalized;
            float rotation = ((Mathf.Atan2(direction.y, direction.x) / Mathf.PI) * 180.0f) - 90.0f;

            Quaternion rotationQuaternion = Quaternion.Euler(0, 0, rotation);
            arrow.transform.rotation = rotationQuaternion;

            yield return new WaitForSeconds(Constants.MINIMAP_ARROW_UPDATE_TIME);
        } while (trader != null);
    }
}
