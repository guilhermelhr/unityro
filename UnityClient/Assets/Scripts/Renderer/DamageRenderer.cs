using UnityEngine;
using System;
using TMPro;
using System.Text;

public class DamageRenderer : MonoBehaviour {

    [SerializeField]
    private TextMeshPro textMesh;
    [SerializeField]
    public float _startingAngle = 50;
    [SerializeField]
    public float angleMultiplier = 3f;
    [SerializeField]
    public float yMultiplier = 0.8f;
    [SerializeField]
    public float xMultiplier = 0.1f;

    private float Delay;
    private bool Ready;
    private double start;

    public DamageType CurrentType { get; private set; }

    float x = 0;
    float y = 0;
    float z = 0;
    float scale = 0f;

    float angle => MathHelper.ToRadians(_startingAngle);

    void Update() {
        Ready = start < GameManager.Tick;

        if (!Ready)
            return;

        textMesh.enabled = Ready;

        float perc = (float) ((GameManager.Tick - start) / Delay);

        if ((CurrentType & DamageType.COMBO) > 0) {
            scale = Mathf.Min(scale + Time.deltaTime * 8f, 1f);
            var newScale = new Vector2(scale, scale);
            transform.GetChildren().ForEach(it => it.transform.localScale = newScale);
        } else if ((CurrentType & DamageType.DAMAGE) > 0) {
            _startingAngle += Time.deltaTime * angleMultiplier;
            y += ((float) Math.Cos(angle)) * yMultiplier;
            x += ((float) Math.Sin(angle)) * xMultiplier;

            transform.position += new Vector3(x, y, -1) * Time.deltaTime;
        } else {
            transform.position += Vector3.up * Time.deltaTime * 7;
        }

        var color = textMesh.color;
        color[3] -= 0.3f * Time.deltaTime;
        textMesh.color = color;
    }

    public float Display(float amount, double tick, DamageType? type, Entity entity) {
        var stringBuilder = new StringBuilder(128);
        transform.position = entity.transform.position;

        CurrentType = (type ?? (amount > 0 ? DamageType.DAMAGE : DamageType.MISS));
        if (entity.Type == EntityType.PC) {
            CurrentType |= DamageType.ENEMY;
        }

        var color = new Color();
        color[3] = 0f;
        Delay = 1500;
        start = tick;

        transform.position += Vector3.up * 2;

        if ((CurrentType & DamageType.SP) > 0) {
            color = Color.blue;
        } else if ((CurrentType & DamageType.HEAL) > 0) {
            color = Color.green;
        } else if ((CurrentType & DamageType.ENEMY) > 0) {
            color = Color.red;
        } else if ((CurrentType & DamageType.COMBO) > 0) {
            transform.position += Vector3.up * 5;
            transform.GetChildren().ForEach(it => it.transform.localScale = Vector3.zero);
            color = Color.yellow;
            Delay = 3000;
        } else {
            color = Color.white;
        }

        textMesh.color = color;
        Ready = false;

        stringBuilder.Append("<cspace=0.4>");

        if (amount == 0) {
            stringBuilder.Append("<indent=8%>");
            stringBuilder.Append("<size=60%>");
            stringBuilder.Append("<sprite=10 tint>"); //"miss" sprite
            stringBuilder.Append("</indent>");
            stringBuilder.Append("</size>");
        } else {
            stringBuilder.Append("<indent=4%>");
            foreach (var c in amount.ToString()) {
                stringBuilder.Append($"<sprite={c} tint>");
            }
            stringBuilder.Append("</indent>");
        }

        textMesh.text = stringBuilder.ToString();
        stringBuilder.Clear();
        textMesh.enabled = false;

        return Delay / 1000f;
    }
}
