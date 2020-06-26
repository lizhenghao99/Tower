// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Loot Beams/Beam/AlphaBlended(+Mask)"
{
	Properties
	{
		_Glow("Glow", Float) = 1
		_TintColor("Color", Color) = (1,1,1,1)
		_GlobalColor("Global Color", Range( 0 , 1)) = 1
		[NoScaleOffset]_MainTex("Main Texture", 2D) = "white" {}
		_MaskValue("Mask Value", Float) = 1
		_MaskScale("Mask Scale", Float) = 1
		_MaskScroll("Mask Scroll", Float) = 1
		[NoScaleOffset]_MaskTex("Mask Texture", 2D) = "white" {}
		_BotFadeRange("Bot Fade Range", Range( 0 , 1)) = 0
		_BotFadeSmooth("Bot Fade Smooth", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha , SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float _Glow;
		uniform float4 _TintColor;
		uniform sampler2D _MainTex;
		uniform sampler2D _MaskTex;
		uniform float _MaskScale;
		uniform float _MaskScroll;
		uniform float _MaskValue;
		uniform float _BotFadeRange;
		uniform float _BotFadeSmooth;
		uniform float _GlobalColor;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_MainTex3 = i.uv_texcoord;
			float4 tex2DNode3 = tex2D( _MainTex, uv_MainTex3 );
			o.Emission = ( _Glow * ( _TintColor * _TintColor.a * tex2DNode3 * i.vertexColor ) ).rgb;
			float2 appendResult15 = (float2(_MaskScroll , 0.0));
			float clampResult33 = clamp( ( ( ( 1.0 - i.uv_texcoord.x ) - ( ( _BotFadeRange * ( _BotFadeSmooth + 1.0 ) ) - _BotFadeSmooth ) ) * ( 1.0 / _BotFadeSmooth ) ) , 0.0 , 1.0 );
			o.Alpha = ( ( ( tex2DNode3.a * i.vertexColor.a * pow( tex2D( _MaskTex, ( (i.uv_texcoord*_MaskScale + 0.0) + ( appendResult15 * _Time.y ) ) ).r , _MaskValue ) * _TintColor.a ) * clampResult33 ) * _GlobalColor );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17000
7;2;1906;1010;2384.119;466.9017;2.012107;True;False
Node;AmplifyShaderEditor.RangedFloatNode;21;-3464.335,1700.677;Float;False;Property;_BotFadeSmooth;Bot Fade Smooth;10;0;Create;True;0;0;False;0;0;0.06;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-3598.434,634.566;Float;False;Property;_MaskScroll;Mask Scroll;7;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-3723.693,1809.262;Float;False;Constant;_Float;Float;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-3210.441,1369.607;Float;False;Property;_BotFadeRange;Bot Fade Range;9;0;Create;True;0;0;False;0;0;0.06;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-3149.615,1471.915;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;14;-3354.231,892.2461;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;15;-3387.117,639.6378;Float;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-3089.028,656.0092;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;37;-3024.484,568.1174;Float;False;Property;_MaskScale;Mask Scale;6;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;36;-2741.539,656.4638;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-2763.7,1448.826;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;25;-2586.507,1672.6;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-2809.957,869.5685;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;29;-3156.252,1808.715;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;27;-2452.625,1169.447;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-2493.797,846.8902;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;28;-2383.079,1448.373;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;30;-1869.512,1425.063;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;31;-1788.276,1744.063;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1809.991,1105.463;Float;False;Property;_MaskValue;Mask Value;5;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;9;-1938.982,821.4048;Float;True;Property;_MaskTex;Mask Texture;8;1;[NoScaleOffset];Create;False;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-1551.636,406.8308;Float;True;Property;_MainTex;Main Texture;4;1;[NoScaleOffset];Create;False;0;0;False;0;None;c0034222ac7408041bfb7d5aa68ac09c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2;-1467.167,220.3641;Float;False;Property;_TintColor;Color;2;0;Create;False;0;0;False;0;1,1,1,1;0.008677464,0.1875966,0.3679245,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;8;-1494.847,848.938;Float;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-1481.807,1425.63;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;4;-1432.448,611.4215;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;33;-1032.448,1426.073;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-1085.356,503.8174;Float;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1;-1087.857,225.1454;Float;True;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1029.59,134.639;Float;False;Property;_Glow;Glow;1;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-687.5849,503.7487;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-637.3107,750.1364;Float;False;Property;_GlobalColor;Global Color;3;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-702.6496,206.3008;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-195.5971,502.7548;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;25.54739,141.3874;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Loot Beams/Beam/AlphaBlended(+Mask);False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;22;0;21;0
WireConnection;22;1;20;0
WireConnection;15;0;16;0
WireConnection;36;0;13;0
WireConnection;36;1;37;0
WireConnection;24;0;23;0
WireConnection;24;1;22;0
WireConnection;25;0;21;0
WireConnection;12;0;15;0
WireConnection;12;1;14;0
WireConnection;29;0;20;0
WireConnection;29;1;21;0
WireConnection;27;0;13;1
WireConnection;11;0;36;0
WireConnection;11;1;12;0
WireConnection;28;0;24;0
WireConnection;28;1;25;0
WireConnection;30;0;27;0
WireConnection;30;1;28;0
WireConnection;31;0;29;0
WireConnection;9;1;11;0
WireConnection;8;0;9;1
WireConnection;8;1;18;0
WireConnection;32;0;30;0
WireConnection;32;1;31;0
WireConnection;33;0;32;0
WireConnection;5;0;3;4
WireConnection;5;1;4;4
WireConnection;5;2;8;0
WireConnection;5;3;2;4
WireConnection;1;0;2;0
WireConnection;1;1;2;4
WireConnection;1;2;3;0
WireConnection;1;3;4;0
WireConnection;19;0;5;0
WireConnection;19;1;33;0
WireConnection;7;0;6;0
WireConnection;7;1;1;0
WireConnection;38;0;19;0
WireConnection;38;1;35;0
WireConnection;0;2;7;0
WireConnection;0;9;38;0
ASEEND*/
//CHKSM=394FC3D655F91BE283532FA914863416199E04B0