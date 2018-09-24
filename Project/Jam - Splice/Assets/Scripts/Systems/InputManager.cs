using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour {

	public static InputManager instance;

	[Header ("Move Parameters")]
	public float moveSpeed = 1;

	[Header ("Charge Parameters")]
	public float chargeSpeed = 2;
	public float chargeWarmUp = 1;
	public float chargeDuration = 1;
	public float chargeDamage = 30;

	[Header ("Stomp Parameters")]
	public float stompDamage = 20;
	public float stunDuration = 3;

	[Header ("Player Creatures")]
	List<Player> _Input = new List<Player> ();
	public List<CreatureLinks> playerCreatures = new List<CreatureLinks> ();

	[Header ("Player UI")]
	public UIManageDNASelection[] uiManagerDNASelection;

	void Awake () {
		instance = this;
	}

	void Start () {
		GameManager.OnGamePhaseChanged += GamePhaseChanged;

		_Input.Add (ReInput.players.GetPlayer (0));
		_Input.Add (ReInput.players.GetPlayer (1));
	}

	IEnumerator FightPhase () {
		while (GameManager.GamePhase == GamePhases.Fight) {
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
			yield return null;
		}
	}

	IEnumerator SplicePhase () {
		while (GameManager.GamePhase == GamePhases.Splice) {

			yield return null;
		}
	}

	void GamePhaseChanged (GamePhases gamePhase) {
		switch (gamePhase) {
			case GamePhases.Fight:
				foreach (Player input in _Input) {
					input.controllers.maps.SetAllMapsEnabled (false);
					input.controllers.maps.SetMapsEnabled (true, "Fight");
				}
				StartCoroutine (FightPhase ());
				break;
			case GamePhases.Splice:
				foreach (Player input in _Input) {
					input.controllers.maps.SetAllMapsEnabled (false);
					input.controllers.maps.SetMapsEnabled (true, "Splice");
				}
				foreach (CreatureLinks link in playerCreatures) {
					link.enabled = true;
					link.ResetCreature();
				}
				StartCoroutine (SplicePhase ());
				break;
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

	public void InitiatePlayerReadyLimbo (UnityAction returnCall, int playerIndex) {
		PlayerReadyLimbo (returnCall, playerIndex);
	}

	IEnumerator PlayerReadyLimbo (UnityAction returnCall, int playerIndex) {
		bool cancel = false;
		while (GameManager.GamePhase == GamePhases.Splice && cancel == false) {
			if (_Input[playerIndex].GetButtonDown ("UICancel")) {
				returnCall.Invoke ();
				cancel = true;
			}
			yield return null;
		}
	}
}