using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SplicedCreature {
    //Percentages of creatures
    //Creature parts
    //Special Attribute
    //Ability Damage - Charge, Strike, Stomp, Tail

    [Header ("DNA Structure")]
    public List<DNA> splicedDNA;

    [Header ("Body Structure")]
    public CreatureObject head;
    public CreatureObject jaw;
    public CreatureObject body;
    public CreatureObject tail;
    public CreatureObject frontLegs;
    public CreatureObject backLegs;

    public SplicedCreature (List<DNA> _splicedDNA) {
        splicedDNA = _splicedDNA;
        GenerateSplicedCreature ();
    }

    void GenerateSplicedCreature () {
        Debug.Log ("Generating new Spliced Creature");
        List<int> bodyPartIndecies = new List<int> { 0, 1, 2, 3, 4 };
        int varieties = bodyPartIndecies.Count / splicedDNA.Count;
        int remainder = bodyPartIndecies.Count - (varieties * splicedDNA.Count);
        Debug.Log ("Varieties: " + varieties);
        List<int> varietyCounter = new List<int> ();
        for (int i = 0; i < splicedDNA.Count; i++) {
            varietyCounter.Add (varieties);
        }
        while (remainder > 0){
            varietyCounter[Random.Range(0,varietyCounter.Count)] ++;
            remainder --;
        }
        Debug.Log ("Variety Counter: " + varietyCounter[0] + ", " + varietyCounter[1]);

        for (int i = 0; i < varietyCounter.Count; i++) {
            while (varietyCounter[i] > 0) {
                int newIndex = Random.Range (0, bodyPartIndecies.Count);

                switch (bodyPartIndecies[newIndex]) {
                    case 0:
                        head = splicedDNA[i].creatureType;
                        jaw = splicedDNA[i].creatureType;
                        break;
                    case 1:
                        body = splicedDNA[i].creatureType;
                        break;
                    case 2:
                        tail = splicedDNA[i].creatureType;
                        break;
                    case 3:
                        frontLegs = splicedDNA[i].creatureType;
                        break;
                    case 4:
                        backLegs = splicedDNA[i].creatureType;
                        break;
                }

                varietyCounter[i]--;
                bodyPartIndecies.RemoveAt (newIndex);
            }
        }
    }
}