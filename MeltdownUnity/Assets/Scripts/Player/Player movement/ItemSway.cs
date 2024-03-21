using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSway : MonoBehaviour
{
	[SerializeField] private float _smooth;
	[SerializeField] private float _swayMultiplier;

	private void Start()
	{

	}

	private void Update()
	{
		float mouseX = Input.GetAxisRaw("Mouse X") * _swayMultiplier;
		float mouseY = Input.GetAxisRaw("Mouse Y") * _swayMultiplier;

		Quaternion rotationX = Quaternion.AngleAxis(mouseY, Vector3.right);
		Quaternion rotationY = Quaternion.AngleAxis(-mouseX, Vector3.up);

		Quaternion targetRotation = rotationX * rotationY;

		transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, _smooth * Time.deltaTime);
	}
}
