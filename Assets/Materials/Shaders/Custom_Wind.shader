// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Wind"
{
	Properties
	{
		_WindSpeed("WindSpeed", Vector) = (1,0,0,0)
		_WindFrequency("WindFrequency", Float) = 1
		_WindStrength("WindStrength", Range( 0 , 2)) = 0.4
		_WindTiling("WindTiling", Vector) = (1,1,0,0)
		_MaskHeight("MaskHeight", Float) = 2
		_MainTex("MainTex", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" }
		LOD 300
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 5.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows nolightmap  nodirlightmap 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _MainTex;
		uniform float _MaskHeight;
		uniform float2 _WindSpeed;
		uniform float2 _WindTiling;
		uniform float _WindFrequency;
		uniform float _WindStrength;
		uniform float _Cutoff = 0.5;


		//https://www.shadertoy.com/view/XdXGW8
		float2 GradientNoiseDir( float2 x )
		{
			const float2 k = float2( 0.3183099, 0.3678794 );
			x = x * k + k.yx;
			return -1.0 + 2.0 * frac( 16.0 * k * frac( x.x * x.y * ( x.x + x.y ) ) );
		}
		
		float GradientNoise( float2 UV, float Scale )
		{
			float2 p = UV * Scale;
			float2 i = floor( p );
			float2 f = frac( p );
			float2 u = f * f * ( 3.0 - 2.0 * f );
			return lerp( lerp( dot( GradientNoiseDir( i + float2( 0.0, 0.0 ) ), f - float2( 0.0, 0.0 ) ),
					dot( GradientNoiseDir( i + float2( 1.0, 0.0 ) ), f - float2( 1.0, 0.0 ) ), u.x ),
					lerp( dot( GradientNoiseDir( i + float2( 0.0, 1.0 ) ), f - float2( 0.0, 1.0 ) ),
					dot( GradientNoiseDir( i + float2( 1.0, 1.0 ) ), f - float2( 1.0, 1.0 ) ), u.x ), u.y );
		}


		float2 voronoihash102( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi102( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash102( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F1;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float4(0,0,1,1).xyz;
			float2 uv_TexCoord40 = i.uv_texcoord * _WindTiling;
			float2 panner36 = ( _Time.y * _WindSpeed + uv_TexCoord40);
			float gradientNoise90 = GradientNoise(panner36,_WindFrequency);
			float temp_output_74_0 = ( pow( abs( i.uv_texcoord.y ) , _MaskHeight ) * ( gradientNoise90 * _WindStrength ) );
			float time102 = 0.0;
			float2 coords102 = panner36 * 1.0;
			float2 id102 = 0;
			float2 uv102 = 0;
			float voroi102 = voronoi102( coords102, time102, id102, uv102, 0 );
			float4 appendResult105 = (float4(( temp_output_74_0 + i.uv_texcoord.x ) , ( ( temp_output_74_0 * voroi102 ) + i.uv_texcoord.y ) , 0.0 , 0.0));
			float4 tex2DNode53 = tex2D( _MainTex, appendResult105.xy );
			o.Albedo = tex2DNode53.rgb;
			o.Alpha = 1;
			clip( tex2DNode53.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18200
753.3334;72.66667;1539;807;2162.505;1058.761;2.277779;True;False
Node;AmplifyShaderEditor.CommentaryNode;86;-2048.179,-285.6149;Inherit;False;1616.018;835.5214;Noise;10;102;96;90;94;84;36;39;40;38;101;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;101;-2030.443,-34.11635;Inherit;False;Property;_WindTiling;WindTiling;3;0;Create;True;0;0;False;0;False;1,1;2,5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;85;-1436.849,-778.525;Inherit;False;876.2339;472.7676;Mask;4;73;72;69;70;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TimeNode;38;-1842.838,226.9532;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;40;-1862.757,-29.66802;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;39;-1823.143,97.13814;Inherit;False;Property;_WindSpeed;WindSpeed;0;0;Create;True;0;0;False;0;False;1,0;1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;36;-1565.256,93.58701;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;69;-1373.719,-642.4485;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;84;-1299.226,59.6162;Inherit;False;Property;_WindFrequency;WindFrequency;1;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;90;-1119.317,-243.5594;Inherit;True;Gradient;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;94;-1101.542,17.12753;Inherit;False;Property;_WindStrength;WindStrength;2;0;Create;True;0;0;False;0;False;0.4;0.541;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-1055.839,-469.3641;Inherit;False;Property;_MaskHeight;MaskHeight;4;0;Create;True;0;0;False;0;False;2;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;70;-1087.777,-717.2634;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;-789.0965,-133.1764;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;72;-861.9218,-617.6236;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;102;-1216.958,264.4944;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;-381.8881,-283.456;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;55;-191.914,-193.889;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;106;-314.7785,250.2245;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;107;-330.4001,122.7678;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;108;-117.4119,-26.69652;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;-78.6798,-370.2672;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;105;138.3715,-257.7467;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;53;317.8241,-549.957;Inherit;True;Property;_MainTex;MainTex;5;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;87;551.9476,-283.1678;Inherit;False;Constant;_Vector0;Vector 0;6;0;Create;True;0;0;False;0;False;0,0,1,1;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;15;813.8884,-529.8354;Float;False;True;-1;7;ASEMaterialInspector;300;0;Standard;Custom/Wind;False;False;False;False;False;False;True;False;True;False;False;False;False;False;True;False;False;False;False;False;False;Off;1;False;-1;1;False;-1;False;0;False;-1;0;False;-1;False;1;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;300;;6;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;40;0;101;0
WireConnection;36;0;40;0
WireConnection;36;2;39;0
WireConnection;36;1;38;2
WireConnection;90;0;36;0
WireConnection;90;1;84;0
WireConnection;70;0;69;2
WireConnection;96;0;90;0
WireConnection;96;1;94;0
WireConnection;72;0;70;0
WireConnection;72;1;73;0
WireConnection;102;0;36;0
WireConnection;74;0;72;0
WireConnection;74;1;96;0
WireConnection;107;0;74;0
WireConnection;107;1;102;0
WireConnection;108;0;107;0
WireConnection;108;1;106;2
WireConnection;56;0;74;0
WireConnection;56;1;55;1
WireConnection;105;0;56;0
WireConnection;105;1;108;0
WireConnection;53;1;105;0
WireConnection;15;0;53;0
WireConnection;15;1;87;0
WireConnection;15;10;53;4
ASEEND*/
//CHKSM=552E450EAD8D2F75FFDA75ACD7A7846F3AC2E280