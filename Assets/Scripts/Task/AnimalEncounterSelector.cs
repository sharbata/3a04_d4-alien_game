using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AnimalEncounterSelector : MonoBehaviour
{
    private ArrayList animals = new ArrayList {"Bear", "Hyena", "Wolf"};
    
    [SerializeField] private SparController sparController;
    
    [SerializeField] private Text animalEncounterText;
    
    public void EncounterRandomAnimal()
    {
        int animalIndex = Random.Range(0, animals.Count);
        string animal = (string) animals[animalIndex];

        string animalEncounterMessage = "";
        float damageMultiplier = 1.3f;
        switch (animal.ToLower())
        {
            case "bear":
                animalEncounterMessage = "You hear leaves rustling behind you and turn around to find a bear staring at you hungrily";
                damageMultiplier = 2.0f;
                break;
            case "hyena":
                animalEncounterMessage = "You sensed it before you saw it. A wild hyena. Staring right at you.";
                damageMultiplier = 1.5f;
                break;
            case "wolf":
                animalEncounterMessage = "You hear a low growl behind you and turn around to find a very hungry wolf";
                damageMultiplier = 1.3f;
                break;
        }

        animalEncounterText.text = animalEncounterMessage;
        sparController.opponentDamageMultiplier = damageMultiplier;
    }
}
