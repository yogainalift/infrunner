using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class Controller : RaycastController{ 

	float maxClimbAngle = 80;
	float maxDescendAngle = 80;
	
	public CollisionInfo collisions;


	public override void Start(){
		base.Start();
		GetComponent<BoxCollider>().isTrigger = true;
	}
	
	public void Move(Vector3 velocity, bool standingOnPlatform = false){
		UpdateRaycastOrigins();
		collisions.Reset();
		collisions.velocityOld = velocity;

		if (velocity.y < 0) {
			DescendSlope(ref velocity);
		}
		if (velocity.x !=0) {
			HorizontalCollisions(ref velocity);
		}
		if (velocity.y !=0){
			VerticalCollisions(ref velocity);
		}
		transform.Translate (velocity);

		if (standingOnPlatform){
			collisions.below = true;
		}
	}

	void HorizontalCollisions(ref Vector3 velocity) {
		float rayLength = Mathf.Abs (velocity.z) - transform.localScale.z/1.5f;
		
		for (int i = 0; i < horizontalRayCount; i ++) {
			Vector3 rayOrigin = new Vector3( raycastOrigins.bottomLeft.x , transform.position.y, raycastOrigins.bottomRight.z);
			rayOrigin += Vector3.right *2 * (horizontalRaySpacing * i);
			RaycastHit hit;
			Ray ray = new Ray(rayOrigin, Vector3.forward  * rayLength);

			Debug.DrawRay(rayOrigin, Vector3.forward  * rayLength, Color.blue);
			
			if (Physics.Raycast(ray, out hit, rayLength, collisionMask)){

				if (hit.distance == 0){
					continue;
				}

				float slopeAngle = Vector3.Angle(hit.normal,Vector3.up);

				if (i==0 && slopeAngle <= maxClimbAngle){
					if (collisions.descendingSlope) {
						collisions.descendingSlope = false;
						velocity = collisions.velocityOld;
					}

					float distanceToSlopeStart = 0;
					ClimbSlope(ref velocity, slopeAngle);
				}

				if (!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
					rayLength = hit.distance;
					
					if (collisions.climbingSlope) {
						velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
					}
					
					collisions.left = true;
					collisions.right = false;
				}

			
			}
		}
	}

	void VerticalCollisions(ref Vector3 velocity){
		float directionY=Mathf.Sign(velocity.y);
		float rayLength = Mathf.Abs(velocity.y) + skinWidth;

		for (int i=0 ; i< verticalRayCount ; i++){
			Vector3 rayOrigin = (directionY == -1) ? new Vector3( transform.position.x, raycastOrigins.bottomLeft.y, raycastOrigins.bottomLeft.z ):new Vector3( transform.position.x, raycastOrigins.topLeft.y, raycastOrigins.topLeft.z );
			rayOrigin += Vector3.back * 2 * (verticalRaySpacing * i+ velocity.x);
			RaycastHit hit;
			Ray ray = new Ray(rayOrigin, Vector3.up * directionY);

			Debug.DrawRay(rayOrigin, Vector3.up *directionY * rayLength, Color.red ); 

			if (Physics.Raycast(ray, out hit, rayLength, collisionMask)){ //if hit
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				if (collisions.climbingSlope){
					velocity.x = velocity.y / Mathf.Tan (collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign (velocity.x);
				}

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
		}

		if (collisions.climbingSlope){
			float directionX = Mathf.Sign(velocity.x);
			rayLength = Mathf.Abs(velocity.x) + skinWidth;
			Vector3 rayOrigin = ((directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight) +Vector3.up * velocity.y;
			RaycastHit hit;
			Ray ray = new Ray(rayOrigin, Vector3.right * directionX);

			if (Physics.Raycast(ray, out hit, rayLength, collisionMask)){ //if hit
				float slopeAngle = Vector3.Angle (hit.normal, Vector3.up);
				if (slopeAngle != collisions.slopeAngle){
					velocity.x = (hit.distance - skinWidth)* directionX;
					collisions.slopeAngle = slopeAngle;
				}
			}
		}
	}

	void ClimbSlope(ref Vector3 velocity, float slopeAngle){
		float moveDistance = Mathf.Abs (velocity.x);
		float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (velocity.y <= climbVelocityY ){
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
			collisions.below = true;
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
		}
	}

	void DescendSlope(ref Vector3 velocity) {
		float directionX = Mathf.Sign (velocity.x);
	
		Vector3 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
		RaycastHit hit;
		Ray ray = new Ray(rayOrigin, -Vector3.up);


		if (Physics.Raycast(ray, out hit, Mathf.Infinity, collisionMask)){ //if hit

			float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
			if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) {
				if (Mathf.Sign(hit.normal.x) == directionX) {
					if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x)) {
						float moveDistance = Mathf.Abs(velocity.x);
						float descendVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
						velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
						velocity.y -= descendVelocityY;
						
						collisions.slopeAngle = slopeAngle;
						collisions.descendingSlope = true;
						collisions.below = true;
					}
				}
			}
		}
	}
}