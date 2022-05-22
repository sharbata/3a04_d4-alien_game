using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create new item")]

public class ItemBase : ScriptableObject
{
    [SerializeField] string name;
    [SerializeField] string description;
    [SerializeField] Sprite icon;
    [SerializeField] int expInc;
    [SerializeField] int physHPInc;
    [SerializeField] int intellInc;
    [SerializeField] int mentalHPInc;
    [SerializeField] int strengthInc;
    [SerializeField] int creativityInc;
    [SerializeField] int resilienceInc;
    public string Name => name;
    public string Description => description;
    public Sprite Icon => icon;

    

}
