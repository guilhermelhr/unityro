Shader "Custom/2SidedAlphaClipped" {
	Properties{ _MainTex("Base (RGB) Trans (A)", 2D) = "white" {} }

	SubShader{	
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
		LOD 200 
		Blend SrcAlpha OneMinusSrcAlpha 
		Cull Off

		CGPROGRAM
		#pragma surface surf Lambert addshadow

		sampler2D _MainTex;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

			clip(c.a - 0.5);

			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		ENDCG
	}

	Fallback "Transparent/VertexLit"
}