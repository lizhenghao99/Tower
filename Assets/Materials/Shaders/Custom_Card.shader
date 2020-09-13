Shader "Custom/Card"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)

        _Saturation("Saturation", Range(0,1)) = 1
        [HDR]_OutlineColor("Outline Color", Color) = (1,1,1,1)
        _OutlineStrength("Outline Strength", Float) = 1
        _OutlineRange("Outline Range", Float) = 20
        _OutlineThreshold("Outline Threshold", Range(0,1)) = 0.5

        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255

        _ColorMask("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }

            Stencil
            {
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }

            Cull Off
            Lighting Off
            ZWrite Off
            ZTest[unity_GUIZTestMode]
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask[_ColorMask]

            Pass
            {
                Name "Default"
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0

                #include "UnityCG.cginc"
                #include "UnityUI.cginc"

                #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
                #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

                struct appdata_t
                {
                    float4 vertex   : POSITION;
                    float4 color    : COLOR;
                    float2 texcoord : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f
                {
                    float4 vertex   : SV_POSITION;
                    fixed4 color : COLOR;
                    float2 texcoord  : TEXCOORD0;
                    float4 worldPosition : TEXCOORD1;
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                sampler2D _MainTex;
                fixed4 _Color;
                fixed4 _TextureSampleAdd;
                float4 _ClipRect;
                float4 _MainTex_ST;
                float _Saturation;
                fixed4 _OutlineColor;
                float _OutlineStrength;
                float _OutlineRange;
                float _OutlineThreshold;

                float4 _MainTex_TexelSize;

                v2f vert(appdata_t v)
                {
                    v2f OUT;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                    OUT.worldPosition = v.vertex;
                    OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                    OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                    OUT.color = v.color * _Color;
                    return OUT;
                }

                fixed4 frag(v2f IN) : SV_Target
                {
                    half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

                    #ifdef UNITY_UI_CLIP_RECT
                    color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                    #endif

                    #ifdef UNITY_UI_ALPHACLIP
                    clip(color.a - 0.001);
                    #endif

             
                    // custom effects

                    // outline
                    float2 texDdx = ddx(IN.texcoord);
                    float2 texDdy = ddy(IN.texcoord);
                    float count = 0;

                    for (int i = 1; i <= _OutlineRange; i++)
                    {
                        float prevCount = count;
                        float2 pixelUpTexCoord = IN.texcoord + float2(0, i * _MainTex_TexelSize.y);
                        fixed pixelUpAlpha = tex2Dgrad(_MainTex, pixelUpTexCoord, texDdx, texDdy).a;
                        if (pixelUpAlpha > _OutlineThreshold) count += 1;

                        float2 pixelDownTexCoord = IN.texcoord - float2(0, i * _MainTex_TexelSize.y);
                        fixed pixelDownAlpha = tex2Dgrad(_MainTex, pixelDownTexCoord, texDdx, texDdy).a;
                        if (pixelDownAlpha > _OutlineThreshold) count += 1;

                        float2 pixelRightTexCoord = IN.texcoord + float2(i * _MainTex_TexelSize.x, 0);
                        fixed pixelRightAlpha = tex2Dgrad(_MainTex, pixelRightTexCoord, texDdx, texDdy).a;
                        if (pixelRightAlpha > _OutlineThreshold) count += 1;

                        float2 pixelLeftTexCoord = IN.texcoord - float2(i * _MainTex_TexelSize.x, 0);
                        fixed pixelLeftAlpha = tex2Dgrad(_MainTex, pixelLeftTexCoord, texDdx, texDdy).a;
                        if (pixelLeftAlpha > _OutlineThreshold) count += 1;

                        if (count - prevCount > 1)
                        {
                            continue;
                        }

                        float2 pixelNETexCoord = IN.texcoord + float2(i * _MainTex_TexelSize.x, i * _MainTex_TexelSize.y);
                        fixed pixelNEAlpha = tex2Dgrad(_MainTex, pixelNETexCoord, texDdx, texDdy).a;
                        if (pixelNEAlpha > _OutlineThreshold)
                        {
                            count += 4;
                            continue;
                        }

                        float2 pixelNWTexCoord = IN.texcoord + float2(-i * _MainTex_TexelSize.x, i * _MainTex_TexelSize.y);
                        fixed pixelNWAlpha = tex2Dgrad(_MainTex, pixelNWTexCoord, texDdx, texDdy).a;
                        if (pixelNWAlpha > _OutlineThreshold)
                        {
                            count += 4;
                            continue;
                        }

                        float2 pixelSETexCoord = IN.texcoord + float2(i * _MainTex_TexelSize.x, -i * _MainTex_TexelSize.y);
                        fixed pixelSEAlpha = tex2Dgrad(_MainTex, pixelSETexCoord, texDdx, texDdy).a;
                        if (pixelSEAlpha > _OutlineThreshold)
                        {
                            count += 4;
                            continue;
                        }

                        float2 pixelSWTexCoord = IN.texcoord + float2(-i * _MainTex_TexelSize.x, -i * _MainTex_TexelSize.y);
                        fixed pixelSWAlpha = tex2Dgrad(_MainTex, pixelSWTexCoord, texDdx, texDdy).a;
                        if (pixelSWAlpha > _OutlineThreshold)
                        {
                            count += 4;
                            continue;
                        }
                    }

                    float ratio = count * _OutlineStrength / (_OutlineRange * 8);

                    if (color.a < _OutlineThreshold)
                    {   
                        color = lerp(color, _OutlineColor, ratio);
                    }

                    // gray scale
                    half grayscale = Luminance(color);
                    color.rgb = lerp(grayscale, color.rgb, _Saturation);

                    return color;
                }
            ENDCG
            }
        }
}