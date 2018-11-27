Shader "Unlit/CharacterTrailShader" {
	Properties {
		outlineColor("Outline color",      Color) = (0, 1, 0, 1)
		whiteBeamWidth("White beam width", Float) = 0.6
	}

	SubShader {
		Pass {
			CGPROGRAM

			#pragma vertex   vertexShader
			#pragma fragment fragmentShader

			struct VertexData {
					float4 position: POSITION;
					float2 uv:       TEXCOORD0;
			};

			struct VertexShaderOutput {
					float4 position: SV_POSITION;
					float2 uv:       TEXCOORD0;
			};

			VertexShaderOutput vertexShader(VertexData input) {
					VertexShaderOutput output;

					output.position = UnityObjectToClipPos(input.position);
					output.uv       = input.uv;

					return output;
			}

			fixed4 outlineColor;
			float  whiteBeamWidth;

			fixed4 fragmentShader(VertexShaderOutput input): COLOR0 {
				float low = (1.0 - whiteBeamWidth) / 2.0;

				float high = 1.0 - low;

				if (input.uv.y <= low || input.uv.y >= high) {
					return outlineColor;
				}

				return fixed4(1, 1, 1, 1);
			}

			ENDCG
		}
	}
}
