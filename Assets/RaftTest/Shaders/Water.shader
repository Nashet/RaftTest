// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Water" {
	Properties{
		_MainTex("Albedo (RGB)", 2D) = "black" {}

		_WaterAbsorption("WaterAbsorption", Vector) = (0.259, 0.086, 0.113, 2000.0)

	}
		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		
		LOD 200

		GrabPass{ "_GrabTex" }
			
		CGPROGRAM
#pragma exclude_renderers gles
#pragma surface surf Lambert vertex:vert noforwardadd nolightmap
#pragma target 3.0
#pragma glsl
			
		sampler2D _MainTex, _GrabTex, _CameraDepthTexture;

	float4  _WaterAbsorption;
	struct Input
	{
		float2 uv_MainTex;
		float3 worldPos;
		float4 grabUV;
		float4 projPos;
		float depth;
	};

	void vert(inout appdata_tan v, out Input o)
	{
		UNITY_INITIALIZE_OUTPUT(Input, o)

			//v.tangent = float4(1, 0, 0, 1);

		float4 pos = UnityObjectToClipPos(v.vertex);
		o.grabUV = ComputeGrabScreenPos(pos);
		o.projPos = ComputeScreenPos(pos);
		o.depth = pos.z / pos.w;

	}

	void surf(Input IN, inout SurfaceOutput o)
		{



		float depth = Linear01Depth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.projPos)).r);
		float fragmentsDepth = Linear01Depth(IN.depth);

		float waterDepth = clamp(depth - fragmentsDepth, 0.0, 1.0);

		float3 AbsorptonCof = _WaterAbsorption.rgb  * waterDepth * _WaterAbsorption.a;

		float3 grab = tex2Dproj(_GrabTex, UNITY_PROJ_COORD(IN.grabUV)).rgb;
		float3 col = grab * exp(-AbsorptonCof * AbsorptonCof);
		
		o.Albedo = col + tex2D(_MainTex, IN.uv_MainTex)/4;

		//o.Albedo = lerp(tex2D(_MainTex, IN.uv_MainTex) + _SkyColor, _SunColor, (IN.uv_MainTex.x + IN.uv_MainTex.y + _SinTime.z)*_FlareScale);//

	}
	ENDCG
		}
			FallBack "Diffuse"
}
