using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Specimen/Create new specimen")]
public class SpecimenBase : ScriptableObject
{
    [SerializeField] int specimenId;

    [SerializeField] int experience;
    
    [SerializeField] int energyPoints;

    [SerializeField] int physicalHealth;

    [SerializeField] int intelligence;

    [SerializeField] int mentalHealth;

    [SerializeField] int strength;

    [SerializeField] int creativity;

    [SerializeField] int resilience;

    [SerializeField] List<object> tasksCompleted;

    [SerializeField] List<object> itemsPurchased;

    public int SpecimenId {
        get {
            return specimenId;
        }
    }

    public int Experience {
        get {
            return experience;
        }

        set {
            experience = value;
        }
    }

    public int PhysicalHealth {
        get {
            return physicalHealth;
        }

        set {
            physicalHealth = value;
        }
    }
    
    public int EnergyPoints {
        get => energyPoints;

        set => energyPoints = value;
    }

    public int Intelligence {
        get {
            return intelligence;
        }

        set {
            intelligence = value;
        }
    }

    public int MentalHealth {
        get {
            return mentalHealth;
        }

        set {
            mentalHealth = value;
        }
    }

    public int Strength {
        get {
            return strength;
        }

        set {
            strength = value;
        }
    }

    public int Creativity {
        get {
            return creativity;
        }

        set {
            creativity = value;
        }
    }

    public int Resilience {
        get {
            return resilience;
        }

        set {
            resilience = value;
        }
    }

    public List<object> TasksCompleted {
        get {
            return tasksCompleted;
        }
    }

    public List<object> ItemsPurchased {
        get {
            return itemsPurchased;
        }
    }
}



//     public void PurchaseItem(Item Item) {
//         if (!ItemsPurchased.Contains(Item)) {
//             ItemsPurchased.Add(Item);
//         }
//     }

//     [SerializeField] int AddCompletedTask;

//     [SerializeField] int ReturnSpecimenInformation;

//     [SerializeField] List<Task> TasksCompleted;

//     [SerializeField] List<Item> ItemsPurchased;

//     public Specimen(){
//         SpecimenId = Guid.NewGuid();
//         experience = 0;
//         PhysiaclHealth = 100;
//         Intelligence = 1;
//         MentalHealth = 30;
//         Strength = 1;
//         Creativity = 1;
//         Resilience = 1;

//     }
// }

// public class Task
// {
//     [SerializeField] Guid TaskId;

//     [SerializeField] string TaskTitle;

//     [SerializeField] string TaskObjective;

//     [SerializeField] string TaskInstruction;

//     [SerializeField] int TaskSuccessRequirement;

//     [SerializeField] Scene Map;

//     public Task(string title, string objective, string instruction, int successRequirement, Scene Map)
//     {
//         // AUTOGENERATE TaskId
//         TaskId = Guid.NewGuid();
//         TaskObjective = objective;
//         TaskTitle = title;
//         TaskInstruction = instruction;
//         TaskSuccessRequirement = successRequirement;
//     }
// }

// public class Item
// {

// }