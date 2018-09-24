using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

public class UIManageDNASelection : MonoBehaviour {

	[Header ("UI References")]
	public Button buttonGenerate;
	public Button buttonConfirm;
	public int playerIndex = 0;
	public List<CreatureObject> selectedCreatures = new List<CreatureObject> ();
	List<DNA> selectedDNA = new List<DNA> ();

	Button[] tubeButtons;
	Button[] genButtons;
	int buttonSelectedIndex = 0;
	int buttonGenSelectedIndex = 0;
	bool tubeSelection = true;
	Player _Input;
	Animator _animator;

	void Start () {
		_animator = GetComponent<Animator> ();
		_Input = ReInput.players.GetPlayer (playerIndex);
		tubeButtons = transform.GetChild (0).GetComponentsInChildren<Button> ();
		genButtons = transform.GetChild (1).GetComponentsInChildren<Button> ();
		tubeButtons[buttonSelectedIndex].Select ();

		GameManager.OnGamePhaseChanged += GamePhaseChanged;
	}

	float lastVertical = 0;
	bool lastVerticalReset = true;
	float lastHorizontal = 0;
	bool lastHorizontalReset = true;

	void GamePhaseChanged (GamePhases gamePhase) {
		if (gamePhase == GamePhases.Splice) {
			PlayerUnReady ();
		}
	}

	void Update () {
		if (GameManager.GamePhase == GamePhases.Splice) {

			if (Mathf.Abs (_Input.GetAxis ("UIHorizontal")) > 0.5f) {
				if (lastHorizontalReset) {
					Debug.Log ("UI Horizontal: " + _Input.GetAxis ("UIHorizontal"));
					lastHorizontalReset = false;
					lastHorizontal = 1;
					if (tubeSelection) {
						NextDNATube (_Input.GetAxis ("UIHorizontal") > 0);
					} else {
						NextButton (_Input.GetAxis ("UIHorizontal") > 0);
					}
				}
			}
			if (lastHorizontal <= 0 || _Input.GetAxis ("UIHorizontal") == 0) {
				lastHorizontalReset = true;
			} else if (lastHorizontal > 0) {
				lastHorizontal -= Time.deltaTime;
			}

			if (Mathf.Abs (_Input.GetAxis ("UIVertical")) > 0.5f) {
				if (lastVerticalReset) {
					Debug.Log ("UI Vertical: " + _Input.GetAxis ("UIVertical"));
					lastVerticalReset = false;
					lastVertical = 1;
					UIButtons (_Input.GetAxis ("UIVertical") > 0);
				}
			}
			if (lastVertical <= 0 || _Input.GetAxis ("UIVertical") == 0) {
				lastVerticalReset = true;
			} else if (lastVertical > 0) {
				lastVertical -= Time.deltaTime;
			}

			if (_Input.GetButtonDown ("UISubmit")) {
				if (tubeSelection) {
					AddToSelection ();
				} else {
					if (buttonGenSelectedIndex == 0) {
						CreateCreature ();
					} else {
						PlayerReady ();
					}
				}
			}

			if (_Input.GetButtonDown ("UICancel")) {
				if (!tubeSelection) {
					PlayerUnReady ();
				}
			}
		}
	}

	public void AddToSelection (int DNAIndex) {
		if (tubeSelection) {
			CreatureObject creature = SpliceManager.instance.allCreatureTypes[DNAIndex];
			if (selectedCreatures.Contains (creature)) {
				selectedCreatures.Remove (creature);
			} else {
				selectedCreatures.Add (creature);
			}
			tubeButtons[buttonSelectedIndex].GetComponent<UIButtonSelected> ().Selected ();
			buttonGenerate.interactable = selectedCreatures.Count >= 2;
		}
	}
	public void AddToSelection () {
		if (tubeSelection) {
			CreatureObject creature = SpliceManager.instance.allCreatureTypes[buttonSelectedIndex];
			if (selectedCreatures.Contains (creature)) {
				selectedCreatures.Remove (creature);
			} else {
				selectedCreatures.Add (creature);
			}
			tubeButtons[buttonSelectedIndex].GetComponent<UIButtonSelected> ().Selected ();
			buttonGenerate.interactable = selectedCreatures.Count >= 2;
		}
	}

	public void CreateCreature () {
		selectedDNA = new List<DNA> ();
		foreach (CreatureObject creature in selectedCreatures) {
			selectedDNA.Add (new DNA (creature));
		}
		SpliceManager.instance.GenerateNewCreature (playerIndex, selectedDNA);
		buttonConfirm.interactable = true;
	}

	public void PlayerReady () {
		GameManager.instance.PlayerReadyToFight (playerIndex, true);
		InputManager.instance.InitiatePlayerReadyLimbo (PlayerUnReady, playerIndex);
		_animator.SetBool ("Open", false);
	}

	public void PlayerUnReady () {
		GameManager.instance.PlayerReadyToFight (playerIndex, false);
		_animator.SetBool ("Open", true);
	}

	public void NextDNATube (bool right) {
		if (right) {
			buttonSelectedIndex = (buttonSelectedIndex < tubeButtons.Length - 1) ? buttonSelectedIndex + 1 : buttonSelectedIndex = 0;
		} else {
			buttonSelectedIndex = (buttonSelectedIndex > 0) ? buttonSelectedIndex - 1 : buttonSelectedIndex = tubeButtons.Length - 1;
		}
		tubeButtons[buttonSelectedIndex].Select ();

	}
	public void NextButton (bool right) {
		if (right) {
			genButtons[1].Select ();
			buttonGenSelectedIndex = 1;
		} else {
			genButtons[0].Select ();
			buttonGenSelectedIndex = 0;
		}

	}

	public void UIButtons (bool up) {
		if (up) {
			if (selectedCreatures.Count >= 2) {
				genButtons[0].Select ();
				buttonGenSelectedIndex = 0;
				tubeSelection = false;
			}
		} else {
			tubeButtons[0].Select ();
			buttonSelectedIndex = 0;

			tubeSelection = true;
		}
	}
}