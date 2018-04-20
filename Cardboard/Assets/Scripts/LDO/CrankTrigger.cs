using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankTrigger : MonoBehaviour
{
	[SerializeField] private List<Interactable> targets;
    [SerializeField] private float rotateSpeed = 2f;

    private Transform mesh;

    void Awake()
    {
        mesh = GetComponentInChildren<Transform>();
    }

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
            {
                mesh.transform.Rotate(new Vector3(0f, 0f, -rotateSpeed));
                foreach (var t in targets)
                    t.StartInteract();
            }
            else
            {
                foreach (var t in targets)
                    t.StopInteract();
            }
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
			foreach (var t in targets)
				t.StopInteract();
	}
}
