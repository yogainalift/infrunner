using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : Entity {

	//player handling
	public float gravity = 20;
	public float walkSpeed = 8;
	public float runSpeed = 12;
	public float acceleration = 30;
	public float jumpHeight=12;

	//system	
	private float animationSpeed;
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;

	//states
	private bool jumping;

	//components
	private PlayerPhysics playerPhysics;
	private Animator animator;


	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
		animator = GetComponent<Animator>();
	}
	
	void Update(){

		if (playerPhysics.movementStopped){
			targetSpeed=0;
			currentSpeed=0;
		}

		animationSpeed=IncrementTowards (animationSpeed, Mathf.Abs (targetSpeed), acceleration);
		animator.SetFloat ("Speed", animationSpeed);

		//Input
		float speed= (Input.GetButton("Run")? runSpeed : walkSpeed);
		targetSpeed = Input.GetAxisRaw("Horizontal") * speed;
		currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);

		//if player touching the ground
		if (playerPhysics.grounded){
			amountToMove.y=0;

			if (jumping){
				jumping = false;
				animator.SetBool("Jumping", false);
			}

			//jump input
			if (Input.GetButtonDown("Jump")){
				amountToMove.y = jumpHeight;
				jumping = true;
				animator.SetBool("Jumping",true);
			}
		}
		//amount to move
		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity *Time.deltaTime;
		playerPhysics.Move(amountToMove*Time.deltaTime);

		//face direction
		float moveDir = Input.GetAxisRaw("Horizontal");

		if (moveDir != 0){
			transform.eulerAngles = (moveDir > 0)? Vector3.up * 180 : Vector3.zero;
		}
	}

	private float IncrementTowards(float n, float target, float a){
		if (n == target){
			return n;
		}
		else{
			float dir = Mathf.Sign(target - n);
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target - n))? n: target;
		}
	}
}