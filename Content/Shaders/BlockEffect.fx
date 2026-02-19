#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


Texture2D SpriteTexture;
float ID;




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

float f(float x,float dir)
{
    return (x+dir)/7+0.1;
}
float g(float x)
{
    return x;
}


float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 Color = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;
    Color.rg *= 0.2;
    float fx = input.TextureCoordinates.x*1./40.;
    float fy = fmod(input.TextureCoordinates.y,1./40.);
    if(f(fx,0.) < fy)
	{
		Color = 0.;
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


