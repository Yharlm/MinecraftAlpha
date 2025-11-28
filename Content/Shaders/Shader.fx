#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};


struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 Color = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;
    

    return Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};

// =======================================================
// Vertex → Geometry → Pixel outline expansion shader
// Expands a quad outward to create extra pixels
// =======================================================

Texture2D MainTex : register(t0);
SamplerState Samp  : register(s0);

cbuffer ExpandSettings : register(b0)
{
    float2 ExpandPixels;   // e.g. float2(1.0, 1.0) for 32→34
};


// ---------------------------
// Vertex Shader
// ---------------------------
struct VS_IN
{
    float3 pos : POSITION;
    float2 uv  : TEXCOORD0;
};

struct VS_OUT
{
    float4 pos : SV_POSITION;
    float2 uv  : TEXCOORD0;
};

VS_OUT VS_Main(VS_IN input)
{
    VS_OUT o;
    o.pos = float4(input.pos, 1.0);
    o.uv  = input.uv;
    return o;
}


// ---------------------------
// Geometry Shader
// Expands quad outward
// ---------------------------
[maxvertexcount(4)]
void GS_Expand(triangle VS_OUT input[3], inout TriangleStream<VS_OUT> triStream)
{
    VS_OUT v0 = input[0];
    VS_OUT v1 = input[1];
    VS_OUT v2 = input[2];

    // Create a fourth vertex (the missing one)
    VS_OUT v3 = v0;

    float2 expand = ExpandPixels;

    // Expand all vertices outward (screen-space)
    // You may flip +/- depending on winding order

    v0.pos.xy += float2(-expand.x,  expand.y); // top-left
    v1.pos.xy += float2( expand.x,  expand.y); // top-right
    v2.pos.xy += float2( expand.x, -expand.y); // bottom-right
    v3.pos.xy += float2(-expand.x, -expand.y); // bottom-left

    // Output expanded quad as two triangles
    triStream.Append(v0);
    triStream.Append(v1);
    triStream.Append(v2);
    triStream.RestartStrip();

    triStream.Append(v0);
    triStream.Append(v2);
    triStream.Append(v3);
    triStream.RestartStrip();
}


// ---------------------------
// Pixel Shader
// ---------------------------
float4 PS_Main(VS_OUT input) : SV_TARGET
{
    return MainTex.Sample(Samp, input.uv);
}


// ---------------------------
// Technique
// ---------------------------
technique11 ExpandQuad
{
    pass P0
    {
        SetVertexShader( CompileShader(vs_5_0, VS_Main()) );
        SetGeometryShader( CompileShader(gs_5_0, GS_Expand()) );
        SetPixelShader( CompileShader(ps_5_0, PS_Main()) );
    }
}
