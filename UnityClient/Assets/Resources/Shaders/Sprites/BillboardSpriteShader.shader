Shader "UnityRO/BillboardSpriteShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
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
            float _Alpha;
            float4 _MainTex_ST;

            float rayPlaneIntersection( float3 rayDir, float3 rayPos, float3 planeNormal, float3 planePos)
            {
                float denom = dot(planeNormal, rayDir);
                denom = max(denom, 0.000001);
                float3 diff = planePos - rayPos;
                return dot(diff, planeNormal) / denom;
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
                fixed4 col = tex2D(_MainTex, i.uv);

                col.a *= _Alpha;

                UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }
            ENDCG
        }
    }
}
