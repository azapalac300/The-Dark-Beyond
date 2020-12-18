// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Terrestrial Planet"
{
	Properties{
		_TintA("Tint A", Color) = (1, 1, 1, 1)
		_TintB("Tint B", Color) = (1, 1, 1, 1)
		_TintC("Tint C", Color) = (1, 1, 1, 1)
		_TintD("Tint D", Color) = (1, 1, 1, 1)
		_TintE("Tint E", Color) = (1, 1, 1 ,1)
		_Val("Dist Val", Float) = 0
		_Thresh("Threshold", Float) = 0
	}


		SubShader{
			Pass{
	CGPROGRAM
	#pragma vertex MyVertexProgram
	#pragma fragment MyFragmentProgram
	#include "UnityCG.cginc"
	float4 _TintA;
	float4 _TintB;
	float4 _TintC;
	float4 _TintD;
	float4 _TintE;

	float _Val;
	float _Thresh;


	struct v2f {
		float4 vertPosition: SV_POSITION;
		float4 vertWorldPosition: TEXCOORD0;
	};

v2f MyVertexProgram(float4 position: POSITION){
	v2f o;
	o.vertPosition = UnityObjectToClipPos(position);
	o.vertWorldPosition = mul(unity_ObjectToWorld, position);
		return o;
}

float4 MyFragmentProgram(v2f o): SV_TARGET {
	float4 color = _TintA;
	float4 origin = mul(unity_ObjectToWorld, float4(0, 0, 0, 1));



	

	if (length(o.vertWorldPosition -
		origin) > (_Val / 2)) {
		color = _TintB;
	}

	if (length(o.vertWorldPosition -
		origin) > (_Val / 2 + _Thresh)) {
		color = _TintC;
	}

	if (length(o.vertWorldPosition -
		origin) > (_Val / 2 + 2*_Thresh)) {
		color = _TintD;
	}


	if (length(o.vertWorldPosition -
		origin) > (_Val / 2 + 3 * _Thresh)) {
		color = _TintE;
	}

	return color;


}
ENDCG
}
}

}