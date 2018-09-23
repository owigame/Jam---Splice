using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManageDNASelection : MonoBehaviour {

	[Header ("UI References")]
	public Button buttonGenerate;
	public Button buttonConfirm;
	public int playerIndex = 0;
	public List<CreatureObject> selectedCreatures = new List<CreatureObject> ();
	List<DNA> selectedDNA = new List<DNA> ();

	void Start () {

	}

	void Update () {

	}

	public void AddToSelection (int DNAIndex) {
		CreatureObject creature = SpliceManager.instance.allCreatureTypes[DNAIndex];
		if (selectedCreatures.Contains (creature)) {
			selectedCreatures.Remove (creature);
		} else {
			selectedCreatures.Add (creature);
		}
		buttonGenerate.interactable = selectedCreatures.Count >= 2;
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
		InputManager.instance.InitiatePlayerReadyLimbo(PlayerUnReady, playerIndex);
	}

	public void PlayerUnReady () {
		GameManager.instance.PlayerReadyToFight (playerIndex, false);
	}
}