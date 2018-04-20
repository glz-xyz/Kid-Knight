using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {

	[SerializeField] private Animator	bookAnim;
	[SerializeField] private Animator	camAnim;
	[SerializeField] private float		animLength;
	[SerializeField] private float		bookDelay;
	[SerializeField] private int		sceneID;
	[SerializeField] private Canvas		canvas;

	public bool start = false;

	void Start () {
		bookAnim.speed	= 0;
		camAnim.speed	= 0;
	}
	
	void Update () {
		if (Input.GetButtonDown("Jump"))
			StartAnim();
		if (start)
		{
			StartCoroutine(DelayBook());
			camAnim.speed = 1;
		}
	}

	public void StartAnim()
	{
		start = true;
		canvas.enabled = false;
		StartCoroutine(LaunchScene());
	}

	public IEnumerator LaunchScene()
	{
		yield return new WaitForSeconds(animLength);
		SceneManager.LoadScene(sceneID);
	}

	public IEnumerator DelayBook()
	{
		yield return new WaitForSeconds(bookDelay);
		bookAnim.speed = 1;
	}
}
