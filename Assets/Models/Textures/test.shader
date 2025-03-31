Shader "Custom/VoronoiWater" {
    Properties {
        _CellSize ("Cell Size", Range(0, 10)) = 2
        _WindDirection ("Wind Direction", Vector) = (1, 0, 0)
        _WindFactor ("Wind Factor", Range(0, 10)) = 1
        _BorderColor ("Border Color", Color) = (0,0,0,1)
        _SurfaceNoise("Surface Noise", 2D) = "white" {}
        _SurfaceNoiseFactor ("Surface Noise Factor", Range(0, 10)) = 1
        _VoronoiDistortFactor ("Voronoi Distort Factor", Range(0, 10)) = 1
        _ColorMap("Color Map", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

            float _CellSize, _SurfaceNoiseFactor, _VoronoiDistortFactor;
            float3 _BorderColor;
            float2 _WindDirection;
            float _WindFactor;

            sampler2D _ColorMap;
            sampler2D _SurfaceNoise;
            float4 _SurfaceNoise_ST;

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 noiseUV : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            float rand2dTo1d(float2 value, float2 dotDir = float2(12.9898, 78.233)) {
                float random = frac(sin(dot(value, dotDir)) * 143758.5453);
                return random;
            }

            float2 rand2dTo2d(float2 value) {
                return float2(
                    rand2dTo1d(value, float2(12.989, 78.233)),
                    rand2dTo1d(value, float2(39.346, 11.135))
                );
            }

            float voronoiNoise(float2 value) {
                float2 baseCell = floor(value);
                float minDistToCell = 10.0;
                [unroll]
                for (int x = -1; x <= 1; x++) {
                    [unroll]
                    for (int y = -1; y <= 1; y++) {
                        float2 cell = baseCell + float2(x, y);
                        float2 cellPosition = cell + rand2dTo2d(cell);
                        float2 toCell = cellPosition - value;
                        float distToCell = length(toCell);
                        minDistToCell = min(minDistToCell, distToCell);
                    }
                }
                return minDistToCell;
            }

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                float2 noiseUV = i.noiseUV + _Time.y * _WindDirection * _WindFactor;
                float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;
                float2 voronoiPos = i.worldPos.xz + _Time.y * _WindDirection * _WindFactor;
                voronoiPos.x += surfaceNoiseSample * _VoronoiDistortFactor;
                float noise = voronoiNoise(voronoiPos / _CellSize);
                float color = tex2D(_ColorMap, float2(noise, 0)).r;
                return float4(color, color, color, 1);
            }
            ENDCG
        }
    }
}