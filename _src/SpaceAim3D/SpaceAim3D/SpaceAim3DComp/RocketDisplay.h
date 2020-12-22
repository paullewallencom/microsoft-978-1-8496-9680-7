#pragma once
#include <string>
#include "SpriteBatch.h"
#include "SpriteFont.h"
#include "DDSTextureLoader.h"
#include "Constants.h"

using namespace std;
using namespace DirectX;

// The class representing the rocket display
class RocketDisplay
{
public:
	// Prepares necessary resources, i.e. a texture and a sprite font
	void Prepare(ID3D11Device *device);

	// Draws the rocket display on the screen
	void Draw(SpriteBatch *spriteBatch, float height, float width, int level, int score, int rockets, float scaleX, float scaleY);

private:
	// The font used on the rocket display
	unique_ptr<SpriteFont> m_font;

	// The texture representing the rocket display
	ID3D11ShaderResourceView *m_texture;
};
