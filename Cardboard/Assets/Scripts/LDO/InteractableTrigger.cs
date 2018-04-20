using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
	[SerializeField] private List<Interactable> targets;

	void Start ()
	{
		
	}
	
	void Update ()
	{
		
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			if (Input.GetButton("Interact"))
				foreach (var t in targets)
					t.StartInteract();
			else
				foreach (var t in targets)
					t.StopInteract();
		}
		
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
			foreach (var t in targets)
				t.StopInteract();
	}
}
