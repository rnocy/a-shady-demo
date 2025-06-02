Shader "Temmie/SurfaceShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _ShadowThreshold("ShadowTHreshold",Range(0,1))=0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Temmie fullforwardshadows
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        float _ShadowThreshold;
        sampler2D _MainTex;
      float4 LightingTemmie(SurfaceOutput s, float3 lightDir, float atten){
            //how much does the normal point towards the light?
            float lambert = dot(s.Normal, lightDir);
            //remap the value from -1 to 1 to between 0 and 1
            float towardsLight =lambert * 0.5 + 0.5;

            //read from toon ramp
            float3 lightIntensity =towardsLight.xxx;
            atten=step(0.1,atten);
            atten=min(1-pow(1-saturate(lambert),10),atten);
            //combine the color
            float4 col;
            //intensity we calculated previously, diffuse color, light falloff and shadowcasting, color of the light
            col.rgb = lightIntensity * s.Albedo * atten * _LightColor0.rgb;
            //col.xyz=atten.xxx;
            //in case we want to make the shader transparent in the future - irrelevant right now
            col.a = s.Alpha; 

            return col;
        }
        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
