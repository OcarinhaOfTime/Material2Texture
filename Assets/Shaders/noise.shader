Shader "VShaders/Test/Noise"
{
	Properties
	{		
		_Scale("Scale", Float) = 1
		_Offset("Offset", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque"}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			float _Scale;
			float _Offset;

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

            float rand(float2 st){
                return frac(sin(dot(st, float2(12.9898,78.233))) * 43758.5453123);
            }

            float noise(float2 st){
                float2 i = floor(st);
                float2 f = frac(st);

                float a = rand(i);
                float b = rand(i + float2(1.0, 0.0));
                float c = rand(i + float2(0.0, 1.0));
                float d = rand(i + float2(1.0, 1.0));

                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(a, b, u.x) + (c - a)* u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
            }
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{				
				float n = noise(i.uv * _Scale + _Offset);
				return fixed4(n, n, n, 1);
			}
			ENDCG
		}
	}
}
