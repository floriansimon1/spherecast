Shader "Custom/NewSurfaceShader" {
	SubShader {
		Tags     { "RenderType" = "Opaque" }
		Lighting Off

		CGPROGRAM

		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surfaceShader NoLighting

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			float3 worldNormal;
			float3 viewDir;
		};

		void surfaceShader(Input input, inout SurfaceOutput output) {
			float threshold = 0.38f;

			// 90 degrees.
			float middle = 3.14f / 2.0f;

			// Works because both vectors are normalized.
			float angle = acos(dot(input.viewDir, input.worldNormal));

			float distance = abs(angle - middle);

			if (distance <= threshold) {
				output.Emission = float3(0.4f, 0.15f, 0.4f);
			} else {
				output.Emission = float3(0.0f, 0.0f, 0.0f);
			}
		}

		fixed4 LightingNoLighting(SurfaceOutput input, fixed3 _, fixed __) {
			return fixed4(0.0f, 0.0f, 0.0f, 0.0f);
		}

		ENDCG
	}

	FallBack "Diffuse"
}
