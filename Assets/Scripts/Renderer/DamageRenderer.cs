using UnityEngine;
using System.Collections;
using System;

public class DamageRenderer : MonoBehaviour {

    [SerializeField]
    private TextMesh textMesh;
    private float Delay;
    private bool Ready;

    private double start;
    private DamageType CurrentType;

    float x = 0;
    float y = 0;
    float z = 0;
    float scale = 1f;
    float angle = MathHelper.ToRadians(50);

    float angleMultiplier = 3f;
    float yMultiplier = 0.9f;
    float xMultiplier = 0.1f;

    void Update() {
        if(!Ready)
            return;

        float perc = (float)((Core.Tick - start) / Delay);

        if((CurrentType & DamageType.COMBO) > 0) {
            scale = (float)(Math.Min(perc, 0.05) * 0.75);
            z += 5 + perc;
        } else if((CurrentType & DamageType.DAMAGE) > 0) {
            scale = (float)((1 - perc) * 4);

            angle += Time.deltaTime * angleMultiplier;
            y += ((float)Math.Cos(angle)) * yMultiplier;
            x += ((float)Math.Sin(angle)) * xMultiplier;

            transform.position += new Vector3(x, y, 0) * Time.deltaTime;
        } else if((CurrentType & DamageType.HEAL) > 0) {

        } else if((CurrentType & DamageType.MISS) > 0) {
            perc = (float)((Core.Tick - start) / 800);
            scale = 0.5f;
            transform.position += Vector3.up * Time.deltaTime * 7;
        }

        //Debug.Log(transform.position);
        //transform.localScale *= scale;

        var color = textMesh.color;
        color[3] -= 0.3f * Time.deltaTime;
        textMesh.color = color;
    }

    public void Display(float amount, double tick, DamageType? type, Entity entity) {
        transform.position = entity.transform.position;

        CurrentType = (type ?? (amount > 0 ? DamageType.DAMAGE : DamageType.MISS));
        if(entity.Type == EntityType.PC) {
            CurrentType |= DamageType.ENEMY;
        }

        var color = new Color();
        color[3] = 1.0f;
        Delay = 1500;
        start = tick;

        if((CurrentType & DamageType.SP) > 0) {
            color[0] = 0.13f;
            color[1] = 0.19f;
            color[2] = 0.75f;
        } else if((CurrentType & DamageType.HEAL) > 0) {
            color[1] = 1.0f;
        } else if((CurrentType & DamageType.ENEMY) > 0) {
            color[2] = 1.0f;
        } else if((CurrentType & DamageType.COMBO) > 0) {
            color[0] = 0.9f;
            color[1] = 0.9f;
            color[2] = 0.15f;
            Delay = 3000;
        } else {
            color[0] = 1.0f;
            color[1] = 1.0f;
            color[2] = 1.0f;
        }

        textMesh.color = color;
        Ready = true;
        Destroy(gameObject, Delay / 1000);

        // miss
        if(amount == 0) {
            textMesh.text = "MISS";
        } else {
            textMesh.text = $"{amount}";
        }

    }
}
