using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specimen
{
    SpecimenBase _base;
    int level;

    public Specimen(SpecimenBase pBase, int pLevel){
        _base = pBase;
        level = pLevel;
    }

    public void PurchaseItem {
        set {
            if (!_base.itemsPurchased.Contains(value)){
                _base.itemsPurchased.Add(value);
            }
        }
    }

    public void CompleteTask {
        set {
            if (!_base.tasksCompleted.Contains(value)){
                _base.tasksCompleted.Add(value);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

