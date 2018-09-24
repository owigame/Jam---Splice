using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;
using UnityEngine.UI;

public class CreatureLinks : MonoBehaviour {

	[Header ("SpriteRenderers")]
	public SpriteRenderer head;
	public SpriteRenderer jaw;
	public SpriteRenderer body;
	public SpriteRenderer tail;
	public SpriteRenderer frontLegsR;
	public SpriteRenderer frontLegsL;
	public SpriteRenderer backLegsR;
	public SpriteRenderer backLegsL;

	[Header ("Creature Object")]
	public CreatureObject creatureObjectSetup;

	[Header ("Creature Info")]
	public float creatureHealth = 100;
	float maxCreatureHealth;

	public float creatureDamage = 0;

	[Header ("UI Links")]
	public Image healthBar;

	Animator _animator;
	Rigidbody _rigidBody;
	bool charging = false;
	bool stunned = false;
	bool chargeDamaged = false;
	CreatureLinks inTriggerZone;
	float lastScaleX = 0;
	Vector3 origScale;
	Vector3 origPos;

	SplicedCreature oldCreature;

	void Start () {
		origScale = transform.localScale;
		origPos = transform.position;
		_animator = GetComponent<Animator> ();
		_rigidBody = GetComponent<Rigidbody> ();
	}

	public void ResetCreature () {
		transform.localScale = origScale;
		transform.position = origPos;
		stunned = false;
		SetCreature (oldCreature);
	}

	public void SetCreature (SplicedCreature splicedCreature) {
		oldCreature = splicedCreature;
		head.sprite = splicedCreature.head.head;
		jaw.sprite = splicedCreature.jaw.jaw;
		body.sprite = splicedCreature.body.body;
		tail.sprite = splicedCreature.tail.tail;
		frontLegsR.sprite = splicedCreature.frontLegs.frontLegsR;
		frontLegsL.sprite = splicedCreature.frontLegs.frontLegsL;
		backLegsR.sprite = splicedCreature.backLegs.backLegsR;
		backLegsL.sprite = splicedCreature.backLegs.backLegsL;
		creatureObjectSetup = splicedCreature.body;
		LoadCreatureObjectPivots ();
		LoadCreatureObjectColliders (splicedCreature);
		jaw.transform.localPosition = splicedCreature.head.jawPivot;
		body.transform.localScale = new Vector3 (splicedCreature.body.uniformScale, splicedCreature.body.uniformScale, splicedCreature.body.uniformScale);

		creatureDamage = splicedCreature.head.damage +
			splicedCreature.jaw.damage +
			splicedCreature.body.damage +
			splicedCreature.tail.damage +
			splicedCreature.frontLegs.damage +
			splicedCreature.frontLegs.damage +
			splicedCreature.backLegs.damage +
			splicedCreature.backLegs.damage +
			splicedCreature.body.damage;

		creatureHealth = splicedCreature.head.health +
			splicedCreature.jaw.health +
			splicedCreature.body.health +
			splicedCreature.tail.health +
			splicedCreature.frontLegs.health +
			splicedCreature.frontLegs.health +
			splicedCreature.backLegs.health +
			splicedCreature.backLegs.health +
			splicedCreature.body.health;

		maxCreatureHealth = creatureHealth;
		healthBar.fillAmount = creatureHealth / maxCreatureHealth;

	}

	public void Move (float _H, float _V, float moveSpeed) {
		if (!stunned) {
			_animator.SetFloat ("Horizontal", Mathf.Abs (_H));
			_rigidBody.AddForce (Vector3.right * Time.deltaTime * moveSpeed * _H);
			_rigidBody.AddForce (Vector3.up * Time.deltaTime * moveSpeed * _V);
			transform.localScale = new Vector3 (_H < 0 ? -1 * Mathf.Abs (transform.localScale.x) : _H > 0 ? 1 * Mathf.Abs (transform.localScale.x) : transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
	}

	public void Stomp () {
		if (!stunned) {
			_animator.SetTrigger ("Stomp");
			if (inTriggerZone != null) {
				inTriggerZone.DealDamage (InputManager.instance.stompDamage * creatureDamage, InputManager.instance.stunDuration);
				CameraShake._CameraShake.DoCameraShake (0.2f, InputManager.instance.stompDamage / 500);

			}
		}
	}

	public void Charge (float chargeSpeed, float chargeWarmUp, float chargeDuration) {
		if (!stunned) {
			if (charging == false) {
				charging = true;
				_animator.SetTrigger ("Charge");
				lastScaleX = transform.localScale.x;
				StartCoroutine (ChargeRun (chargeSpeed, chargeWarmUp, chargeDuration));
			}
		}
	}

	IEnumerator ChargeRun (float chargeSpeed, float chargeWarmUp, float chargeDuration) {
		yield return new WaitForSeconds (chargeWarmUp);
		while (chargeDuration > 0 && !stunned && lastScaleX == transform.localScale.x) {
			chargeDuration -= Time.deltaTime;
			_rigidBody.AddForce ((transform.localScale.x < 0 ? Vector3.left : Vector3.right) * Time.deltaTime * chargeSpeed);
			yield return null;
		}
		charging = false;
		chargeDamaged = false;
	}

	public void Maul () {
		if (!stunned) {
			_animator.SetTrigger ("Maul");
			_animator.SetTrigger ("Maul");
		}
	}

	[Button]
	void SetCreatureObjectPivots () {
		if (creatureObjectSetup != null) {
			creatureObjectSetup.hipsPivot = transform.GetChild (0).localPosition;
			creatureObjectSetup.bodyPivot = body.transform.localPosition;
			creatureObjectSetup.headPivot = head.transform.localPosition;
			creatureObjectSetup.jawPivot = jaw.transform.localPosition;
			creatureObjectSetup.frontLegsRPivot = frontLegsR.transform.localPosition;
			creatureObjectSetup.frontLegsLPivot = frontLegsL.transform.localPosition;
			creatureObjectSetup.backLegsRPivot = backLegsR.transform.localPosition;
			creatureObjectSetup.backLegsLPivot = backLegsL.transform.localPosition;
			creatureObjectSetup.tailPivot = tail.transform.localPosition;
		}
	}

	[Button]
	void SetCreatureObjectColliders () {
		if (creatureObjectSetup != null) {
			creatureObjectSetup.bodyCol = new CreatureObject.CapsuleColliderData (body.GetComponent<CapsuleCollider> ());
			creatureObjectSetup.headCol = new CreatureObject.CapsuleColliderData (head.GetComponent<CapsuleCollider> ());
			creatureObjectSetup.frontLegsRCol = new CreatureObject.CapsuleColliderData (frontLegsR.GetComponent<CapsuleCollider> ());
			creatureObjectSetup.frontLegsLCol = new CreatureObject.CapsuleColliderData (frontLegsL.GetComponent<CapsuleCollider> ());
			creatureObjectSetup.backLegsRCol = new CreatureObject.CapsuleColliderData (backLegsR.GetComponent<CapsuleCollider> ());
			creatureObjectSetup.backLegsLCol = new CreatureObject.CapsuleColliderData (backLegsL.GetComponent<CapsuleCollider> ());
		}
	}

	[Button]
	void LoadCreatureObjectPivots () {
		if (creatureObjectSetup != null) {
			transform.GetChild (0).localPosition = creatureObjectSetup.hipsPivot;
			body.transform.localPosition = creatureObjectSetup.bodyPivot;
			head.transform.localPosition = creatureObjectSetup.headPivot;
			jaw.transform.localPosition = creatureObjectSetup.jawPivot;
			frontLegsR.transform.localPosition = creatureObjectSetup.frontLegsRPivot;
			frontLegsL.transform.localPosition = creatureObjectSetup.frontLegsLPivot;
			backLegsR.transform.localPosition = creatureObjectSetup.backLegsRPivot;
			backLegsL.transform.localPosition = creatureObjectSetup.backLegsLPivot;
			tail.transform.localPosition = creatureObjectSetup.tailPivot;
		}
	}

	[Button]
	void LoadCreatureObjectColliders () {
		if (creatureObjectSetup != null) {
			LoadColliderData (body.GetComponent<CapsuleCollider> (), creatureObjectSetup.bodyCol);
			LoadColliderData (head.GetComponent<CapsuleCollider> (), creatureObjectSetup.headCol);
			LoadColliderData (frontLegsR.GetComponent<CapsuleCollider> (), creatureObjectSetup.frontLegsRCol);
			LoadColliderData (frontLegsL.GetComponent<CapsuleCollider> (), creatureObjectSetup.frontLegsLCol);
			LoadColliderData (backLegsR.GetComponent<CapsuleCollider> (), creatureObjectSetup.backLegsRCol);
			LoadColliderData (backLegsL.GetComponent<CapsuleCollider> (), creatureObjectSetup.backLegsLCol);
		}
	}

	void LoadCreatureObjectColliders (SplicedCreature creature) {
		if (creatureObjectSetup != null) {
			LoadColliderData (body.GetComponent<CapsuleCollider> (), creature.body.bodyCol);
			LoadColliderData (head.GetComponent<CapsuleCollider> (), creature.head.headCol);
			LoadColliderData (frontLegsR.GetComponent<CapsuleCollider> (), creature.frontLegs.frontLegsRCol);
			LoadColliderData (frontLegsL.GetComponent<CapsuleCollider> (), creature.frontLegs.frontLegsLCol);
			LoadColliderData (backLegsR.GetComponent<CapsuleCollider> (), creature.backLegs.backLegsRCol);
			LoadColliderData (backLegsL.GetComponent<CapsuleCollider> (), creature.backLegs.backLegsLCol);
		}
	}

	void LoadColliderData (CapsuleCollider collider, CreatureObject.CapsuleColliderData data) {
		collider.center = data.center;
		collider.radius = data.radius;
		collider.height = data.height;
		collider.direction = data.direction;
	}

	[Button]
	void LoadCreatureSprites () {
		if (creatureObjectSetup != null) {
			head.sprite = creatureObjectSetup.head;
			jaw.sprite = creatureObjectSetup.jaw;
			body.sprite = creatureObjectSetup.body;
			tail.sprite = creatureObjectSetup.tail;
			frontLegsR.sprite = creatureObjectSetup.frontLegsR;
			frontLegsL.sprite = creatureObjectSetup.frontLegsL;
			backLegsR.sprite = creatureObjectSetup.backLegsR;
			backLegsL.sprite = creatureObjectSetup.backLegsL;
		}
	}

	public void DealDamage (float amount, float stun = 0) {
		creatureHealth -= amount / 50;
		Mathf.Clamp01 (creatureHealth);
		healthBar.fillAmount = creatureHealth / maxCreatureHealth;
		if (creatureHealth <= 0) {
			Debug.Log ("Creature Died!");
			stunned = true;
			transform.localScale = Vector3.zero;
			GameManager.instance.GameOver ();
			this.enabled = false;

		} else if (stun > 0) {
			_animator.SetTrigger ("Stun");
			StartCoroutine (Stunned (stun));
		}
	}

	IEnumerator Stunned (float stunDuration) {
		stunned = true;
		yield return new WaitForSeconds (stunDuration);
		stunned = false;
	}

	void OnCollisionEnter (Collision collisionInfo) {
		if (charging && collisionInfo.gameObject.tag == "Player" && !chargeDamaged && !stunned) {
			chargeDamaged = true;
			float damageAmount = InputManager.instance.chargeDamage * collisionInfo.impactForceSum.magnitude;
			CameraShake._CameraShake.DoCameraShake (0.01f * damageAmount, damageAmount / 100);
			Debug.Log ("Charge Damage: " + damageAmount);
			collisionInfo.gameObject.GetComponent<CreatureLinks> ().DealDamage (damageAmount * creatureDamage);
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			inTriggerZone = other.GetComponent<CreatureLinks> ();
		}
	}
	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			inTriggerZone = null;
		}
	}
}