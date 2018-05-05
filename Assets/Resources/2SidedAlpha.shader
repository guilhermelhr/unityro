// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Custom/2SidedAlpha" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
		
	SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" "PreviewType" = "Plane" }
		LOD 200
		Cull Off

		CGPROGRAM

		#pragma surface surf Lambert alpha

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			half4 c = tex2D(_MainTex, IN.uv_MainTex);

			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}

	Fallback "Mobile/VertexLit"
}
