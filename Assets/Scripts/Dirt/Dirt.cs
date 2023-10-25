using UnityEngine;
using FaRUtils.Systems.DateTime;
using FaRUtils.Systems.Weather;

[RequireComponent(typeof(DirtAreaHarvest))]
public class Dirt : MonoBehaviour
{
    public bool _isWet;

    public bool testing;

    public GameObject colliders;

    public int abilityLevelPlaceholder = 1;

    public bool IsEmpty { get; private set; }
    public GameObject violeta { get; private set; }
    public GameObject currentCrop { get; private set; }
    public SeedItemData currentCropData { get; private set; }
    public CropSaveData cropSaveData { get; private set; }

    public GameObject TextureAnimation;
    public static Color wetDirtColor = new(0.5f, 0.3f, 0.3f);

    void Start()
    {
        //LoadData(GetComponent<DirtLoadData>().Load());

        IsEmpty = true;
        DateTime.OnHourChanged.AddListener(DryDirt);
        Cama.Instance.SaveDataEvent.AddListener(SaveData);
        WeatherManager.Instance.IsRaining.AddListener(DirtIsWet);
    }

    private void SaveData(bool isTemporarySave)
    {
        var dirtSaveData = new DirtSaveData(_isWet, IsEmpty, currentCrop, currentCropData, cropSaveData);
        DirtSaver.instance.WriteSave(dirtSaveData, isTemporarySave);
    }

    private void LoadData(DirtSaveData data)
    {
        _isWet = data._isWet;
        IsEmpty = data.IsEmpty;
        currentCrop = data.currentCrop;
        currentCropData = data.currentCropData;
        cropSaveData = data.cropSaveData;
    }

    public bool GetCrop(SeedItemData itemData)
    {
        IsEmpty = false;

        GameObject instantiated = GameObject.Instantiate(itemData.DirtPrefab, transform.position, GridGhost.Rotation(), transform);

        currentCrop = instantiated;
        currentCropData = itemData;

        GridGhost.UpdateRandomSeed();
        return (instantiated != null);
    }

    public void DirtIsWet()
    {
        _isWet = true;
        this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.color = wetDirtColor;
    }

    public void GetDown()
    {
        //AND MOVE IT ALL AROUND
        colliders.transform.position = new Vector3(colliders.transform.position.x, -2, colliders.transform.position.z);
    }

    public void DryDirt(int hour)
    {
        if (testing) return;

        if (hour != 5) return;

        _isWet = false;
        this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.white;
    }

    void OnEnable()
    {
        TextureAnimation = GetComponentInChildren<Animation>().gameObject;
        TextureAnimation.GetComponent<Animation>().enabled = true;
    }

    void OnDisable()
    {
        currentCrop = null;
        currentCropData = null;
        IsEmpty = true;
        _isWet = false;
        TextureAnimation.GetComponent<Animation>().clip.SampleAnimation(TextureAnimation, 0f);
        colliders.transform.position = this.transform.position;
        DateTime.OnHourChanged.RemoveListener(DryDirt);
        WeatherManager.Instance.IsRaining.RemoveListener(DirtIsWet);
    }
}
