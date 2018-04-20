using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour {

	[SerializeField] private float speed;
	public Vector3 dir;

	void Start () {
		
	}
	
	void Update () {
		transform.position += dir * speed * Time.deltaTime;
	}
}
