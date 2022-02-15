using UnityEngine;
using System;
using TMPro;
using System.Text;

public class DamageRenderer : MonoBehaviour {

    [SerializeField]
    private TextMeshPro textMesh;
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
        if (!Ready)
            return;

        //float perc = (float) ((GameManager.Tick - start) / Delay);

        //if ((CurrentType & DamageType.COMBO) > 0) {
        //    scale = (float) (Math.Min(perc, 0.05) * 0.75);
        //    z += 5 + perc;
        //} else if ((CurrentType & DamageType.DAMAGE) > 0) {
        //    scale = (float) ((1 - perc) * 4);

        //    angle += Time.deltaTime * angleMultiplier;
        //    y += ((float) Math.Cos(angle)) * yMultiplier;
        //    x += ((float) Math.Sin(angle)) * xMultiplier;

        //    transform.position += new Vector3(x, y, 0) * Time.deltaTime;
        //} else if ((CurrentType & DamageType.HEAL) > 0) {

        //} else if ((CurrentType & DamageType.MISS) > 0) {
        //    perc = (float) ((GameManager.Tick - start) / 800);
        //    scale = 0.5f;
        //    transform.position += Vector3.up * Time.deltaTime * 7;
        //}

        transform.position += Vector3.up * Time.deltaTime * 7;

        //Debug.Log(transform.position);
        //transform.localScale *= scale;

        var color = textMesh.color;
        color[3] -= 0.3f * Time.deltaTime;
        textMesh.color = color;
    }

    public void Display(float amount, double tick, DamageType? type, Entity entity) {
        var stringBuilder = new StringBuilder(128);
        transform.position = entity.transform.position;

        CurrentType = (type ?? (amount > 0 ? DamageType.DAMAGE : DamageType.MISS));
        if (entity.Type == EntityType.PC) {
            CurrentType |= DamageType.ENEMY;
        }

        var color = new Color();
        color[3] = 1.0f;
        Delay = 1500;
        start = tick;

        if ((CurrentType & DamageType.SP) > 0) {
            color = Color.blue;
        } else if ((CurrentType & DamageType.HEAL) > 0) {
            color = Color.green;
        } else if ((CurrentType & DamageType.ENEMY) > 0) {
            color = Color.red;
        } else if ((CurrentType & DamageType.COMBO) > 0) {
            color = Color.yellow;
            Delay = 3000;
        } else {
            color = Color.white;
        }

        textMesh.color = color;
        Ready = true;
        Destroy(gameObject, Delay / 1000);

        stringBuilder.Append("<cspace=0.4>");

        if (amount == 0) {
            stringBuilder.Append("<indent=8%>");
            stringBuilder.Append("<size=60%>");
            stringBuilder.Append("<sprite=10 tint>"); //"miss" sprite
            stringBuilder.Append("</indent>");
            stringBuilder.Append("</size>");
        } else {
            foreach (var c in amount.ToString()) {
                stringBuilder.Append($"<sprite={c} tint>");
            }
        }

        textMesh.text = stringBuilder.ToString();
        stringBuilder.Clear();
    }
}
