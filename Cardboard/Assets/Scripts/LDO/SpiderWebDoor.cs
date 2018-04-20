using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWebDoor : Interactable
{
    private MeshRenderer    webMesh;
    private BoxCollider     collider;
    [SerializeField] private bool            open;

	void Awake ()
	{
        webMesh     = GetComponentInChildren<MeshRenderer>();
        collider    = GetComponentInChildren<BoxCollider>();
	}

	public override void StartInteract()
	{
        open              = !open;
        webMesh.enabled   = open;
        collider.enabled  = open;
	}

	public override void StopInteract()
	{
		
	}
}
