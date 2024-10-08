﻿using System;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;
using DateTime = FaRUtils.Systems.DateTime.DateTime;

public class GrowingBase : MonoBehaviour
{
    protected int interactableLayerInt = 7;

    protected int daysPlanted; //Dias que pasaron desde que se plantó.
    [SerializeField] protected int maxDaysWithoutHarvest;
    protected int daysWithoutHarvest;
    [SerializeField] protected int maxDaysDry;
    protected int daysDry;

    public bool isFruit;

    [SerializeField] protected GrowingState[] states;
    
    [Tooltip("Always reference first state in prefab")]
    public GrowingState currentState; // TODO: Cambiar a protected y que los demás objetos hagan Get de una propiedad, pero me da paja

    [HideInInspector] public MeshFilter meshFilter;
    [HideInInspector] public MeshCollider meshCollider;
    [HideInInspector] public MeshRenderer meshRenderer;

    public int DiasPlantado => daysPlanted;

    protected virtual void Awake()
    {
        TryGetComponent(out meshFilter);
        TryGetComponent(out meshCollider);
        TryGetComponent(out meshRenderer);
    }

    protected virtual void Start()
    {
        /*
        TryGetComponent(out meshFilter);
        TryGetComponent(out meshCollider);
        TryGetComponent(out meshRenderer);
        */
        
        
        /*if (TryGetComponent<MeshFilter>(out MeshFilter filter))
        {
            meshFilter = filter;
        }
        if (TryGetComponent<MeshCollider>(out MeshCollider col))
        {
            meshCollider = col;
        }
        if (TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
        {
            meshRenderer = renderer;
        }*/
        
        
        DateTime.OnHourChanged.AddListener(OnHourChanged);
    }

    public virtual void OnHourChanged(int hour) { }

    public virtual void CheckDayGrow() //SE FIJA LOS DIAS DEL CRECIMIENTO.
    {
        GrowingState lastState = currentState;
        currentState = states.FirstOrDefault<GrowingState>(state => state.IsThisState(daysPlanted));
        
        if(currentState != lastState) UpdateState(); // Only change mesh data if changed state
        
    }

    protected virtual void UpdateState()
    {
        //Debug.Log($"Current State: {currentState.name}");
        meshFilter.mesh = currentState.mesh;
        meshRenderer.material = currentState.material;
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = currentState.mesh;
        }
        if (currentState.isLastPhase) SetInteractable();
    }

    public virtual void SetInteractable()
    {
        if (isFruit) return;

        gameObject.layer = interactableLayerInt; //layer interactuable.
    }

    public void LoadData(CropSaveData cropSaveData)
    {
        daysPlanted = cropSaveData.DiasPlantado;
        currentState = cropSaveData.GrowingState;
        UpdateState();
    }

    void OnDisable()
    {
        DateTime.OnHourChanged.RemoveListener(OnHourChanged);
    }
}
