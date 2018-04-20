using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInteractable : Interactable
{
	enum MoveState { LEFT, RIGHT }
	[SerializeField] private MoveState  moveState;
	[SerializeField] private Vector2	moveDir;
	[SerializeField] private float		moveSpeed;

	private Rigidbody	rigidBody;
	private Vector2		movement;
	private bool		moving;

	void Awake ()
	{
		rigidBody = GetComponent<Rigidbody>();
		moving = false;
		moveDir = moveState == MoveState.RIGHT ? moveDir : -moveDir;
		moveDir.Normalize();
		movement = moveDir;

		RigidbodyConstraints rbFlags  = RigidbodyConstraints.FreezeRotation;
		if (moveDir.x != 0f) rbFlags |= RigidbodyConstraints.FreezePositionX;
		if (moveDir.y != 0f) rbFlags |= RigidbodyConstraints.FreezePositionY;
	}

	private void Update()
	{
		if (moving)
		{
			transform.position = transform.position +
				new Vector3(movement.x, movement.y) *
				moveSpeed *
				Time.deltaTime;
		}
	}

	private void SwitchDirections()
	{
		moveDir *= -1f;
		movement = moveDir;
	}

	override public void StartInteract() { moving = true;  }
	override public void StopInteract()	 { moving = false; }

	private void OnTriggerEnter(Collider collider)
	{
        Debug.Log(collider.gameObject.tag);
		if (collider.gameObject.tag == "PlatformCheckpoint")
		{
			if (moveState == MoveState.LEFT)
				moveState = MoveState.RIGHT;
			else
				moveState = MoveState.LEFT;
			SwitchDirections();
		}
	}
}
