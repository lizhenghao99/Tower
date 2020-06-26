// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Loot Beams/Beam/Additive(+Ripple)"
{
	Properties
	{
		_Glow("Glow", Float) = 1
		_TintColor("Color", Color) = (1,1,1,1)
		_GlobalColor("Global Color", Range( 0 , 1)) = 1
		[NoScaleOffset]_MainTex("Main Texture", 2D) = "white" {}
		_BotFadeRange("Bot Fade Range", Range( 0 , 1)) = 0
		_BotFadeSmooth("Bot Fade Smooth", Range( 0 , 1)) = 0
		[NoScaleOffset]_RippleTexture("Ripple Texture", 2D) = "white" {}
		_RippleValue("Ripple Value", Float) = 0.1
		_RippleScale("Ripple Scale", Float) = 1
		_RippleScroll("Ripple Scroll", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		Blend SrcAlpha One , SrcAlpha One
		
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
		uniform sampler2D _RippleTexture;
		uniform float _RippleScale;
		uniform float _RippleScroll;
		uniform float _RippleValue;
		uniform float _GlobalColor;
		uniform float _BotFadeRange;
		uniform float _BotFadeSmooth;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult39 = (float2(_RippleScroll , 0.0));
			float2 temp_cast_0 = (tex2D( _RippleTexture, ( (i.uv_texcoord*_RippleScale + 0.0) + ( appendResult39 * _Time.y ) ) ).r).xx;
			float2 lerpResult31 = lerp( i.uv_texcoord , temp_cast_0 , _RippleValue);
			float4 tex2DNode3 = tex2D( _MainTex, lerpResult31 );
			o.Emission = ( _Glow * ( _TintColor * _TintColor.a * tex2DNode3 * i.vertexColor ) ).rgb;
			float clampResult26 = clamp( ( ( ( 1.0 - i.uv_texcoord.x ) - ( ( _BotFadeRange * ( _BotFadeSmooth + 1.0 ) ) - _BotFadeSmooth ) ) * ( 1.0 / _BotFadeSmooth ) ) , 0.0 , 1.0 );
			o.Alpha = ( _GlobalColor * ( ( tex2DNode3.a * i.vertexColor.a * _TintColor.a ) * clampResult26 ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17000
7;2;1906;1010;2081.863;833.8704;2.25429;True;False
Node;AmplifyShaderEditor.RangedFloatNode;40;-4127.858,474.9701;Float;False;Property;_RippleScroll;Ripple Scroll;10;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-3353.319,424.251;Float;False;Property;_RippleScale;Ripple Scale;9;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;34;-3415.48,747.137;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;39;-3832.126,480.051;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;38;-3867.077,644.6025;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-3454.938,1289.657;Float;False;Constant;_Float;Float;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-3195.58,1181.072;Float;False;Property;_BotFadeSmooth;Bot Fade Smooth;6;0;Create;True;0;0;False;0;0;0.06;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;48;-3036.969,456.8727;Float;True;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-3586.026,478.5947;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-2880.86,952.3091;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-2941.686,850.0012;Float;False;Property;_BotFadeRange;Bot Fade Range;5;0;Create;True;0;0;False;0;0;0.06;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-2628.119,457.5832;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-2494.945,929.2202;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;20;-2317.752,1152.995;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;27;-2056.275,768.9305;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;14;-2114.324,928.7678;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;12;-2887.497,1289.11;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;57;-2063.465,674.2554;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-2200.406,321.0336;Float;False;Property;_RippleValue;Ripple Value;8;0;Create;True;0;0;False;0;0.1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;32;-2321.619,431.0085;Float;True;Property;_RippleTexture;Ripple Texture;7;1;[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;31;-1883.677,435.0363;Float;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;11;-1600.756,905.4576;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;28;-1519.52,1224.457;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-1398.36,219.7157;Float;False;Property;_TintColor;Color;2;0;Create;False;0;0;False;0;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1213.051,906.0242;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-1482.829,406.1823;Float;True;Property;_MainTex;Main Texture;4;1;[NoScaleOffset];Create;False;0;0;False;0;None;22f0f59668faf9c4b82a93cfbbe402bf;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;4;-1363.641,610.773;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-1016.549,503.1689;Float;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;26;-763.6918,906.4675;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-960.7834,133.9906;Float;False;Property;_Glow;Glow;1;0;Create;True;0;0;False;0;1;0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-638.5269,754.9745;Float;False;Property;_GlobalColor;Global Color;3;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1;-1019.051,224.497;Float;True;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-636.0312,505.5287;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-302.8032,483.7315;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-633.8436,205.6524;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;27.39894,165.0963;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Loot Beams/Beam/Additive(+Ripple);False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;8;5;False;-1;1;False;-1;8;5;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;39;0;40;0
WireConnection;48;0;34;0
WireConnection;48;1;49;0
WireConnection;36;0;39;0
WireConnection;36;1;38;0
WireConnection;16;0;18;0
WireConnection;16;1;17;0
WireConnection;33;0;48;0
WireConnection;33;1;36;0
WireConnection;15;0;25;0
WireConnection;15;1;16;0
WireConnection;20;0;18;0
WireConnection;27;0;34;1
WireConnection;14;0;15;0
WireConnection;14;1;20;0
WireConnection;12;0;17;0
WireConnection;12;1;18;0
WireConnection;57;0;34;0
WireConnection;32;1;33;0
WireConnection;31;0;57;0
WireConnection;31;1;32;1
WireConnection;31;2;42;0
WireConnection;11;0;27;0
WireConnection;11;1;14;0
WireConnection;28;0;12;0
WireConnection;9;0;11;0
WireConnection;9;1;28;0
WireConnection;3;1;31;0
WireConnection;5;0;3;4
WireConnection;5;1;4;4
WireConnection;5;2;2;4
WireConnection;26;0;9;0
WireConnection;1;0;2;0
WireConnection;1;1;2;4
WireConnection;1;2;3;0
WireConnection;1;3;4;0
WireConnection;8;0;5;0
WireConnection;8;1;26;0
WireConnection;29;0;30;0
WireConnection;29;1;8;0
WireConnection;7;0;6;0
WireConnection;7;1;1;0
WireConnection;0;2;7;0
WireConnection;0;9;29;0
ASEEND*/
//CHKSM=829F5F0C37FCF6F1AEAFEBAA0FACA7CB02B6C1A6