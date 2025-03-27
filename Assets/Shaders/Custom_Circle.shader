Shader "Custom/Circle"
{
  Properties
  {
    _Color ("Color", Color) = (1,0,0,0)
    _Thickness ("Thickness", Range(0, 0.5)) = 0.05
    _Radius ("Radius", Range(0, 0.5)) = 0.4
    _Dropoff ("Dropoff", Range(0.01, 4)) = 0.1
  }
  SubShader
  {
    Tags
    { 
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      Blend SrcAlpha OneMinusSrcAlpha
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
      uniform float _Thickness;
      uniform float _Radius;
      uniform float _Dropoff;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXTCOORD0 :TEXTCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXTCOORD0 :TEXTCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1.w = 1;
          tmpvar_1.xyz = in_v.vertex.xyz;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_1));
          out_v.xlv_TEXTCOORD0 = (in_v.texcoord.xy - float2(0.5, 0.5));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float d_2;
          d_2 = sqrt(((in_f.xlv_TEXTCOORD0.x * in_f.xlv_TEXTCOORD0.x) + (in_f.xlv_TEXTCOORD0.y * in_f.xlv_TEXTCOORD0.y)));
          float tmpvar_3;
          if((d_2<(_Radius - (0.5 * _Thickness))))
          {
              float tmpvar_4;
              tmpvar_4 = ((d_2 - _Radius) + (0.5 * _Thickness));
              float tmpvar_5;
              tmpvar_5 = (_Dropoff * _Thickness);
              tmpvar_3 = (((-(tmpvar_4 * tmpvar_4)) / (tmpvar_5 * tmpvar_5)) + 1);
          }
          else
          {
              if((d_2>(_Radius + (0.5 * _Thickness))))
              {
                  float tmpvar_6;
                  tmpvar_6 = ((d_2 - _Radius) - (0.5 * _Thickness));
                  float tmpvar_7;
                  tmpvar_7 = (_Dropoff * _Thickness);
                  tmpvar_3 = (((-(tmpvar_6 * tmpvar_6)) / (tmpvar_7 * tmpvar_7)) + 1);
              }
              else
              {
                  tmpvar_3 = 1;
              }
          }
          float4 tmpvar_8;
          tmpvar_8.xyz = _Color.xyz;
          tmpvar_8.w = (_Color.w * tmpvar_3);
          tmpvar_1 = tmpvar_8;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
