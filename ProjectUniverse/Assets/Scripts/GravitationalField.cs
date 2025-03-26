using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;

public class GravitationalField : MonoBehaviour
{
    public float mass = 1000f;
    
    public float radius = 50f;
    
  
    public const double gravitationalConstant = 6.674e-11;
    
    public float scaleFactor = 1000000f;
    
    public float gravityMultiplier = 1f;
    
    public float minDistance = 0.1f;
    
    public bool showDebugSphere = true;
    
    public bool shrinkObjectWhenBlackHole;

    private float shrinkFactor = 0.001f;
    
    public Color debugSphereColor = new Color(0.5f, 0.8f, 1f, 0.2f);

    private float shrinkMagnitude;

    
    
    private void OnDrawGizmos()
    {
        if (showDebugSphere)
        {
            Gizmos.color = debugSphereColor;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
    
    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        
        foreach (Collider col in colliders)
        {
            GravityObject gravObj = col.GetComponent<GravityObject>();
            if (gravObj != null)
            {
                ApplyGravityToObject(gravObj);
            }
        }
    }

    private IEnumerator ShrinkObjectOnDelay(GravityObject gravObj, float shrinkMagnitude)
    {

        gravObj = gravObj.GetComponent<GravityObject>();
        
        gravObj.GetComponentInParent<Transform>().localScale -= new Vector3(shrinkMagnitude, shrinkMagnitude, shrinkMagnitude);

        Vector3 gravObjVector = gravObj.GetComponentInParent<Transform>().localScale;
        
        if (gravObjVector.x < -0.1f)
        {
            Destroy(gravObj.gameObject);
        }
        yield return new WaitForSeconds(0.1f);
    }
    
    private void ApplyGravityToObject(GravityObject gravObj)
    {
        Rigidbody rb = gravObj.GetComponent<Rigidbody>();
        
        if (rb == null) return;

       
       
        Vector3 direction = transform.position - gravObj.transform.position;
        
        float distance = direction.magnitude;
        
        distance = Mathf.Max(distance, minDistance);
        
        direction.Normalize();
        
        float objMass = (gravObj.gravitationalMass > 0) ? gravObj.gravitationalMass : rb.mass;
        
        double forceMagnitude = gravitationalConstant * (mass * objMass) / (distance * distance);
        
        forceMagnitude *= scaleFactor;
        
        forceMagnitude *= gravityMultiplier * gravObj.gravityMultiplier;
        
        rb.AddForce(direction * (float)forceMagnitude);




        shrinkMagnitude = gravObj.gravitationalMass / (distance * distance);
        
        
        if (shrinkObjectWhenBlackHole)
        {
            StartCoroutine(ShrinkObjectOnDelay(gravObj, shrinkMagnitude));
        }
        
        
    }
}