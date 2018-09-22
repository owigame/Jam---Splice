using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

public class SpliceManager : MonoBehaviour {

	public List<CreatureObject> allCreatureTypes = new List<CreatureObject> ();

	[Header ("Player Creatures")]
	public CreatureLinks splicedCreature1;
	public CreatureLinks splicedCreature2;

	void Start () {

	}

	void Update () {

	}

	[Header ("Test DNA")]
	public List<DNA> splicedDNA = new List<DNA> ();

	[Header ("Test DNA Result")]
	public SplicedCreature splicedCreature;

	[Button]
	void TestNewCreature () {
		splicedCreature1.SetCreature(GetNewSplicedCreature (splicedDNA));
		splicedCreature2.SetCreature(GetNewSplicedCreature (splicedDNA));
	}

	public SplicedCreature GetNewSplicedCreature (List<DNA> splicedDNA) {
		return new SplicedCreature (splicedDNA);
	}
}