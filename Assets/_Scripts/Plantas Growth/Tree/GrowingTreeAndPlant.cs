using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GrowingTreeAndPlant : GrowingBase
{

    
    [SerializeField] protected List<Transform> spawnPoints;
    public List<GameObject> fruits;
    public Dirt Tierra;

    [Header("Fruits Settings")]
    [SerializeField] protected int daysToGiveFruits = 1;
    [SerializeField] protected int minFruitsToSpawn;
    [SerializeField] protected int maxFruitsToSpawn;
    protected int daysWithoutFruitsCounter;
    protected int _reGrowCounter; //Veces que volvio a dar frutos.
    public int ReGrowMaxTimes; //Veces maxima que puede volver a dar frutos.

    [FormerlySerializedAs("Prefab")] public GameObject fruitPrefab;

    [HideInInspector] public List<Transform> SpawnPoints => spawnPoints;
    private HashSet<Transform> availableSpawnPoints = new HashSet<Transform>();
    [HideInInspector] public int RandInt;
    [HideInInspector] public int ExpectedInt;

    public int ReGrowCounter => _reGrowCounter;

    protected override void Start()
    {
        base.Start();
        availableSpawnPoints = SpawnPoints.ToHashSet();
    }

    protected override void CatchUpMissedGrowth()
    {
        throw new System.NotImplementedException();
    }

    public override void OnHourChanged(int hour)
    {
        if (hour != 5) return;
        daysPlanted++;
            
        CheckDayGrow();
        
        if (currentState.isLastPhase) SpawnFruits(minFruitsToSpawn, maxFruitsToSpawn);
        
    }

    public Transform GetRandomSpawnPoint()
    {
        if (availableSpawnPoints.Count == 0) return null;
        
        Transform point = availableSpawnPoints.ElementAt(Random.Range(0, availableSpawnPoints.Count));

        availableSpawnPoints.Remove(point);

        return point;
    }

    public virtual void SpawnFruits(int minFruits = 10, int maxFruits = 15)
    {
        if (_reGrowCounter == ReGrowMaxTimes) return;
        
        daysWithoutFruitsCounter = 0;

        RandInt = Random.Range(minFruits, maxFruits);

        for (int i = 0; i < RandInt; i++)
        {
            Transform spawnPoint = GetRandomSpawnPoint();
            if (spawnPoint == null) return;
            
            GameObject fruit = Instantiate(fruitPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
            fruits.Add(fruit.transform.gameObject);
        }
    }

    protected virtual void ResetSpawnPoints()
    {
        availableSpawnPoints = spawnPoints.ToHashSet();
    }

    public void DestroyThisBush()
    {
        StartCoroutine(BushCedeLaPresidencia());
    }
    public virtual IEnumerator BushCedeLaPresidencia() //LA CONCHA DE TU MADRE SATIA QUE NOMBRE DE MIERDA.
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(transform.parent.gameObject);
    }
}
