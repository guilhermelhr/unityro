Shader "Custom/BlackWhiteSpriteShader"
{
	Properties{
			 _MainTex("Base (RGB)", 2D) = "white" {}
	}
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {

			// Get the original texture colour
			half4 tex = tex2D(_MainTex, IN.uv_MainTex);

			// Get the apparent brightness
			half brightness = dot(tex.rgb, half3(0.3, 0.59, 0.11));

			// Set RGB values equal to brightness
			o.Albedo = brightness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
