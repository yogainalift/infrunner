using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour{ 

	public LayerMask collisionMask;

	private BoxCollider myCollider;
	private Vector3 s; //size
	private Vector3 c; //center

	private Vector3 originalSize;
	private Vector3 originalCenter;
	private float colliderScale;

	private int collisionDivisionX = 3;
	private int collisionDivisionY = 10;

	private float skin= .005f;

	[HideInInspector]
	public bool grounded;
	[HideInInspector]
	public bool movementStopped;

	Ray ray;
	RaycastHit hit;
	void Start(){
		myCollider = GetComponent<BoxCollider>();
		colliderScale = transform.localScale.x;

		originalSize = myCollider.size;
		originalCenter = myCollider.center;
		SetCollider(originalSize, originalCenter);
	}

	public void Move(Vector2 moveAmount){

		float deltaX = moveAmount.x;
		float deltaY = moveAmount.y;

		Vector2 p = transform.position; //position

		//collision up-down
		grounded=false;
		for (int i = 0 ; i < collisionDivisionX ;i++){
			float dir = Mathf.Sign (deltaY);
			float x =(p.x+c.x-s.x/2)+s.x/(collisionDivisionX-1) * i; //left, center and then rightmost point of myCollider
			float y = p.y+c.y+s.y/2 * dir; //bottom of myCollider

			ray = new Ray(new Vector2(x,y), new Vector2(0,dir));
//			Debug.DrawRay(ray.origin, ray.direction, Color.green);
			if (Physics.Raycast(ray,out hit, Mathf.Abs(deltaY) + skin, collisionMask)){
				float dst = Vector3.Distance(ray.origin, hit.point);
				if (dst>skin){
					deltaY = dst*dir - skin*dir;
				}
				else{
					deltaY=0;
				}
				grounded = true;
				break;

			}
		}

		//collisions left-right'
		movementStopped = false;
		for (int i =0; i<collisionDivisionY;i++){
			float dir = Mathf.Sign (deltaX);
			float x =p.x + c.x + s.x/2 * dir; //left, center and then rightmost point of myCollider
			float y = p.y+c.y - s.y/2 + s.y/(collisionDivisionY-1) * i;
			
			ray = new Ray(new Vector2(x,y), new Vector2(dir,0));
//			Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
			if (Physics.Raycast(ray,out hit, Mathf.Abs(deltaX) + skin, collisionMask)){
				float dst = Vector3.Distance(ray.origin, hit.point);
				if (dst>skin){
					deltaX = dst*dir - skin*dir;
				}
				else{
					deltaX=0;
				}

				movementStopped=true;
				break;
				
			}
		}

		if (!grounded && !movementStopped){
			Vector3 playerDir = new Vector3(deltaX,deltaY);
			Vector3 o = new Vector3(p.x + c.x + s.x/2 * Mathf.Sign (deltaX), p.y+c.y+s.y/2 * Mathf.Sign (deltaY));
			Debug.DrawRay (o,playerDir.normalized);
			ray= new Ray(o,playerDir.normalized);
			if (Physics.Raycast (ray, Mathf.Sqrt (deltaX*deltaX + deltaY*deltaY), collisionMask)){
				grounded = true;
				deltaY = 0;
			}
		}

		Vector2 finalTransform = new Vector2(deltaX,deltaY);

		transform.Translate(finalTransform, Space.World);
	}

	public void SetCollider(Vector3 size, Vector3 center){
		myCollider.size = size;
		myCollider.center = center;

		s=size*colliderScale;
		c=center*colliderScale;
	}
}