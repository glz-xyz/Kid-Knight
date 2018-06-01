using System.Collections.Generic;
using UnityEngine;

public class InteractableSwitch : MonoBehaviour
{
	[SerializeField] private List<Interactable>	targets;

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
			if (Input.GetButtonDown("Interact"))
				foreach (var t in targets)
					t.StartInteract();
			else
				foreach (var t in targets)
					t.StopInteract();
		}
		
	}
}
