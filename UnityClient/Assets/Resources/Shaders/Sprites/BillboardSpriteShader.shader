Shader "UnityRO/BillboardSpriteShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _PaletteTex("Texture", 2D) = "white" {}
        _Alpha("Alpha", Range(0.0, 1.0)) = 1.0
    }

        SubShader
        {
            Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "DisableBatching" = "True" }

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    fixed4 color : TEXCOOD1;
                    UNITY_FOG_COORDS(1)
                };

                sampler2D _MainTex;
                sampler2D _PaletteTex;

                float4 _MainTex_TexelSize;
                float _Alpha;

                float rayPlaneIntersection(float3 rayDir, float3 rayPos, float3 planeNormal, float3 planePos)
                {
                    float denom = dot(planeNormal, rayDir);
                    denom = max(denom, 0.000001);
                    float3 diff = planePos - rayPos;
                    return dot(diff, planeNormal) / denom;
                }

                fixed4 bilinearSample(sampler2D indexT, sampler2D LUT, float2 uv, float4 indexT_TexelSize) {
                    float2 TextInterval = 1.0 / indexT_TexelSize.zw;

                    float tlLUT = tex2D(indexT, uv).x;
                    float trLUT = tex2D(indexT, uv + float2(TextInterval.x, 0.0)).x;
                    float blLUT = tex2D(indexT, uv + float2(0.0, TextInterval.y)).x;
                    float brLUT = tex2D(indexT, uv + TextInterval).x;

                    float4 transparent = float4(0.5, 0.5, 0.5, 0.0);

                    float4 tl = tlLUT == 0.0 ? transparent : float4(tex2D(LUT, float2(tlLUT, 1.0)).rgb, 1.0);
                    float4 tr = trLUT == 0.0 ? transparent : float4(tex2D(LUT, float2(trLUT, 1.0)).rgb, 1.0);
                    float4 bl = blLUT == 0.0 ? transparent : float4(tex2D(LUT, float2(blLUT, 1.0)).rgb, 1.0);
                    float4 br = brLUT == 0.0 ? transparent : float4(tex2D(LUT, float2(brLUT, 1.0)).rgb, 1.0);

                    float2 f = frac(uv.xy * indexT_TexelSize.zw);
                    float4 tA = lerp(tl, tr, f.x);
                    float4 tB = lerp(bl, br, f.x);

                    return lerp(tA, tB, f.y);
                }

                v2f vert(appdata v)
                {
                    v2f o;
                    o.uv = v.uv.xy;

                    // billboard mesh towards camera
                    float3 vpos = mul((float3x3)unity_ObjectToWorld, v.vertex.xyz);
                    float4 worldCoord = float4(unity_ObjectToWorld._m03_m13_m23, 1);
                    float4 viewPivot = mul(UNITY_MATRIX_V, worldCoord);

                    // Temporary ignoring shaders billboard rotation, handled by cs script until we join all quads sprites in one
                    float4 viewPos = float4(viewPivot + mul(vpos, (float3x3)unity_ObjectToWorld), 1.0);
                    o.pos = UnityObjectToClipPos(v.vertex);

                    // calculate distance to vertical billboard plane seen at this vertex's screen position
                    const float3 planeNormal = normalize((_WorldSpaceCameraPos.xyz - unity_ObjectToWorld._m03_m13_m23) * float3(1,0,1));
                    const float3 planePoint = unity_ObjectToWorld._m03_m13_m23;
                    const float3 rayStart = _WorldSpaceCameraPos.xyz;
                    const float3 rayDir = -normalize(mul(UNITY_MATRIX_I_V, float4(viewPos.xyz, 1.0)).xyz - rayStart); // convert view to world, minus camera pos
                    const float dist = rayPlaneIntersection(rayDir, rayStart, planeNormal, planePoint);

                    // calculate the clip space z for vertical plane
                    float4 planeOutPos = mul(UNITY_MATRIX_VP, float4(rayStart + rayDir * dist, 1.0));
                    float newPosZ = planeOutPos.z / planeOutPos.w * o.pos.w;

                    // use the closest clip space z
                    #if defined(UNITY_REVERSED_Z)
                        o.pos.z = max(o.pos.z, newPosZ);
                    #else
                        o.pos.z = min(o.pos.z, newPosZ);
                    #endif

                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 tex = bilinearSample(_MainTex, _PaletteTex, i.uv, _MainTex_TexelSize);

                    if (tex.a == 0.0) {
                        discard;
                    }

                    tex.a *= _Alpha;

                    UNITY_APPLY_FOG(i.fogCoord, tex);

                    return tex;
                }
                ENDCG
            }
        }
}
