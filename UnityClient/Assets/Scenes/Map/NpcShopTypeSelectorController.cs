using System;
using UnityEngine;

public class NpcShopTypeSelectorController : MonoBehaviour {

    private uint NAID;

    internal void DisplayDealTypeSelector(ushort cmd, int size, InPacket packet) {
        if (packet is ZC.SELECT_DEALTYPE SELECT_DEALTYPE) {
            NAID = SELECT_DEALTYPE.NAID;
        }

        gameObject.SetActive(true);
    }

    public void Cancel() {
        gameObject.SetActive(false);
        NAID = 0;
    }

    public void SeletDealType(bool isBuying) {
        new CZ.ACK_SELECT_DEALTYPE {
            NAID = NAID,
            Type = (byte)(isBuying ? 0 : 1)
        }.Send();
        Cancel();
    }
}
