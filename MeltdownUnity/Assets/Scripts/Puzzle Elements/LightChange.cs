using UnityEngine;

public class LightChange : MonoBehaviour
{
    Light lightcomponent;

    private void Start()
    {
        lightcomponent = GetComponent<Light>();
    }

    public void ToggleLight()
    {
        if (lightcomponent.enabled != true)
        {
            lightcomponent.enabled = true;
        }
        else 
        {
            lightcomponent.enabled = false;
        }
    }
}
