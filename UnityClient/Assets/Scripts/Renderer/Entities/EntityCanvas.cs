using System;
using System.Collections;
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

    private Entity Entity;

    public void Init(Entity entity) {
        Entity = entity;
        Entity.OnParameterUpdated += OnEntityParameterUpdated;
    }

    private void OnDestroy() {
        if (Entity != null) {
            Entity.OnParameterUpdated -= OnEntityParameterUpdated;
        }
    }

    private void OnEntityParameterUpdated() {
        SetEntityName(Entity.Status.name);
        SetEntityHP(Entity.Status.hp, Entity.Status.max_hp);
        SetEntitySP(Entity.Status.sp, Entity.Status.max_sp);
    }

    public void SetEntityName(string name) {
        EntityName.text = name;

        Vector2 textSize = EntityName.GetPreferredValues(name);
        (EntityName.gameObject.transform as RectTransform).sizeDelta = textSize;
    }

    public void SetEntityHP(long value, long maxValue) {
        HPBar.minValue = 0f;
        HPBar.maxValue = maxValue;
        HPBar.value = value;
    }

    public void SetEntitySP(long value, long maxValue) {
        SPBar.minValue = 0f;
        SPBar.maxValue = maxValue;
        SPBar.value = value;
    }

    public void SetEntityMessage(string message) {
        EntityMessage.text = message;
        EntityMessage.transform.parent.gameObject.SetActive(true);

        Vector2 textSize = EntityMessage.GetPreferredValues(message);
        (EntityMessage.gameObject.transform as RectTransform).sizeDelta = textSize;

        StartCoroutine(HideAfterSeconds(6f, delegate {
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
