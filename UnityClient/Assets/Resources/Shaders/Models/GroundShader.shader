// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Custom/GroundShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Lightmap ("Base (RGB)", 2D) = "white" {}
		_Tintmap ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _Lightmap;
		sampler2D _Tintmap;

		struct Input {
			float2 uv_MainTex;
			float2 uv2_Lightmap;
			float2 uv3_Tintmap;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 lightmap = tex2D(_Lightmap, IN.uv2_Lightmap);
			
			if (c.a == 0.0) {
				discard;
			}

			if (length(IN.uv3_Tintmap)) {
				fixed4 tintmap = tex2D(_Tintmap, IN.uv3_Tintmap);
				c *= fixed4(tintmap.bgr, 1.0);
			}
			
			o.Albedo = c.rgb + lightmap.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}

	Fallback "Mobile/VertexLit"
}
