using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityCanvas : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI EntityName;
    [SerializeField] private TextMeshProUGUI StoreName;
    [SerializeField] private TextMeshProUGUI ChatRoomName;
    [SerializeField] private TextMeshProUGUI EntityMessage;
    [SerializeField] private Slider HPBar;
    [SerializeField] private Slider SPBar;

    public void SetEntityName(string name) {
        EntityName.text = name;
        EntityName.autoSizeTextContainer = true;

        Vector2 textSize = EntityName.GetPreferredValues(name);
        (EntityName.gameObject.transform as RectTransform).sizeDelta = textSize;
    }

    public void SetEntityHP(int value, int maxValue) {
        HPBar.minValue = 0f;
        HPBar.maxValue = maxValue;
        HPBar.value = value;
    }

    public void SetEntitySP(int value, int maxValue) {
        SPBar.minValue = 0f;
        SPBar.maxValue = maxValue;
        SPBar.value = value;
    }

    public void SetEntityMessage(string message) {
        EntityMessage.text = message;
        EntityMessage.autoSizeTextContainer = true;
        EntityMessage.transform.parent.gameObject.SetActive(true);

        Vector2 textSize = EntityMessage.GetPreferredValues(message);
        (EntityMessage.gameObject.transform as RectTransform).sizeDelta = textSize;

        StartCoroutine(HideAfterSeconds(3f, delegate {
            EntityMessage.transform.parent.gameObject.SetActive(false);
        }));
    }

    internal void ShowEntityName() {
        EntityName.gameObject.SetActive(true);
    }

    internal void HideEntityName() {
        EntityName.gameObject.SetActive(false);
    }

    internal void ShowEntityHP() {
        HPBar.gameObject.SetActive(true);
    }

    internal void HideEntityHP() {
        HPBar.gameObject.SetActive(false);
    }

    internal void ShowEntitySP() {
        SPBar.gameObject.SetActive(true);
    }

    internal void HideEntitySP() {
        SPBar.gameObject.SetActive(false);
    }

    private IEnumerator HideAfterSeconds(float seconds, Action callback) {
        yield return new WaitForSeconds(seconds);
        callback.Invoke();
    }
}
