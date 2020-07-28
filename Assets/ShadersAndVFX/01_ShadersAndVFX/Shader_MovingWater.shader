Shader "Custom/MOD2ScrollingVoronoi"{
	Properties{
		_CellSize("Cell Size", Range(0, 2)) = 2
		_BorderColor("Border Color", Color) = (0,0,0,1)
		_TimeScale("Scrolling Speed", Range(0, 2)) = 1
		_CellColor("Cell Color", Color) = (0,0,0,1)

		_Amplitude("Wave Size", Range(0,1)) = 0.4
		_Frequency("Wave Freqency", Range(1, 8)) = 2
		_AnimationSpeed("Animation Speed", Range(0,5)) = 1
	}
		SubShader{
			Tags{ "RenderType" = "Opaque" "Queue" = "Geometry"}

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows vertex:vert addshadow
		//vertex:vert addshadows porque se non calcula as sombras cos vértices sen modificar
			#pragma target 3.0

			#include "WhiteNoise.cginc"

			float _CellSize;
			float _TimeScale;
			float3 _BorderColor;
			float3 _CellColor;

			float _Amplitude;
			float _Frequency;
			float _AnimationSpeed;

			struct Input {
				float3 worldPos;

			};

			float3 voronoiNoise(float3 value) {
				float3 baseCell = floor(value);

				//first pass to find the closest cell
				float minDistToCell = 10;
				float3 toClosestCell;
				float3 closestCell;
				[unroll]
				for (int x1 = -1; x1 <= 1; x1++) {
					[unroll]
					for (int y1 = -1; y1 <= 1; y1++) {
						[unroll]
						for (int z1 = -1; z1 <= 1; z1++) {
							float3 cell = baseCell + float3(x1, y1, z1);
							float3 cellPosition = cell + rand3dTo3d(cell);
							float3 toCell = cellPosition - value;
							float distToCell = length(toCell);
							if (distToCell < minDistToCell) {
								minDistToCell = distToCell;
								closestCell = cell;
								toClosestCell = toCell;
							}
						}
					}
				}

				//second pass to find the distance to the closest edge
				float minEdgeDistance = 10;
				[unroll]
				for (int x2 = -1; x2 <= 1; x2++) {
					[unroll]
					for (int y2 = -1; y2 <= 1; y2++) {
						[unroll]
						for (int z2 = -1; z2 <= 1; z2++) {
							float3 cell = baseCell + float3(x2, y2, z2);
							float3 cellPosition = cell + rand3dTo3d(cell);
							float3 toCell = cellPosition - value;

							float3 diffToClosestCell = abs(closestCell - cell);
							bool isClosestCell = diffToClosestCell.x + diffToClosestCell.y + diffToClosestCell.z < 0.1;
							if (!isClosestCell) {
								float3 toCenter = (toClosestCell + toCell) * 0.5;
								float3 cellDifference = normalize(toCell - toClosestCell);
								float edgeDistance = dot(toCenter, cellDifference);
								minEdgeDistance = min(minEdgeDistance, edgeDistance);
							}
						}
					}
				}

				float random = rand3dTo1d(closestCell);
				return float3(minDistToCell, random, minEdgeDistance);
			}

			//se facemos só un seno cos vértices, non temos en conta as normais, por iso todo este pitifostio
			void vert(inout appdata_full data) {
				float4 modifiedPos = data.vertex;
				modifiedPos.y += sin(data.vertex.x * _Frequency + _Time.y * _AnimationSpeed) * _Amplitude;

				float3 posPlusTangent = data.vertex + data.tangent * 0.01;
				posPlusTangent.y += sin(posPlusTangent.x * _Frequency + _Time.y * _AnimationSpeed) * _Amplitude;

				float3 bitangent = cross(data.normal, data.tangent);
				float3 posPlusBitangent = data.vertex + bitangent * 0.01;
				posPlusBitangent.y += sin(posPlusBitangent.x * _Frequency + _Time.y * _AnimationSpeed) * _Amplitude;

				float3 modifiedTangent = posPlusTangent - modifiedPos;
				float3 modifiedBitangent = posPlusBitangent - modifiedPos;

				float3 modifiedNormal = cross(modifiedTangent, modifiedBitangent);
				data.normal = normalize(modifiedNormal);
				data.vertex = modifiedPos;
			}

			void surf(Input i, inout SurfaceOutputStandard o) {
				float3 value = i.worldPos.xyz / _CellSize;
				value.y += _Time.y * _TimeScale;
				float3 noise = voronoiNoise(value);

				float3 cellColor = _CellColor;
				float valueChange = fwidth(value.z) * 0.5;
				float isBorder = 1 - smoothstep(0.05 - valueChange, 0.05 + valueChange, noise.z);
				float3 color = lerp(cellColor, _BorderColor, isBorder);


				o.Albedo = color;
				
			}
			ENDCG
	}
		FallBack "Standard"
}