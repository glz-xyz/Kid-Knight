using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossBehaviour : Interactable {

	[SerializeField] private Transform					fireSpawn;
	[SerializeField] private Transform					fireTarget;
	[SerializeField] private float						fireDelay;
	[SerializeField] private List<float>				fireRate;
	[SerializeField] private List<float>				hitRate;
	[SerializeField] private GameObject					firePrefab;
	[SerializeField] private float						hitLag;
	private int life;
	private Animator anim;
	private Vector3 flameDir;


	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Start ()
	{
		flameDir = (fireTarget.position - fireSpawn.position).normalized;
		life = 3;
	}
	
	void Update ()
	{
	}

	private IEnumerator Punch()
	{
		while(life > 0)
		{
			for (int i = 0; i < hitRate.Capacity; i++)
			{
				StartCoroutine(PlayAnim("PUNCH"));
				yield return new WaitForSeconds(hitRate[i]);
			}
		}
	}

	private IEnumerator Flame()
	{
		while (life > 0)
		{
			for (int i = 0; i < fireRate.Capacity; i++)
			{
				StartCoroutine(ShootFlame());
				yield return new WaitForSeconds(fireRate[i]);
			}
		}
	}

	private IEnumerator PlayAnim(string name)
	{
		anim.SetBool(name, true);
		yield return new WaitForEndOfFrame();
		anim.SetBool(name, false);
	}

	private IEnumerator ShootFlame()
	{
		StartCoroutine(PlayAnim("FLAME"));
		yield return new WaitForSeconds(fireDelay);
		GameObject f = Instantiate(firePrefab);
		f.transform.position = fireSpawn.position;
		//f.transform.rotation = fireSpawn.rotation;
		f.GetComponent<Flame>().dir = flameDir;
	}

	private IEnumerator GetHit()
	{
		StartCoroutine(PlayAnim("GOT HIT"));
		yield return new WaitForSeconds(hitLag);
		if (life == 2)
			StartCoroutine(Punch());
		else if (life == 1)
			StartCoroutine(Flame());
		else if (life < 1)
		{
			yield return new WaitForSeconds(1);
			Destroy(GameObject.FindGameObjectWithTag("Player"));
			SceneManager.LoadScene(0);
		}
	}

	override public void StartInteract()
	{
		--life;
		StopAllCoroutines();
		StartCoroutine(GetHit());
	}

	override public void StopInteract()
	{
	}
}
