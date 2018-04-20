using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector2 offset;
    [SerializeField]
    private float maxDistanceX;
    [SerializeField]
    private float maxDistanceY;
    [SerializeField]
    private float speed;
	
	void LateUpdate()
    {
		/*Vector2 dir = new Vector2(target.position.x + offset.x, target.position.y + offset.y) - new Vector2(transform.position.x, transform.position.y);
		if (Mathf.Abs(dir.x) > maxDistanceX)
		{
            transform.position = Vector3.MoveTowards(   transform.position, 
                                                        new Vector3(    target.position.x, transform.position.y,	transform.position.z) +
														new Vector3(    offset.x,			0,						0), 
                                                        speed * Time.deltaTime);
        }*/
		/*if (Mathf.Abs(dir.y) > maxDistanceY)
		{
			transform.position = Vector3.MoveTowards(	transform.position,
														new Vector3(transform.position.x,	target.position.y,	transform.position.z) +
														new Vector3(0,						offset.y,			0),
														speed * Time.deltaTime);
		}*/
		transform.position = Vector3.Lerp(	transform.position,
											new Vector3(target.position.x, transform.position.y, transform.position.z) +
											new Vector3(offset.x, 0, 0),
											speed * Time.deltaTime);
	}
}
