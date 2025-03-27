Shader "UI Extensions/SoftMaskShader"
{
  Properties
  {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1,1,1,1)
    _StencilComp ("Stencil Comparison", float) = 8
    _Stencil ("Stencil ID", float) = 0
    _StencilOp ("Stencil Operation", float) = 0
    _StencilWriteMask ("Stencil Write Mask", float) = 255
    _StencilReadMask ("Stencil Read Mask", float) = 255
    _ColorMask ("Color Mask", float) = 15
    _AlphaMask ("AlphaMask - Must be Wrapped", 2D) = "white" {}
    _CutOff ("CutOff", float) = 0
    [MaterialToggle] _HardBlend ("HardBlend", float) = 0
    _FlipAlphaMask ("Flip Alpha Mask", float) = 0
    _NoOuterClip ("Outer Clip", float) = 0
  }
  SubShader
  {
    Tags
    { 
      "CanUseSpriteAtlas" = "true"
      "IGNOREPROJECTOR" = "true"
      "PreviewType" = "Plane"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "IGNOREPROJECTOR" = "true"
        "PreviewType" = "Plane"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Cull Off
      Stencil
      { 
        Ref 0
        ReadMask 0
        WriteMask 0
        Pass Keep
        Fail Keep
        ZFail Keep
        PassFront Keep
        FailFront Keep
        ZFailFront Keep
        PassBack Keep
        FailBack Keep
        ZFailBack Keep
      } 
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask 0
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _Color;
      uniform float4 _AlphaMask_ST;
      uniform float4 _TextureSampleAdd;
      uniform sampler2D _MainTex;
      uniform int _UseAlphaClip;
      uniform int _FlipAlphaMask;
      uniform sampler2D _AlphaMask;
      uniform float _CutOff;
      uniform int _HardBlend;
      uniform int _NoOuterClip;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR :COLOR;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR :COLOR;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float2 tmpvar_1;
          tmpvar_1 = in_v.texcoord.xy;
          float4 tmpvar_2;
          float2 tmpvar_3;
          float4 tmpvar_4;
          tmpvar_4.w = 1;
          tmpvar_4.xyz = in_v.vertex.xyz;
          tmpvar_3 = tmpvar_1;
          tmpvar_2 = (in_v.color * _Color);
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_4));
          out_v.xlv_COLOR = tmpvar_2;
          out_v.xlv_TEXCOORD0 = tmpvar_3;
          out_v.xlv_TEXCOORD1 = TRANSFORM_TEX(in_v.vertex.xy, _AlphaMask);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float4 color_2;
          float4 tmpvar_3;
          tmpvar_3 = ((tex2D(_MainTex, in_f.xlv_TEXCOORD0) + _TextureSampleAdd) * in_f.xlv_COLOR);
          color_2 = tmpvar_3;
          float4 tmpvar_4;
          tmpvar_4.xy = float2(bool2(in_f.xlv_TEXCOORD1 >= float2(0, 0)));
          tmpvar_4.zw = float2(bool2(float2(1, 1) >= in_f.xlv_TEXCOORD1));
          int tmpvar_5;
          if((_NoOuterClip==int(0)))
          {
              float4 tmpvar_6;
              tmpvar_6 = bool4(tmpvar_4);
              int tmpvar_7;
              tmpvar_7 = ((tmpvar_6.x && tmpvar_6.y) && (tmpvar_6.z && tmpvar_6.w));
              tmpvar_5 = (tmpvar_7==int(0));
          }
          else
          {
              tmpvar_5 = int(0);
          }
          if(tmpvar_5)
          {
              color_2.w = 0;
          }
          else
          {
              float a_8;
              float tmpvar_9;
              tmpvar_9 = tex2D(_AlphaMask, in_f.xlv_TEXCOORD1).w;
              a_8 = tmpvar_9;
              if((a_8<=_CutOff))
              {
                  a_8 = 0;
              }
              else
              {
                  if(_HardBlend)
                  {
                      a_8 = 1;
                  }
              }
              if((_FlipAlphaMask==1))
              {
                  a_8 = (1 - a_8);
              }
              color_2.w = (color_2.w * a_8);
          }
          if(_UseAlphaClip)
          {
              float x_10;
              x_10 = (color_2.w - 0.001);
              if((x_10<0))
              {
                  discard;
              }
          }
          tmpvar_1 = color_2;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
