using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCam : MonoBehaviour
{
	[SerializeField]	private Vector3		finalPos;
	[SerializeField]	private float		speed = 2f;
						private GameObject	cam;
						private FollowCam	camScript;

	void Start ()
	{
		cam = GameObject.FindGameObjectWithTag("MainCamera");
		camScript = cam.GetComponent<FollowCam>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			camScript.enabled = false;
			StartCoroutine(GoToPos());
		}
	}

	private IEnumerator GoToPos()
	{
		for (int i = 0; i <60; i++)
		{
			cam.transform.position = Vector3.Lerp(cam.transform.position, finalPos, speed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
		
	}


}
