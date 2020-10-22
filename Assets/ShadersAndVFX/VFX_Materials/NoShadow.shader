Shader "Custom/NoShadow"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)

        _MainTex ("Texture", 2D) = "white" {}
        _Amplitud("Amplitud", float)=1
        _Speed("Speed", float)=1
        _Seno("Seno", float)=1
    }
    SubShader
    {
        Tags{"RenderType"="Transparent"}
        // No culling or depth
        //Cull Off 
        //ZWrite Off 
        //ZTest Always

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Amplitud;
            float _Speed;
            float _Seno;
        fixed4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                
            
            //  i.uv.y += _Speed;
                fixed4 col = tex2D(_MainTex, i.uv)* _Color;
                return col;
            }
            ENDCG
        }
    }
}
