using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class InputManager : MonoBehaviour {

	public static InputManager instance;

	[Header ("Move Parameters")]
	public float moveSpeed = 1;

	[Header ("Charge Parameters")]
	public float chargeSpeed = 2;
	public float chargeWarmUp = 1;
	public float chargeDuration = 1;
	public float chargeDamage = 30;

	[Header("Stomp Parameters")]
	public float stompDamage = 20;
	public float stunDuration = 3;

	[Header ("Player Creatures")]
	List<Player> _Input = new List<Player> ();
	public List<CreatureLinks> playerCreatures = new List<CreatureLinks> ();

	void Awake()
	{
		instance = this;
	}

	void Start () {
		_Input.Add (ReInput.players.GetPlayer (0));
		_Input.Add (ReInput.players.GetPlayer (1));
	}

	void Update () {
		foreach (Player input in _Input) {
			Move (input, input.GetAxis ("Horizontal"), input.GetAxis ("Vertical"));
			if (input.GetButtonDown ("Charge")) {
				Charge (input);
			}
			if (input.GetButtonDown ("Stomp")) {
				Stomp (input);
			}
			if (input.GetButtonDown ("Maul")) {
				Maul (input);
			}
		}
	}

	void Move (Player input, float _H, float _V) {
		playerCreatures[_Input.IndexOf (input)].Move (_H, _V, moveSpeed);
	}

	void Charge (Player input) {
		playerCreatures[_Input.IndexOf (input)].Charge (chargeSpeed, chargeWarmUp, chargeDuration);
	}
	void Stomp (Player input) {
		playerCreatures[_Input.IndexOf (input)].Stomp ();
	}
	void Maul (Player input) {
		playerCreatures[_Input.IndexOf (input)].Maul ();
	}
}