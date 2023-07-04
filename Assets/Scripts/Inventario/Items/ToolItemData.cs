using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jueguito Granjil/Inventario/ToolItem")]
public class ToolItemData : InventoryItemData
{
    public int energyCost;
   // private PauseMenu _pauseMenu;
    private GridGhost _gridGhost()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<GridGhost>();
    }

    public override bool UseItem()
    {
        //_pauseMenu = PauseMenu.Instance;
        if (!PauseMenu.GameIsPaused)
        {
            if (IsHoe)
            {
                return UseHoe();
            }

            if (IsAxe)
            {
                //return UseAxe();
            }

            if (IsBucket)
            {
                return UseBucket();
            }
        }
        return true;
    }
    private bool UseHoe()
    {
        if (_gridGhost().CheckDirt(_gridGhost().finalPosition, 0.1f) == null)
        {
            if(Energy.instance.TryUseAndAnimateEnergy(1, 2f))
            {
                _gridGhost().PlantDirt();
                return true;
            }
            else return false;
            
            //Energy.UseEnergy(energyCost);
           // Energy.UpdateEnergy();
        }
        else return false;
    }

    private bool UseBucket()
    {
        Dirt _dirt = _gridGhost().CheckDirt(_gridGhost().finalPosition, 0.1f);
        if (_dirt != null)
        {
            _dirt.DirtIsWet();
            return true;
        }
        else return false;
    }

}
