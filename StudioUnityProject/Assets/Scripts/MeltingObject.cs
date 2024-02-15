using UnityEngine;

public class MeltingObject : MonoBehaviour
{
	public Material meltingMaterial;

	public int AmmountHit = 1;

	private Vector3 _baseScale;

	private Vector3 _scaleAmmount;

	void Start()
	{
		_baseScale = transform.localScale;

		_scaleAmmount = _baseScale / AmmountHit;
	}

	private void OnCollisionEnter(Collision collision)
	{

		if (collision.gameObject.GetComponent<Fire>() != null)  //why is this not working i want to die
		{
			// Apply the melting effect
			ApplyMeltingEffect();
		}
	}

	private void ApplyMeltingEffect()
	{
		transform.localScale -= _scaleAmmount;
	}
}
