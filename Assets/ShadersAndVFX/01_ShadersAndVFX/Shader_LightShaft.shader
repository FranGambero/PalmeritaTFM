Shader "Custom/Shader_LightShaft"
{
    Properties
    {
		[HDR] 
	_Color("Base Color", Color) = (1,1,1,1)

		[Header(Spec Layer 1)]
		_Specs1("Specs", 2D) = "white" {}
		_SpecColor1("Spec Color", Color) = (1,1,1,1)
		_SpecDirection1("Spec Direction", Vector) = (0, 1, 0, 0)

		[Header(Spec Layer 2)]
		_Specs2("Specs", 2D) = "white" {}
		_SpecColor2("Spec Color", Color) = (1,1,1,1)
		_SpecDirection2("Spec Direction", Vector) = (0, 1, 0, 0)
    }
    SubShader
    {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "ForceNoShadowCasting" = "True"}
        LOD 200

        CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha
		#pragma target 4.0

		fixed4 _Color;

		sampler2D _Specs1;
		fixed4 _SpecColor1;
		float2 _SpecDirection1;

		sampler2D _Specs2;
		fixed4 _SpecColor2;
		float2 _SpecDirection2;

        struct Input
        {
			float2 uv_Specs1;
			float2 uv_Specs2;
        };


        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			//set shaft base color
			fixed4 col = _Color;

			//add first layer of moving specs
			float2 specCoordinates1 = IN.uv_Specs1 + _SpecDirection1 * _Time.y;
			fixed4 specLayer1 = tex2D(_Specs1, specCoordinates1) * _SpecColor1;
			col.rgb = lerp(col.rgb, specLayer1.rgb, specLayer1.a);
			col.a = lerp(col.a, 1, specLayer1.a);

			//add second layer of moving specs
			float2 specCoordinates2 = IN.uv_Specs2 + _SpecDirection2 * _Time.y;
			fixed4 specLayer2 = tex2D(_Specs2, specCoordinates2) * _SpecColor2;
			col.rgb = lerp(col.rgb, specLayer2.rgb, specLayer2.a);
			col.a = lerp(col.a, 1, specLayer2.a);
 
			//apply values to output struct
			o.Albedo = col.rgb;
			o.Alpha = col.a;
		}
        ENDCG
    }
    FallBack "Diffuse"
}
