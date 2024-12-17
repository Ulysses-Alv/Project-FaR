using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FaRUtils;

public class CropExplodeBush : MonoBehaviour
{
    public GameObject Tierra = null;
    public GameObject Coso;
    public GameObject Parent;
    //bool YaExploto = false;
    public InventoryItemData ItemData;
    public GameObject jugador;
    public GameObject Colliders;
    public GameObject Crop;
    public GameObject CropLeaf;


    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        Tierra = transform.parent.gameObject.GetComponent<Dirt>().gameObject;
    }
    

    public void Chau(GameObject FrutillaObj)
    {
        Tierra = transform.parent.gameObject.GetComponent<Dirt>().gameObject;
        Vector3 pos = new Vector3 (Tierra.transform.position.x, 2, Tierra.transform.position.z);
        var inventory = jugador.transform.GetComponent<PlayerInventoryHolder>();
        inventory.AñadirAInventario(ItemData, 1);
        Instantiate(Coso, pos, Quaternion.identity);
        Crop = FrutillaObj;
        StartCoroutine(Destruir());
    }

    IEnumerator Destruir()
    {
        //CropLeaf.GetComponent<MeshRenderer>().enabled = false;
        Crop.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);
    }
}
