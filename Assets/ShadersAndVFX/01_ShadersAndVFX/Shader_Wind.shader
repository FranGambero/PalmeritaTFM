Shader "Custom/Shader_Wind"
{
    Properties
    {
		_Color ("Base Color", Color)=(1,1,1,1)

        _WindTexture ("Wind Texture", 2D)="white"{}
		_WindColor("Wind Color", Color) = (1,1,1,1)
		_WindDirection("Wind Direction", Vector) = (0,1,0,0)
		_Speed("Speed",Range(0.1,2)) = 1
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "ForceNoShadowCasting" = "True" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows alpha
			#pragma target 4.0

		fixed4 _Color;

		sampler2D _WindTexture;
		fixed4 _WindColor;
		float2 _WindDirection;
		float _Speed;
        struct Input
        {
            float2 uv_WindTexture;
        };


        void surf (Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 col = _Color;
			float Slow = 1 / _Speed;
			float2 WindCoordinates = IN.uv_WindTexture + _WindDirection * _Time.y/Slow;
			fixed4 WindLayer1 = tex2D(_WindTexture, WindCoordinates)*_WindColor;
			col.rgb = lerp(col.rgb, WindLayer1.rgb, WindLayer1.a);
			col.a = lerp(col.a, 1, WindLayer1.a);


			o.Albedo = col.rgb;
			o.Alpha = col.a;

        }
        ENDCG
    }
    FallBack "Diffuse"
}
