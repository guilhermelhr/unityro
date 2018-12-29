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

		float4 _MainTex_TexelSize;

		struct Input {
			float2 uv_MainTex;
			float2 uv2_Lightmap;
			float2 uv3_Tintmap;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			//IN.uv_MainTex.x += 0.5 / _MainTex_TexelSize.z;
			//IN.uv_MainTex.y += 0.5 / _MainTex_TexelSize.w;

			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 lightmap = tex2D(_Lightmap, IN.uv2_Lightmap);
			
			//clip(c.a - 0.5f);
			if (c.a == 0.0) {
				discard;
			}

			if (length(IN.uv3_Tintmap)) {
				fixed4 tintmap = tex2D(_Tintmap, IN.uv3_Tintmap);
				//tintmap *= 1.2;
				c *= fixed4(tintmap.bgr, 1.0);
			}
			
			//lightmap *= 1.2;

			o.Albedo = c.rgb + lightmap.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}

	Fallback "Mobile/VertexLit"
}
