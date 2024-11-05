using UnityEngine;
using FaRUtils.FPSController;
using UnityEngine.Events;
using Utils;

[RequireComponent(typeof(UniqueID))]
public class Cofre : Container, IInteractable
{
    [SerializeField] private GameObject _prompt;
    [SerializeField] private Animator _animator;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");
    
    public GameObject InteractionPrompt => _prompt;

    protected void Awake()
    {
        _prompt = GameObject.FindGameObjectWithTag("HouseInteraction");
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        if (_animator != null)
        { 
            _animator.SetBool(IsOpen, true);
        }
        OnDynamicInventoryDisplayRequested?.Invoke(inventorySystem, 0);
        interactSuccessful = true;
    }

    public void InteractOut()
    {
        this.Log("InteractOut");
    }

    public void EndInteraction()
    {
        if (_animator != null)
        { 
            _animator.SetBool(IsOpen, false);
        }
    }

    public void LoadData(ChestData data)
    {
        inventorySystem = new InventorySystem(data.inventorySystem);
        transform.position = data.position;
    }
}