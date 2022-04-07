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

    public List<object> PurchaseItem {
        set {
            if (!_base.ItemsPurchased.Contains(value)){
                _base.ItemsPurchased.Add(value);
            }
        }
    }

    public List<object> CompleteTask {
        set {
            if (!_base.TasksCompleted.Contains(value)){
                _base.TasksCompleted.Add(value);
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

