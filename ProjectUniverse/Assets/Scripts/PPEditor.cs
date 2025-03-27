using UnityEngine;
using UnityEngine.Rendering;

public class PPEditor : MonoBehaviour
{
    [SerializeField] private GameObject PP;

    private Volume volumeObj;
    VolumeProfile volumeProfile;
    private Component[] components;
    void Start()
    {
         volumeObj = PP.GetComponent<Volume>();
         volumeProfile = volumeObj.profile; // volume components
         
      //   foreach (var comp in volumeProfile.components)
     //   {
      //       Debug.Log(comp.GetType());
      //   }
         
    }

   
}

