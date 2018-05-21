using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	#region Serialized Fields
	[SerializeField]				private Vector3		feetOffset;
	[SerializeField]				private float speed;
	[SerializeField]				private float pullSpeed;
	[SerializeField]				private float climbSpeed;
	[SerializeField]				private float jumpSpeed;
    [SerializeField][Range(0, 1)]	private float airControlIncidence;
    [SerializeField]				private float lowJumpModifier;
    [SerializeField]				private float gravityModifier;
	#endregion

	#region Components
	private Animator	animator;
	private Rigidbody	rigidBody;
	#endregion

	#region Player State
	enum PlayerState
	{
		WALKING,
		INTERACTING,
		FALLING,
		CLIMBING,
		PULLING,
		PUSHING
	}
	private GameObject	grabbed;
	private Transform	checkpoint;
	private PlayerState	state;
	private Vector3		movement;
	private float		initialVelocity;
	private Vector3	firstRot;
	private bool		created = false;
	#endregion

	void Awake()
	{
		if (created)
		{
			transform.position = checkpoint.transform.position;
			state = PlayerState.FALLING;
		}
		else
		{
			DontDestroyOnLoad(gameObject);
			rigidBody = GetComponent<Rigidbody>();
		animator	= GetComponent<Animator>();
		firstRot	= transform.rotation.eulerAngles;
			created = true;
		}
	}

	private void Update()
	{
		Vector2 frameAxes		= new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		float frameSpeed		= speed;
		movement				= rigidBody.velocity;
		rigidBody.useGravity	= true;



		switch (state)
		{
			case PlayerState.WALKING:
				if (frameAxes.x < 0)
					transform.eulerAngles = new Vector3(firstRot.x, firstRot.y-180, firstRot.z);
				else if (frameAxes.x > 0)
					transform.eulerAngles = new Vector3(firstRot.x, firstRot.y, firstRot.z);
				animator.SetBool("WALKING", true);
				movement.x = frameAxes.x * frameSpeed;
				animator.SetFloat("Speed", Mathf.Abs(frameAxes.x));
				initialVelocity = movement.x;
				if (NumBaseCollisions() == 0)
				{
					animator.SetBool("WALKING", false);
					animator.SetFloat("Speed", 0f);
					state = PlayerState.FALLING;
				}
				else if (Input.GetButton("Interact"))
				{
					animator.SetBool("WALKING", false);
					animator.SetFloat("Speed", 0f);
					state = PlayerState.INTERACTING;
				}
				else
					HandleJump();
			break;
			case PlayerState.INTERACTING:
				animator.SetBool("INTERACTING", true);
				movement = new Vector3(0f, 0f, 0f);
				if (!Input.GetButton("Interact"))
				{
					if(NumBaseCollisions() > 0)
					{
						animator.SetBool("INTERACTING", false);
						state = PlayerState.WALKING;
					}
					else
					{
						animator.SetBool("INTERACTING", false);
						state = PlayerState.FALLING;
					}
				}
			break;
			case PlayerState.FALLING:
				if (frameAxes.x < 0)
					transform.eulerAngles = new Vector3(firstRot.x, firstRot.y - 180, firstRot.z);
				else if (frameAxes.x > 0)
					transform.eulerAngles = new Vector3(firstRot.x, firstRot.y, firstRot.z);
				animator.SetBool("FALLING", true);
				animator.SetBool("JUMPING", false);
				Vector3 frameGravity = Vector3.up * Physics.gravity.y * Time.deltaTime;
				movement.x = Mathf.Lerp(initialVelocity, frameAxes.x * frameSpeed, airControlIncidence);
				movement += frameGravity * (gravityModifier - 1);
				if (rigidBody.velocity.y > 0f && !Input.GetButton("Jump"))
				{
					movement += frameGravity * (lowJumpModifier - 1);
				}
				else if (rigidBody.velocity.y <= 0f && NumBaseCollisions() > 0)
				{
					animator.SetBool("FALLING", false);
					state = PlayerState.WALKING;
				}
			break;
			case PlayerState.CLIMBING:
                transform.eulerAngles = new Vector3(firstRot.x, firstRot.y, firstRot.z);
				animator.SetBool("CLIMBING", true);
				rigidBody.useGravity = false;
				movement = frameAxes * climbSpeed;
				initialVelocity = movement.x;
				HandleJump();
			break;
		}

		rigidBody.velocity = movement;
		HandleGrab();
	}

	#region Player Actions
	private void HandleJump()
	{
		if (grabbed == null && Input.GetButtonDown("Jump"))
		{
			movement.y += jumpSpeed;
			animator.SetBool("WALKING", false);
			animator.SetBool("CLIMBING", false);
			animator.SetFloat("Speed", 0f);
			animator.SetBool("JUMPING", true);
			state = PlayerState.FALLING;
		}
	}

	private void HandleGrab()
	{
		if (!Input.GetButton("Grab"))
		{
			if (grabbed != null)
			{
                //grabbed.transform.parent = transform.root;
                //grabbed.layer = 0;
				grabbed = null;
			}
		}
        else if (grabbed != null)
        {
            float dx = grabbed.transform.position.x - transform.position.x;
            dx = dx < 0f ? -1.5f : 1.5f;
            grabbed.transform.position = transform.position + new Vector3(dx, 1f, 0f);
        }
	}
	#endregion

	#region Collisions/Triggers
	private int NumBaseCollisions()
	{
		Vector3 location = transform.position + feetOffset;
		return Physics.OverlapSphere(
			location, 
			0.2f, 
			~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Trigger"))).Length;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.gameObject.tag == "Platform")
			transform.parent = collision.gameObject.transform;
		else if (collision.gameObject.CompareTag("Kill"))
		{
			if (checkpoint != null)
				SceneManager.LoadScene(1);
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.CompareTag("Grab"))
		{
			if (Input.GetButton("Grab"))
			{
				grabbed = collision.gameObject;
				//grabbed.layer = LayerMask.NameToLayer("Grab");
				//grabbed.transform.parent = transform;
			}
			else if (grabbed != null)
			{
				//grabbed.transform.parent = transform.root;
				//grabbed.layer = 0;
				grabbed = null;
			}
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (!Input.GetButton("Grab") && collision.gameObject.CompareTag("Grab") && grabbed != null)
		{
			//grabbed.transform.parent = transform.root;
			//grabbed.layer = 0;
			grabbed = null;
		}
		else if (collision.gameObject.CompareTag("Platform"))
			transform.parent = transform.root;
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.CompareTag("Checkpoint"))
			checkpoint = collider.gameObject.transform;
		else if (collider.gameObject.CompareTag("Kill"))
		{
			if (checkpoint != null)
				SceneManager.LoadScene(1);
		}
	}

	private void OnTriggerStay(Collider collider)
	{
		if (collider.gameObject.CompareTag("Climb") && Input.GetAxis("Vertical") >= 0.95f)
		{
			animator.SetBool("WALKING", false);
			animator.SetBool("FALLING", false);
			animator.SetBool("JUMPING", false);
			state = PlayerState.CLIMBING;
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject.CompareTag("Climb"))
		{
			animator.SetBool("CLIMBING", false);
			state = PlayerState.FALLING;
		}
	}
	#endregion
}
