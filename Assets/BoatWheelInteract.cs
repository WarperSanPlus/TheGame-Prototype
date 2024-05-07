using Controllers;
using Interfaces;
using Singletons;
using UnityEngine;

public class BoatWheelInteract : MonoBehaviour, Interactable
{
    [SerializeField]
    private BoatController boat;

    public void OnClick() => ControllerManager.SwitchTo(this.boat);
}
