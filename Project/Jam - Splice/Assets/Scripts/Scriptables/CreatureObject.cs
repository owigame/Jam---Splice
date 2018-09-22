using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreatureObject", menuName = "Creatures/New Creature", order = 0)]

[System.Serializable]
public class CreatureObject : ScriptableObject {

	[Header("Prefab")]
	public GameObject creaturePrefab;

	[Header("Sprites")]
	public Sprite head;
	public Sprite jaw;
	public Sprite body;
	public Sprite tail;
	public Sprite frontLegs;
	public Sprite backLegs;


	void Start () {
		
	}
	
	void Update () {
		
	}
}
