Shader "Custom/WaterWithWaves" {
	Properties{
		_MainTex("Albedo (RGB)", 2D) = "black" {}
		_WaterAbsorption("WaterAbsorption", Vector) = (0.259, 0.086, 0.113, 2000.0)
		// width of the edge effect
		_FoamFactor("Foam Factor", float) = 1.0
		// color of the edge effect
		_FoamColor("Foam Color", Color) = (1, 1, 1, 1)
			
		_WaveFrequency("WaveFrequency", float) = 5
		_WaveAmplitude("WaveAmplitude", float)=10

		_NoiseTex("Noise", 2D) = "black"{}

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

	sampler2D _MainTex, _GrabTex, _CameraDepthTexture, _NoiseTex;

	float4  _WaterAbsorption, _FoamColor;

	float _FoamFactor, _WaveFrequency,_WaveAmplitude;


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
		UNITY_INITIALIZE_OUTPUT(Input, o);


		float4 pos = UnityObjectToClipPos(v.vertex);
		o.grabUV = ComputeGrabScreenPos(pos);
		o.projPos = ComputeScreenPos(pos);
		o.depth = pos.z;// / pos.w;
		// apply wave animation
		float noiseSample = tex2Dlod(_NoiseTex, float4(v.texcoord.xy, 0, 0));
		v.vertex.y += sin((_Time.z + v.vertex.x + v.vertex.z) *_WaveFrequency*noiseSample)/_WaveAmplitude;

	}

	void surf(Input IN, inout SurfaceOutput o)
	{
		float cameraDepth = Linear01Depth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.projPos)).r);
		float fragmentsDepth = Linear01Depth(IN.depth);

		float waterDepth = clamp(cameraDepth - fragmentsDepth, 0.0, 1.0);

		float3 AbsorptonCof = _WaterAbsorption.rgb  * waterDepth * _WaterAbsorption.a;

		float3 grab = tex2Dproj(_GrabTex, UNITY_PROJ_COORD(IN.grabUV)).rgb;
		float3 col = grab * exp(-AbsorptonCof * AbsorptonCof);

		// apply the DepthFactor to be able to tune at what depth values
		// the foam line actually starts
		float foamLine = 1 - saturate(_FoamFactor * (waterDepth));


		o.Albedo = col + tex2D(_MainTex, IN.uv_MainTex) / 4;// +foamLine * _FoamColor;

		//o.Albedo = IN.depth*8;// float4(0, 0, 1, 0);

	}
	ENDCG
		}
			FallBack "Diffuse"
}
