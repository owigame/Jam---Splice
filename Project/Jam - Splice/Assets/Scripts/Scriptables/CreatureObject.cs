using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "CreatureObject", menuName = "Creatures/New Creature", order = 0)]

[System.Serializable]
public class CreatureObject : ScriptableObject {

	[System.Serializable]
	public struct CapsuleColliderData {
		public Vector3 center;
		public int direction;
		public float height;
		public float radius;

		public CapsuleColliderData (CapsuleCollider collider) {
			center = collider.center;
			direction = collider.direction;
			height = collider.height;
			radius = collider.radius;
		}
	}

	[Header ("Creature Settings")]
	public float uniformScale = 0.5f;
	[Header("Creature Config")]
	public float health = 100;
	public float damage = 100;

	[Header ("Sprites")]
	public Sprite head;
	public Sprite jaw;
	public Sprite body;
	public Sprite tail;
	public Sprite frontLegsR;
	public Sprite frontLegsL;
	public Sprite backLegsR;
	public Sprite backLegsL;

	[Header ("Pivots")]
	public Vector3 hipsPivot;
	public Vector3 bodyPivot;
	public Vector3 headPivot;
	public Vector3 jawPivot;
	public Vector3 frontLegsRPivot;
	public Vector3 frontLegsLPivot;
	public Vector3 backLegsRPivot;
	public Vector3 backLegsLPivot;
	public Vector3 tailPivot;

	[Header ("Colliders")]
	public CapsuleColliderData bodyCol;
	public CapsuleColliderData headCol;
	public CapsuleColliderData frontLegsRCol;
	public CapsuleColliderData frontLegsLCol;
	public CapsuleColliderData backLegsRCol;
	public CapsuleColliderData backLegsLCol;

	void Start () {

	}

	void Update () {

	}
}