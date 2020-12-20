using UnityEngine;
using System.Collections;
using System;

public class DamageRenderer : MonoBehaviour {

    [SerializeField]
    private TextMesh textMesh;
    private float Delay;
    private bool Ready;

    private Vector2 offset = Vector2.zero;
    private float angle = 0;
    private float start;
    private DamageType CurrentType;
    private Entity Entity;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (!Ready)
            return;

        var perc = (Core.CurrentTime - start) / Delay;
        var entityPosition = Entity.transform.position;

        float x = entityPosition.x;
        float y = entityPosition.y;
        float z = entityPosition.z;
        float scale = 1f;

        switch (CurrentType) {
            case DamageType.HEAL:
                break;
            case DamageType.MISS:
                perc = (Core.CurrentTime - start) / 800;
                scale = 0.5f;
                y = Entity.transform.position.z + 3.5f + perc * 7;
                break;
            case DamageType.DAMAGE:
                scale = (float)((1 - perc) * 4);
                x += perc * 4;
                z -= perc * 4;
                y += (float)(2 + Math.Sin(-Math.PI / 2 + (Math.PI * (0.5 + perc * 1.5))) * 5);
                break;
            case DamageType.ENEMY:
                break;
            case DamageType.COMBO:
                scale = (float)(Math.Min(perc, 0.05) * 0.75);
                y += 5 + perc;
                break;
            case DamageType.COMBO_FINAL:
                break;
            case DamageType.SP:
                break;
            default:
                break;
        }

        transform.position = new Vector3(x, y, z);
        //transform.localScale *= scale;

        var color = textMesh.color;
        color[3] = 1.0f - perc;
        textMesh.color = color;
    }

    public void Display(float amount, float tick, DamageType? type, Entity entity) {
        Entity = entity;
        CurrentType = type ?? (amount > 0 ? DamageType.DAMAGE : DamageType.MISS);
        if (entity.Type == EntityType.PC) {
            CurrentType |= DamageType.ENEMY;
        }

        var color = new Color();
        color[3] = 1.0f;
        Delay = 1500;
        start = tick;

        switch (CurrentType) {
            case DamageType.SP:
                color[0] = 0.13f;
                color[1] = 0.19f;
                color[2] = 0.75f;
                break;
            case DamageType.HEAL:
                color[1] = 1.0f;
                break;
            case DamageType.ENEMY:
                color[2] = 1.0f;
                break;
            case DamageType.COMBO:
                color[0] = 0.9f;
                color[1] = 0.9f;
                color[2] = 0.15f;
                Delay = 3000;
                break;
            default:
                color[0] = 1.0f;
                color[1] = 1.0f;
                color[2] = 1.0f;
                break;
        }

        textMesh.color = color;
        Ready = true;
        Destroy(gameObject, Delay / 1000);

        // miss
        if (amount == 0) {
            textMesh.text = "MISS";
        } else {
            textMesh.text = $"{amount}";
        }

    }
}
