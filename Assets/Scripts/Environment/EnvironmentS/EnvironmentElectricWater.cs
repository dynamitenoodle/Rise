using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentElectricWater : EnvironmentElement
{

    float startTime;
    float lifeSpan;


    private void Start()
    {
        lifeSpan = Constants.ENVIRONMENT_ELECTRIC_WATER_LIFE_SPAN;
        startTime = Time.time;


        StartCoroutine("UpdateFire");
    }

    public override void EnvironmentAction(EnemyScript enemy)
    {
        enemy.GetHit(Vector3.zero, 0);
        enemy.GetHit(Vector3.zero, 0);
    }

    IEnumerator UpdateFire()
    {
        for (; ; )
        {
            if (Time.time - startTime >= lifeSpan)
            {
                environmentManager.AddWater(this.transform.position, this.transform.localScale.x);
                Destroy(this.gameObject);
            }

            yield return new WaitForSeconds(Constants.ENVIRONMENT_UPDATE_TIME);
        }
    }
}
