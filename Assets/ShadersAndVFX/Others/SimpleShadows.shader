Shader "Custom/SimpleShadows"
{
    
  Properties {
     _Color ("Main Color", Color) = (1,1,1,1)
     _MainTex ("Base (RGB)", 2D) = "white" {}
     _Cutoff("Shadow alpha cutoff", Range(0,1)) = 0.5
	
 }
 SubShader {
     Tags {"Queue"="Geometry" "RenderType"="Transparent" }
     LOD 200
     Cull Off
     Blend SrcAlpha OneMinusSrcAlpha
 CGPROGRAM
 #pragma surface surf Lambert
 
 sampler2D _MainTex;
 fixed4 _Color;
 fixed _Cutoff;

 struct Input {
     float2 uv_MainTex;
     float4 color : COLOR;
 };
 
 void surf (Input IN, inout SurfaceOutput o) {
     fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
     o.Albedo = c.rgb;
     o.Albedo *= IN.color.rgb;
     o.Alpha = c.a;
     clip(o.Alpha - _Cutoff);
 }
 ENDCG
 }
 Fallback "VertexLit"
 }