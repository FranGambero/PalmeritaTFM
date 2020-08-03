Shader "Custom/Shader_Glint"
{
    Properties
    {

        _MainTex ("Textura", 2D) = "white" {}


		[Header(Glint Layer)]
		_Specs1("GlintTexture", 2D) = "white" {}
		_SpecColor1("Glint Color", Color) = (1,1,1,1)
		_SpecDirection1("Glint Direction", Vector) = (0, 1, 0, 0)
		_Speed("Speed",Range(0.1,2)) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

		sampler2D _Specs1;
		fixed4 _SpecColor1;
		float2 _SpecDirection1;
		float _Speed;


        struct Input
        {
            float2 uv_MainTex;
			float2 uv_Specs1;
        };



        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture
            fixed4 col = tex2D (_MainTex, IN.uv_MainTex);
            
			float Slow = 1 / _Speed;
			float2 specCoordinates1 = IN.uv_Specs1 + _SpecDirection1 * _Time.y / Slow;
			fixed4 specLayer1 = tex2D(_Specs1, specCoordinates1) * _SpecColor1;
			col.rgb = lerp(col.rgb, specLayer1.rgb, specLayer1.a);
			col.a = lerp(col.a, 1, specLayer1.a);


			//apply values to output struct
			o.Albedo = col.rgb;
			o.Alpha = col.a;

        }
        ENDCG
    }
    FallBack "Diffuse"
}
