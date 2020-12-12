Shader "Custom/WaterShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_WaterOffset("Water Offset", Float) = 0
		_WaveAmplitude("Wave Amplitude", Float) = 0.19
		_WaveSpeed("Wave Speed", Float) = 0.016
		_WaveLenght("Wave Lenght", Float) = 0.5
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Back
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord  : TEXCOORD0;
			};

			fixed4 _Color;
			float _WaterOffset;
			float _WaveAmplitude;
			float _WaveSpeed;
			float _WaveLenght;

			v2f vert(appdata_t IN)
			{
				IN.vertex.y += _WaveAmplitude * sin(IN.vertex.x * _WaveLenght + _WaterOffset * _WaveSpeed);

				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);

				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
		#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap(OUT.vertex);
		#endif	

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;

			fixed4 SampleSpriteTexture(float2 uv)
			{
				fixed4 color = tex2D(_MainTex, uv);

			#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D(_AlphaTex, uv).r;
		#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
				c.rgb *= c.a;
				return c;
			}
			ENDCG
		}
	}
}
