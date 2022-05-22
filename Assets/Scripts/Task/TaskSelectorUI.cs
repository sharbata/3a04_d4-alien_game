using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskSelectorUI : MonoBehaviour
{
    [SerializeField] private SpecimenBase specimenBase;
    [SerializeField] private Button startButton;
    private void Update()
    {
        startButton.interactable = specimenBase.EnergyPoints > 0;
    }
}
