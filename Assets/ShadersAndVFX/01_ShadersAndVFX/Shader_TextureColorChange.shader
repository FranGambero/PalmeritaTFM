Shader "Unlit/Textura"
{
	Properties{
		_MainTex("Textura", 2D) = "white"{}
	    [PerRendererData] _Color("Tint",color)=(0,0,0,1)
	}
	SubShader{
		Tags{
				"RenderType"="Opaque" 
				"Queue"="Geometry"
			}

			Blend SrcAlpha OneMinusSrcAlpha // Traditional transparency
			ZWrite Off
		Pass{
			

			CGPROGRAM
			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			struct appdata{
				float4 vertex : POSITION;
				float2 uv: TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _Color;
			float4 _MainTex_ST; //para tiling e offset

			struct v2f{
				float4 position : SV_POSITION;
				float2 uv: TEXCOORD0;
			};

			v2f vert(appdata v){
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv,_MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET{
				fixed4 col = tex2D(_MainTex, i.uv);
				col *= _Color;
				return col;
			}

			ENDCG
		}
	}
}