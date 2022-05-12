using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XL_SpellGrenade : XL_Spells
{
    
    [SerializeField] private XL_SpellGrenadeAttributesSO grenadeAttributes;

    private GameObject grenade;
    [SerializeField] private float startingHeight;
    private float[] startingVelocity;

    [SerializeField] private LineRenderer lineRenderer;
    private Vector3[] curvePoints = new Vector3[6];
    private Vector3[] curvePointsWS = new Vector3[6];
    [SerializeField] private SpriteRenderer circle;

    private void Awake()
    {
        grenadeAttributes.Initialize();
    }

    public void Start()
    {
        //Debug.Log("throwing Distance : "+grenadeAttributes.throwingDistance);
        //curvePoints = XL_Utilities.GenerateCurve(6, grenadeAttributes.throwingDistance-1);
        
    }

    private void Update()
    {
        //curvePrevisualisation();
    }

    public void curvePrevisualisation()
    {
        for (int i = 0; i < curvePoints.Length; i++)
        {
            curvePointsWS[i] = transform.rotation * Quaternion.Euler(0, -90, 0) * curvePoints[i] + transform.position + transform.forward;
            lineRenderer.SetPositions(curvePointsWS);
        }
        lineRenderer.enabled = true;
        circle.transform.localScale = new Vector3(grenadeAttributes.explosionRadius / 5, grenadeAttributes.explosionRadius / 5, 0);
        circle.transform.position = new Vector3(curvePointsWS[curvePointsWS.Length-1].x, 0.51f, curvePointsWS[curvePointsWS.Length-1].z);
        Debug.Log(curvePointsWS[curvePointsWS.Length - 1]);
    }

    public void cancelCurvePrevisualisation() 
    {
        lineRenderer.enabled = false;
    }

    public override void ActivateSpell(Vector3 throwingDirection, Transform pos)
    {
        Debug.Log("launchGrenade");
        grenade = XL_Pooler.instance.PopPosition("BlastGrenade", pos.position + pos.forward + pos.up);
        startingVelocity = XL_Utilities.GetVelocity(0.5f, Mathf.Lerp(grenadeAttributes.minThrowingDistance, grenadeAttributes.throwingDistance, throwingDirection.magnitude), grenadeAttributes.travelTime);
        grenade.GetComponent<Rigidbody>().velocity = new Vector3(startingVelocity[0] * (throwingDirection.x), startingVelocity[1], startingVelocity[0] * (throwingDirection.z));
        grenade.GetComponent<XL_Grenade>().SetValue(grenadeAttributes.explosionDamage, grenadeAttributes.explosionRadius);
    }
}
