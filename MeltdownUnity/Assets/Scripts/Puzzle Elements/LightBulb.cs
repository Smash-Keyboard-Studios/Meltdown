using UnityEngine;

/// <summary>
/// No go away
/// Dirt simple
/// </summary>
public class LightBulb : MonoBehaviour
{
	public GameObject bulb;

	public bool Active = false;

	// Update is called once per frame
	void Update()
	{
		bulb.SetActive(Active);
	}

	public void ToggleBulb()
	{
		Active = !Active;
	}

	public void DeActive()
	{
		Active = false;
	}

	public void Activate()
	{
		Active = true;
	}
}
