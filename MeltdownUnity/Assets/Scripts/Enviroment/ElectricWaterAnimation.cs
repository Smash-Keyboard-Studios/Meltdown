using System;
using System.Collections;
using UnityEngine;

public class ElectricWaterAnimation : MonoBehaviour
{

    public Texture[] electricTextures; //
    private Material electricMaterial; //

    private float[] onOffDurations = new float[] { 1, 2, 1.5f, 2, 1, 1.5f, 1, 1.5f };
    private bool startPrevention = true;

    // Start is called before the first frame update
    void Start()
    {
        electricMaterial = transform.GetComponent<Renderer>().material;
        if (electricTextures.Length <= 1) return;

        foreach (Texture tex in electricTextures)
        {
            if (tex == null)
            {
                Debug.LogWarning("Electric Water has missing texture");
                return;
            }
        }
        StartCoroutine(AnimateZap());
        StartCoroutine(ZapFlash());
    }

    
    private void OnEnable() 
    {
        if (electricTextures.Length <= 1 || startPrevention)
        {
            startPrevention = false;
            return;
        }

        foreach (Texture tex in electricTextures)
        {
            if (tex == null)
            {
                Debug.LogWarning("Electric Water has missing texture");
                return;
            }
        }
        StartCoroutine(AnimateZap());
        StartCoroutine(ZapFlash());
    }


    IEnumerator AnimateZap()
    {
        while (true)
        {
            foreach (var texture in electricTextures)
            {
                electricMaterial.SetTexture("_BaseMap", texture);
                yield return new WaitForSeconds(0.07f);
            }
        }

    }

    IEnumerator ZapFlash()
    {
        while (true)
        {

            foreach (float duration in onOffDurations)
            {
                electricMaterial.color = new Color(1, 1, 1, 1);
                //Activate Zap Sound here
                yield return new WaitForSeconds(duration-0.5f);

                electricMaterial.color = new Color(1, 1, 1, 0);
                //Disable Zap Sound here
                yield return new WaitForSeconds(duration);

            }
            
        }
        
    }


}
