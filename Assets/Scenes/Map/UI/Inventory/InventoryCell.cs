using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class InventoryCell : MonoBehaviour {

    [SerializeField]
    private UIItem Item;

    public void SetItem(ItemInfo item) {
        Item.SetItem(item);
    }
}
