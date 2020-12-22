#include "pch.h"
#include "RocketDisplay.h"

void RocketDisplay::Prepare(ID3D11Device *device)
{
	CreateDDSTextureFromFile(device, SA3D_DISPLAY_TEXTURE_FILE, nullptr, &m_texture, MAXSSIZE_T);
	m_font = unique_ptr<SpriteFont>(new SpriteFont(device, SA3D_DISPLAY_FONT_FILE));
}

void RocketDisplay::Draw(SpriteBatch *spriteBatch, float height, float width, int level, int score, int rockets, float scaleX, float scaleY)
{
	// Draw the bitmap
	float textureWidth = SA3D_MAX_SCREEN_HEIGHT * SA3D_MAX_SCREEN_WIDTH / (height / scaleX);
	float margin = (SA3D_DISPLAY_TEXTURE_WIDTH - textureWidth) / 2;
	spriteBatch->Draw(
		m_texture, 
		XMFLOAT2(-margin * scaleX, 0), 
		nullptr, 
		Colors::White, 
		0, 
		XMFLOAT2(0, 0), 
		scaleY, 
		SpriteEffects_None);

	// Draw the level number
	float yCoordinate = (SA3D_MAX_SCREEN_HEIGHT - SA3D_DISPLAY_MARGIN_BOTTOM) * scaleY;
	float levelX = (SA3D_DISPLAY_MARGIN_SIDE - margin) * scaleX;
	wstring levelString = to_wstring(level);
	const wchar_t *levelText = levelString.c_str();
	m_font->DrawString(
		spriteBatch, 
		levelText, 
		XMFLOAT2(levelX, yCoordinate), 
		Colors::LightGray, 
		0, 
		XMFLOAT2(0,0), 
		scaleX);

	// Draw the number of remaining rockets
	wstring rocketsString = to_wstring(rockets);
	const wchar_t *rocketsText = rocketsString.c_str();
	XMFLOAT3 rocketsTextSize;
	XMStoreFloat3(&rocketsTextSize, m_font->MeasureString(rocketsText));
	float rocketsX = (SA3D_MAX_SCREEN_WIDTH - SA3D_DISPLAY_MARGIN_SIDE + margin - rocketsTextSize.x) * scaleX;
	XMVECTOR rocketsColor = Colors::LightGray;
	if (rockets == 2) 
	{ 
		rocketsColor = Colors::Yellow; 
	}
	else if (rockets == 1) 
	{ 
		rocketsColor = Colors::Red; 
	}
	m_font->DrawString(
		spriteBatch, 
		rocketsText, 
		XMFLOAT2(rocketsX, yCoordinate), 
		rocketsColor, 
		0, 
		XMFLOAT2(0,0), 
		scaleX);

	// Draw the score
	wstring scoreString = to_wstring(score);
	const wchar_t *scoreText = scoreString.c_str();
	XMFLOAT3 scoreTextSize;
	XMStoreFloat3(&scoreTextSize, m_font->MeasureString(scoreText));
	float scoreX = ((SA3D_MAX_SCREEN_WIDTH - scoreTextSize.x) / 2) * scaleX;
	m_font->DrawString(
		spriteBatch, 
		scoreText, 
		XMFLOAT2(scoreX, yCoordinate), 
		Colors::DarkGray, 
		0, 
		XMFLOAT2(0,0), 
		scaleX);
}
