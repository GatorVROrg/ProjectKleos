using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightingEffect : MonoBehaviour
{
    private Vector3 target;
    private LineRenderer lineRend;
    private float arcLength = 0.05f;
    private float arcVariation = 0.5f;
    private float inaccuracy = 0.1f;
    private float timeOfZap = 0.5f;
    private float zapTimer;
    //private LightningTrace lightTrace;

    void Start()
    {
        lineRend = gameObject.GetComponent<LineRenderer>();
        zapTimer = 0;
        lineRend.SetVertexCount(1);
        //lightTrace = gameObject.GetComponent<LightningTrace>();
    }

    void Update()
    {
        if (zapTimer > 0)
        {
            Vector3 lastPoint = transform.position;
            int i = 1;
            lineRend.SetPosition(0, transform.position);//make the origin of the LR the same as the transform
            while (Vector3.Distance(target, lastPoint) > 0.5f)
            {//was the last arc not touching the target?
                lineRend.SetVertexCount(i + 1);//then we need a new vertex in our line renderer
                Vector3 fwd = target - lastPoint;//gives the direction to our target from the end of the last arc
                fwd.Normalize();//makes the direction to scale
                fwd = Randomize(fwd, inaccuracy);//we don't want a straight line to the target though
                fwd *= Random.Range(arcLength * arcVariation, arcLength);//nature is never too uniform
                fwd += lastPoint;//point + distance * direction = new point. this is where our new arc ends
                lineRend.SetPosition(i, fwd);//this tells the line renderer where to draw to
                i++;
                lastPoint = fwd;//so we know where we are starting from for the next arc
            }
            lineRend.SetVertexCount(i + 1);
            lineRend.SetPosition(i, target);
            //lightTrace.TraceLight(gameObject.transform.position, target.transform.position);
            zapTimer = zapTimer - Time.deltaTime;
        }
        else
            lineRend.SetVertexCount(1);

    }

    private Vector3 Randomize(Vector3 newVector, float devation)
    {
        newVector += new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * devation;
        newVector.Normalize();
        return newVector;
    }

    public void ZapTarget(Vector3 newTarget)
    {
        //print ("zap called");
        target = newTarget;
        zapTimer = timeOfZap;


    }
}
