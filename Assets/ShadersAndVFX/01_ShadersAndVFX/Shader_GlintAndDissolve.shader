Shader "Custom/Shader_GlintAndDissolve"
{
	Properties{
	   _Color("Tint", Color) = (0, 0, 0, 1)
	   _MainTex("Texture", 2D) = "white" {}
	   _Smoothness("Smoothness", Range(0, 1)) = 0
	   _Metallic("Metalness", Range(0, 1)) = 0
	   //[HDR] _Emission("Emission", color) = (0,0,0)

	   [Header(Dissolve)]
	   _DissolveTex("Dissolve Texture", 2D) = "black" {}
	   [PerRenderData] _DissolveAmount("Dissolve Amount", float) = 0.5

	   [Header(Glow)]
	   [HDR]_GlowColor("Color", Color) = (1, 1, 1, 1)
	   _GlowRange("Range", Range(0, .3)) = 0.1
	   _GlowFalloff("Falloff", Range(0.001, .3)) = 0.1


		[Header(Glint Layer)]
		_Specs1("GlintTexture", 2D) = "white" {}
		[PerRenderData][HDR]_SpecColor1("Glint Color", Color) = (1,1,1,1)
		_SpecDirection1("Glint Direction", Vector) = (0, 1, 0, 0)
		_Speed("Speed",Range(0.1,2)) = 1

	}
		SubShader{
			Tags{ "RenderType" = "Opaque" "Queue" = "Geometry"}

			CGPROGRAM

			#pragma surface surf Standard fullforwardshadows
			#pragma target 3.0

			sampler2D _MainTex;
			fixed4 _Color;

			half _Smoothness;
			half _Metallic;
			//half3 _Emission;

			sampler2D _DissolveTex;
			float _DissolveAmount;

			float3 _GlowColor;
			float _GlowRange;
			float _GlowFalloff;

			//parte glint:
			sampler2D _Specs1;
			fixed4 _SpecColor1;
			float2 _SpecDirection1;
			float _Speed;
			//


			struct Input {
				float2 uv_MainTex;
				float2 uv_DissolveTex;
				//glint:
				float2 uv_Specs1;
			};

			void surf(Input i, inout SurfaceOutputStandard o) {
				float dissolve = tex2D(_DissolveTex, i.uv_DissolveTex).r;
				dissolve = dissolve * 0.999;
				float isVisible = dissolve - _DissolveAmount;
				clip(isVisible);

				float isGlowing = smoothstep(_GlowRange + _GlowFalloff, _GlowRange, isVisible);
				float3 glow = isGlowing * _GlowColor;

				fixed4 col = tex2D(_MainTex, i.uv_MainTex);

				//glint:
				float Slow = 1 / _Speed;
				float2 specCoordinates1 = i.uv_Specs1 + _SpecDirection1 * _Time.y / Slow;
				fixed4 specLayer1 = tex2D(_Specs1, specCoordinates1) * _SpecColor1;
				col.rgb = lerp(col.rgb, specLayer1.rgb, specLayer1.a);
				col.a = lerp(col.a, 1, specLayer1.a);
				//
				col *= _Color;

				o.Albedo = col.rgb;
				o.Alpha = col.a;
				o.Metallic = _Metallic;
				o.Smoothness = _Smoothness;
				//o.Emission = _Emission + glow;
				o.Emission = glow;
			}
			ENDCG
	   }
		   FallBack "Standard"
}