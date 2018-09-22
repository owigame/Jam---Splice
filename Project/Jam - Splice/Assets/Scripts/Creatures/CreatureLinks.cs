using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureLinks : MonoBehaviour {

	[Header ("SpriteRenderers")]
	public SpriteRenderer head;
	public SpriteRenderer jaw;
	public SpriteRenderer body;
	public SpriteRenderer tail;
	public SpriteRenderer[] frontLegs;
	public SpriteRenderer[] backLegs;

	Animator _animator;

	void Start () {
		_animator = GetComponent<Animator> ();
	}

	public void SetCreature (SplicedCreature splicedCreature) {
		head.sprite = splicedCreature.head.head;
		jaw.sprite = splicedCreature.jaw.jaw;
		body.sprite = splicedCreature.body.body;
		tail.sprite = splicedCreature.tail.tail;
		foreach (SpriteRenderer sprite in frontLegs) {
			sprite.sprite = splicedCreature.frontLegs.frontLegs;
		}
		foreach (SpriteRenderer sprite in backLegs) {
			sprite.sprite = splicedCreature.backLegs.backLegs;
		}
	}

	public void Move (float amount, float moveSpeed) {
		_animator.SetFloat ("Horizontal", Mathf.Abs (amount));
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed * Mathf.Abs (amount));
		transform.localEulerAngles = new Vector3 (0, amount < 0 ? 180 : 0, 0);
	}

	public void Stomp () {
		_animator.SetTrigger ("Stomp");
	}

	public void Charge (float chargeSpeed, float chargeWarmUp, float chargeDuration) {
		_animator.SetTrigger ("Charge");
		StartCoroutine (ChargeRun (chargeSpeed, chargeWarmUp, chargeDuration));
	}

	IEnumerator ChargeRun (float chargeSpeed, float chargeWarmUp, float chargeDuration) {
		yield return new WaitForSeconds (chargeWarmUp);
		while (chargeDuration > 0) {
			chargeDuration -= Time.deltaTime;
			transform.Translate (Vector3.right * Time.deltaTime * chargeSpeed);
			yield return null;
		}
	}

	public void Maul () {
		_animator.SetTrigger ("Maul");
	}
}