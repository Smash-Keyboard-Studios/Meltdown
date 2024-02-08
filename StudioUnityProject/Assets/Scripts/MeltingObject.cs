using UnityEngine;

public class MeltingObject : MonoBehaviour
{
    public Material meltingMaterial;

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.CompareTag("Fire"))  //why is this not working i want to die
        {
            // Apply the melting effect
            ApplyMeltingEffect();
        }
    }

    private void ApplyMeltingEffect()
    {
        transform.localScale *= 0.9f;
    }
}
