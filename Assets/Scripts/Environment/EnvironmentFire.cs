using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentFire : EnvironmentElement
{
    float startTime;
    float lifeSpan;

    private Vector2 maxScale;
    private Vector2 minScale;

    private void Start()
    {
        lifeSpan = Constants.ENVIRONMENT_FIRE_LIFE_SPAN;
        startTime = Time.time;

        //temporary
        maxScale = this.transform.localScale;
        minScale = maxScale * 0.1f;

        StartCoroutine("UpdateFire");
    }

    public override void EnvironmentAction()
    {
        
    }

    IEnumerator UpdateFire()
    {
        for(;;)
        {
            Vector2 scale = Vector2.Lerp(maxScale, minScale, ((Time.time - startTime) / lifeSpan));
            this.transform.localScale = scale;

            if (Time.time - startTime >= lifeSpan)
            {
                Destroy(this.gameObject);
            }

            yield return new WaitForSeconds(Constants.ENVIRONMENT_UPDATE_TIME);
        }
    }
}
