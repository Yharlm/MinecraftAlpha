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

float F(float x)
{
	return x;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 Color = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;
    
	if (F(input.TextureCoordinates.x) < input.TextureCoordinates.y )
	{
		Color.rgb = float3(1.0, 0.0, 0.0);
	}


    return Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
