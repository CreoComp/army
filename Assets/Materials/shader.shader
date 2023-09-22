Shader"Custom/shader" {
    Properties {
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
    }

    SubShader {
        Tags { "RenderType" = "Opaque" }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma multi_compile_fwdbase

#include "UnityCG.cginc"

float4 _MainColor;

struct appdata_t
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
    float4 vertex : SV_POSITION;
    float3 worldNormal : TEXCOORD0;
    float3 worldPos : TEXCOORD1;
    fixed4 color : COLOR;
};

float4 _Colors[1023]; // Max instanced batch size.

v2f vert(appdata_t i, uint instanceID : SV_InstanceID)
{
    UNITY_SETUP_INSTANCE_ID(i);

    v2f o;
    o.vertex = UnityObjectToClipPos(i.vertex);
    o.worldNormal = UnityObjectToWorldNormal(i.normal);
    o.worldPos = mul(unity_ObjectToWorld, i.vertex).xyz;
    o.color = float4(1, 1, 1, 1);

#ifdef UNITY_INSTANCING_ENABLED
                    o.color = _Colors[instanceID];
#endif

    return o;
}

fixed LambertianLighting(float3 normal)
{
    float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
    fixed ndotl = max(0, dot(normal, lightDir));
    fixed ambient = 0.2; // Значение окружающего освещения, можно настроить по вашему вкусу.
    return ndotl + ambient;
}

fixed4 frag(v2f i) : SV_Target
{
    fixed lightIntensity = LambertianLighting(normalize(i.worldNormal));
    return i.color * _MainColor * lightIntensity;
}

            ENDCG
        }
    }
}