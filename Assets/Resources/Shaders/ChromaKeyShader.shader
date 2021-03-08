Shader "Unlit/ChromaKeyShader"
{
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Threshold ("Cutout threshold", Range(0,1)) = 0.1
        _Softness ("Cutout softness", Range(0,0.5)) = 0.0
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200
 
        CGPROGRAM
        #pragma surface surf Lambert alpha
 
        sampler2D _MainTex;
        float _Threshold;
        float _Softness;
 
        struct Input {
            float2 uv_MainTex;
        };
 
        void surf (Input IN, inout SurfaceOutput o) {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = smoothstep(_Threshold, _Threshold + _Softness,
               0.333 * (c.r + c.g + c.b));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
