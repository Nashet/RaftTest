Shader "Custom/Gradient" {
	Properties{
		_MainTex("Albedo (RGB)", 2D) = "black" {}
		_SunColor("SunColor", Color) = (0.26,0.19,0.16,1.0)
		_SkyColor("SkyColor", Color) = (0.06,0.19,0.86,1.0)
		_FlareScale("FlareScale", Range(0, 3)) = 1

	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;
			float4 _SunColor;
			float4 _SkyColor;
			float _FlareScale;

			struct Input {
				float2 uv_MainTex;
				
			};


			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o) {

				
				o.Albedo = lerp(tex2D(_MainTex, IN.uv_MainTex) + _SkyColor, _SunColor, (IN.uv_MainTex.x + IN.uv_MainTex.y + _SinTime.z)*_FlareScale);//
				
			}
			ENDCG
		}
			FallBack "Diffuse"
}
