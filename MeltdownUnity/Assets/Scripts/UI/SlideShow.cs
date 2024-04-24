using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideShow : MonoBehaviour
{
    [SerializeField] private GameObject Image1 = null;
    [SerializeField] private GameObject Image2 = null;
    [SerializeField] private GameObject Image3 = null;
    [SerializeField] private GameObject Image4 = null;
    [SerializeField] private GameObject Image5 = null;
    [SerializeField] private GameObject Image6 = null;
    [SerializeField] private GameObject Image7 = null;
    [SerializeField] private GameObject Image8 = null;
    [SerializeField] private GameObject Image9 = null;
    [SerializeField] private GameObject Image10 = null;
    [SerializeField] private GameObject Image11 = null;
    [SerializeField] private GameObject Image12 = null;
    [SerializeField] private GameObject Image13 = null;
    [SerializeField] private GameObject Image14 = null;
    [SerializeField] private GameObject Image15 = null;
    [SerializeField] private GameObject Image16 = null;

    private void Start()
    {
        StartCoroutine(ImageSlideShow());
    }

    public IEnumerator ImageSlideShow()
    {
        Image1.SetActive(true);
        yield return new WaitForSeconds(20);
        Image1.SetActive(false);
        Image2.SetActive(true);
        yield return new WaitForSeconds(20);
        Image2.SetActive(false);
        Image3.SetActive(true);
        yield return new WaitForSeconds(20);
        Image3.SetActive(false);
        Image4.SetActive(true);
        yield return new WaitForSeconds(20);
        Image4.SetActive(false);
        Image5.SetActive(true);
        yield return new WaitForSeconds(20);
        Image5.SetActive(false);
        Image6.SetActive(true);
        yield return new WaitForSeconds(20);
        Image6.SetActive(false);
        Image7.SetActive(true);
        yield return new WaitForSeconds(20);
        Image7.SetActive(false);
        Image8.SetActive(true);
        yield return new WaitForSeconds(20);
        Image8.SetActive(false);
        Image9.SetActive(true);
        yield return new WaitForSeconds(20);
        Image9.SetActive(false);
        Image10.SetActive(true);
        yield return new WaitForSeconds(20);
        Image10.SetActive(false);
        Image11.SetActive(true);
        yield return new WaitForSeconds(20);
        Image11.SetActive(false);
        Image12.SetActive(true);
        yield return new WaitForSeconds(20);
        Image12.SetActive(false);
        Image13.SetActive(true);
        yield return new WaitForSeconds(20);
        Image13.SetActive(false);
        Image14.SetActive(true);
        yield return new WaitForSeconds(20);
        Image14.SetActive(false);
        Image15.SetActive(true);
        yield return new WaitForSeconds(20);
        Image15.SetActive(false);
        Image16.SetActive(true);
        yield return new WaitForSeconds(20);
        Image16 .SetActive(false);
        
        StartCoroutine(ImageSlideShow());

    }
}
