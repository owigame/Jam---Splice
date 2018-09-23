using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

public class SpliceManager : MonoBehaviour {
	public static SpliceManager instance;

	public List<CreatureObject> allCreatureTypes = new List<CreatureObject> ();

	[Header ("Player Creatures")]
	public CreatureLinks splicedCreature1;
	public CreatureLinks splicedCreature2;

	void Awake () {
		instance = this;
	}

	void Start () {
		TestNewCreature ();
	}

	void Update () {

	}

	[Header ("Test DNA")]
	public List<DNA> splicedDNA = new List<DNA> ();

	[Header ("Test DNA Result")]
	public SplicedCreature splicedCreature;

	[Button]
	void TestNewCreature () {
		splicedCreature1.SetCreature (GetNewSplicedCreature (splicedDNA));
		splicedCreature2.SetCreature (GetNewSplicedCreature (splicedDNA));
	}

	public SplicedCreature GetNewSplicedCreature (List<DNA> splicedDNA) {
		return new SplicedCreature (splicedDNA);
	}

	public void GenerateNewCreature (int playerIndex, List<DNA> splicedDNA) {
		CreatureLinks playerCreatureLinks = playerIndex == 0 ? splicedCreature1 : splicedCreature2;
		playerCreatureLinks.gameObject.SetActive(true);
		playerCreatureLinks.SetCreature (GetNewSplicedCreature (splicedDNA));
	}
}