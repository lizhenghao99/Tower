// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.33 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.33;sub:START;pass:START;ps:flbk:Standard,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:False,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:2,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:2805,x:33908,y:33083,varname:node_2805,prsc:2|diff-965-OUT,lwrap-7233-RGB;n:type:ShaderForge.SFN_Slider,id:4703,x:32883,y:32881,ptovrint:False,ptlb:Rim Power,ptin:_RimPower,varname:_RimPower,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Fresnel,id:6298,x:32678,y:32979,varname:node_6298,prsc:2|EXP-2334-OUT;n:type:ShaderForge.SFN_Slider,id:2334,x:32326,y:33001,ptovrint:False,ptlb:Rim Distance,ptin:_RimDistance,varname:_RimDistance,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:5,cur:2,max:0;n:type:ShaderForge.SFN_Color,id:3411,x:32671,y:33124,ptovrint:False,ptlb:Rim Color 1,ptin:_RimColor1,varname:_RimColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.8308824,c2:0.9790061,c3:1,c4:1;n:type:ShaderForge.SFN_Lerp,id:8695,x:32883,y:33107,varname:node_8695,prsc:2|A-9634-RGB,B-3411-RGB,T-6468-V;n:type:ShaderForge.SFN_Color,id:9634,x:32671,y:33295,ptovrint:False,ptlb:Rim Color 2,ptin:_RimColor2,varname:_RimColor_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.462069,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:5742,x:32883,y:32968,varname:node_5742,prsc:2|A-6080-OUT,B-6298-OUT;n:type:ShaderForge.SFN_TexCoord,id:6468,x:32671,y:33446,varname:node_6468,prsc:2,uv:0;n:type:ShaderForge.SFN_Vector1,id:6080,x:32678,y:32903,varname:node_6080,prsc:2,v1:2;n:type:ShaderForge.SFN_Color,id:3524,x:32678,y:32388,ptovrint:False,ptlb:Diffuse Color 1,ptin:_DiffuseColor1,varname:_RimColor02,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.6413792,c3:0,c4:1;n:type:ShaderForge.SFN_Lerp,id:5412,x:33046,y:32455,varname:node_5412,prsc:2|A-9232-RGB,B-3524-RGB,T-3433-OUT;n:type:ShaderForge.SFN_Color,id:9232,x:32678,y:32555,ptovrint:False,ptlb:Diffuse Color 2,ptin:_DiffuseColor2,varname:_RimColor03,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.1241379,c3:0,c4:1;n:type:ShaderForge.SFN_TexCoord,id:1659,x:32527,y:32722,varname:node_1659,prsc:2,uv:0;n:type:ShaderForge.SFN_RemapRange,id:3433,x:32678,y:32722,varname:node_3433,prsc:2,frmn:0.1,frmx:0.9,tomn:0,tomx:1|IN-1659-V;n:type:ShaderForge.SFN_Lerp,id:965,x:33628,y:33084,varname:node_965,prsc:2|A-5412-OUT,B-8695-OUT,T-1877-OUT;n:type:ShaderForge.SFN_Multiply,id:1877,x:33224,y:32961,varname:node_1877,prsc:2|A-4703-OUT,B-5742-OUT;n:type:ShaderForge.SFN_Color,id:7233,x:33628,y:33261,ptovrint:False,ptlb:Light Color,ptin:_LightColor,varname:node_7233,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;proporder:3524-9232-3411-9634-4703-2334-7233;pass:END;sub:END;*/

Shader "FrostForged/Gradient Rimlight" {
    Properties {
        _DiffuseColor1 ("Diffuse Color 1", Color) = (1,0.6413792,0,1)
        _DiffuseColor2 ("Diffuse Color 2", Color) = (1,0.1241379,0,1)
        _RimColor1 ("Rim Color 1", Color) = (0.8308824,0.9790061,1,1)
        _RimColor2 ("Rim Color 2", Color) = (0,0.462069,1,1)
        _RimPower ("Rim Power", Range(0, 1)) = 1
        _RimDistance ("Rim Distance", Range(5, 0)) = 2
        _LightColor ("Light Color", Color) = (0,0,0,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform fixed _RimPower;
            uniform fixed _RimDistance;
            uniform float4 _RimColor1;
            uniform float4 _RimColor2;
            uniform float4 _DiffuseColor1;
            uniform float4 _DiffuseColor2;
            uniform float4 _LightColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 w = _LightColor.rgb*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = forwardLight * attenColor;
                float3 diffuseColor = lerp(lerp(_DiffuseColor2.rgb,_DiffuseColor1.rgb,(i.uv0.g*1.25+-0.125)),lerp(_RimColor2.rgb,_RimColor1.rgb,i.uv0.g),(_RimPower*(2.0*pow(1.0-max(0,dot(normalDirection, viewDirection)),_RimDistance))));
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform fixed _RimPower;
            uniform fixed _RimDistance;
            uniform float4 _RimColor1;
            uniform float4 _RimColor2;
            uniform float4 _DiffuseColor1;
            uniform float4 _DiffuseColor2;
            uniform float4 _LightColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 w = _LightColor.rgb*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = forwardLight * attenColor;
                float3 diffuseColor = lerp(lerp(_DiffuseColor2.rgb,_DiffuseColor1.rgb,(i.uv0.g*1.25+-0.125)),lerp(_RimColor2.rgb,_RimColor1.rgb,i.uv0.g),(_RimPower*(2.0*pow(1.0-max(0,dot(normalDirection, viewDirection)),_RimDistance))));
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Standard"
    CustomEditor "ShaderForgeMaterialInspector"
}
