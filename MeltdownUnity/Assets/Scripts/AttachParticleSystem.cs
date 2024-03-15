using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachParticleSystem : MonoBehaviour
{
    public ParticleSystem particlesystemprefab;
    public float time = 2.0f;
    private ParticleSystem particleSystemInstance;

    
    
    // Start is called before the first frame update
    void Start()
    {
        


    }

    void OnCollisionEnter(Collision collision)
    {
       if (gameObject.activeInHierarchy == true);
       {

        
        if (collision.transform.GetComponent<Fire>() != null)
        {

            
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        ParticleSystem tempObj;
        tempObj = Instantiate(particlesystemprefab , pos, rot);

       
        Object.Destroy(gameObject, time);
       
        }
       }
        
        

    
   
        

        
    }

             
        
    

    // Update is called once per frame
   void update()
   {

   }
}

    
        
    

