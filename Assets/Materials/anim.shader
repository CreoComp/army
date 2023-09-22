Shader"Custom/anim"
{
    Properties
    {
        _MainTex1 ("Texture 1", 2D) = "white" {}
        _MainTex2 ("Texture 2", 2D) = "white" {}
        _BlendFactor ("Blend Factor", Range(0, 1)) = 0.5
    }
 
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
LOD200
 
        CGPROGRAM
        #pragma surface surf Lambert
 
sampler2D _MainTex1;
sampler2D _MainTex2;
float _BlendFactor;
 
struct Input
{
    float2 uv_MainTex;
    float animTime;
};
 
void surf(Input IN, inout SurfaceOutput o)
{
    float3 color1 = tex2D(_MainTex1, IN.uv_MainTex).rgb;
    float3 color2 = tex2D(_MainTex2, IN.uv_MainTex).rgb;
 
            // Blend the colors based on the animation time
    o.Albedo = lerp(color1, color2, _BlendFactor);
}
        ENDCG
    }
FallBack"Diffuse"
}