using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityObject : MonoBehaviour
{
    public float gravitationalMass = 0f;
    
    public float gravityMultiplier = 1f;
    
    public const double gravitationalConstant = 6.674e-11;
    
    public float scaleFactor = 1000000f;
    
    public float maxGravityRadius = 100f;
    
    public float minDistance = 0.1f;

    public Vector3 initialVelocity;
    
    public bool applyInitialVelocity = true;
    
    private static List<GravityObject> allGravityObjects = new List<GravityObject>();
    
    private Rigidbody rb;

    public bool isStarOrPlanet = false;
    
    public bool showMaxRadiusSphere = true;
    public Color maxRadiusSphereColor = new Color(0.2f, 0.6f, 0.8f, 0.15f);
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        rb.useGravity = false;

        if (isStarOrPlanet)
        {
            rb.isKinematic = true;
        }
    }

    private void Start()
    {
        if (applyInitialVelocity)
        {
            StartCoroutine(ApplyInitialVelocity());
        }
    }
    private void OnEnable()
    {
        if (!allGravityObjects.Contains(this))
        {
            allGravityObjects.Add(this);
        }
    }
     IEnumerator ApplyInitialVelocity()
    {
        if (applyInitialVelocity)
        {
            rb.linearVelocity = initialVelocity;
        }
        
        yield return new WaitForSeconds(4f);
    }
    
    private void OnDisable()
    {
        if (allGravityObjects.Contains(this))
        {
            allGravityObjects.Remove(this);
        }
    }
    
    private void FixedUpdate()
    {
        ApplyGravitationalForces();
    }
    
    private void ApplyGravitationalForces()
    {
        float effectiveMass = (gravitationalMass > 0) ? gravitationalMass : rb.mass;
        
        foreach (GravityObject otherObject in allGravityObjects)
        {
            if (otherObject == this) continue;
            
            Vector3 direction = otherObject.transform.position - transform.position;
            float distance = direction.magnitude;
            
            if (distance > maxGravityRadius) continue;
            
            distance = Mathf.Max(distance, minDistance);
            
            direction.Normalize();
            
            float otherMass = (otherObject.gravitationalMass > 0) ? otherObject.gravitationalMass : otherObject.rb.mass;
            double forceMagnitude = gravitationalConstant * (effectiveMass * otherMass) / (distance * distance);
            
            forceMagnitude *= scaleFactor;
            
            forceMagnitude *= gravityMultiplier * otherObject.gravityMultiplier;
            
            Vector3 force = direction * (float)forceMagnitude;
            rb.AddForce(force);
        }
    }
    
    public List<GravityObject> GetNearbyGravityObjects(float radius)
    {
        List<GravityObject> nearbyObjects = new List<GravityObject>();
        
        foreach (GravityObject obj in allGravityObjects)
        {
            if (obj == this) continue;
            
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance <= radius)
            {
                nearbyObjects.Add(obj);
            }
        }
        
        return nearbyObjects;
    }
    private void OnDrawGizmos()
    {
        if (showMaxRadiusSphere)
        {
            Gizmos.color = maxRadiusSphereColor;
            Gizmos.DrawWireSphere(transform.position, maxGravityRadius);
        }
    }
}
