using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	public Animator splicePhaseUI;
	public Animator fightPhaseUI;

	void Start () {
		GameManager.OnGamePhaseChanged += GamePhaseChanged;
	}

	void GamePhaseChanged (GamePhases gamePhase) {
		switch (gamePhase) {
			case GamePhases.Fight:
				splicePhaseUI.SetBool("Open", false);
				fightPhaseUI.SetBool("Open", true);
				break;
			case GamePhases.Splice:
				splicePhaseUI.SetBool("Open", true);
				fightPhaseUI.SetBool("Open", false);
				break;
		}
	}
}