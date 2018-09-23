using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhases {
	Splice,
	Fight,
	Review
}

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	public delegate void GamePhaseEvents (GamePhases phase);
	public static GamePhaseEvents OnGamePhaseChanged;

	public static GamePhases GamePhase {
		get {
			return instance.gamePhase;
		}
	}

	public GamePhases gamePhase;

	[Header("Players")]
	public bool player1Ready = false;
	public bool player2Ready = false;

	void Awake () {
		instance = this;
		gamePhase = GamePhases.Splice;
	}

	void Start () {
		if (OnGamePhaseChanged != null) OnGamePhaseChanged (gamePhase);
	}

	public void PlayerReadyToFight(int index, bool ready){
		if (index == 0){
			player1Ready = ready;
		}
		if (index == 1){
			player2Ready = ready;
		}
		if (player1Ready && player2Ready){
			gamePhase = GamePhases.Fight;
			OnGamePhaseChanged(gamePhase);
		}
	}
}