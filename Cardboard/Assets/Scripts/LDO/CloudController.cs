using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    private Rigidbody   lightning;
    private Vector3     lightningBase;

    void Awake()
    {
        lightning = GetComponentInChildren<Rigidbody>();
        lightningBase = lightning.transform.position;
    }

	void Start ()
    {
        lightning.useGravity = true;
	}
	
	void Update ()
    {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        lightning.transform.position = lightningBase;
    }
}
