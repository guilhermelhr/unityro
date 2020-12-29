using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class SmallBasicInfoWindow : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI line1;

    [SerializeField]
    private TextMeshProUGUI line2;

    [SerializeField]
    private TextMeshProUGUI charName;

    private void Awake() {
        Core.Session.Entity.OnParameterUpdated += OnParameterUpdated;
    }

    private void OnDestroy() {
        Core.Session.Entity.OnParameterUpdated -= OnParameterUpdated;
    }

    private void OnParameterUpdated() {
        var status = Core.Session.Entity.Status;
        charName.text = status.name;
        var exp = 0f;
        if (status.next_base_exp > 0) {
            exp = status.base_exp / status.next_base_exp * 100;
        }
        line1.text = $"Nv. {status.base_level} / {status.class_} / Nv. {status.job_level} / Exp. {exp}%";
        line2.text = $"HP. {status.hp} / {status.max_hp} | SP. {status.sp} / {status.max_sp}";
    }
}