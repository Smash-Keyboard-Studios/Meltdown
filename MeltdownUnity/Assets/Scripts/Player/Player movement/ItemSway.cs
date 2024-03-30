using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSway : MonoBehaviour
{
	[SerializeField, Tooltip("How fast to return the object bac; to the normal rotation")]
	private float _smooth;

	[SerializeField, Tooltip("How noticable the sway is")]
	private float _swayMultiplier;

	private void Update()
	{
		// we want to get the direction the player is moving their mouse.
		float mouseX = Input.GetAxisRaw("Mouse X") * _swayMultiplier;
		float mouseY = Input.GetAxisRaw("Mouse Y") * _swayMultiplier;

		// invert the rotation as a counter rotation.
		Quaternion rotationX = Quaternion.AngleAxis(mouseY, Vector3.right);
		Quaternion rotationY = Quaternion.AngleAxis(-mouseX, Vector3.up);

		// Get the target rotation from both counter rotation.
		Quaternion targetRotation = rotationX * rotationY;

		// Use lerp to control the speed at which the rotation is returned to normal.
		transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, _smooth * Time.deltaTime);
	}
}
