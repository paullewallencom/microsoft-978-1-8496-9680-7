#include "pch.h"
#include "Countdown.h"

void Countdown::Prepare(ID3D11Device *device)
{
	m_font = unique_ptr<SpriteFont>(new SpriteFont(device, SA3D_COUNTDOWN_FONT_FILE));
}

void Countdown::Update(float time)
{
	if (m_startTime == 0) { m_startTime = time; }
	if (m_countdownNumber > 0 && time - m_startTime > SA3D_COUNTDOWN_DELAY)
	{
		m_startTime = time;
		m_countdownNumber--;
	}
}

void Countdown::Draw(SpriteBatch *spriteBatch, wstring levelTranslation, float height, float width, int level, float scaleX, float scaleY)
{
	// Draw the level number
	wstring levelString = levelTranslation + L" " + to_wstring(level);
	const wchar_t *levelText = levelString.c_str();
	XMFLOAT3 levelTextSize;
	XMStoreFloat3(&levelTextSize, m_font->MeasureString(levelText));
	int levelX = (int)((SA3D_MAX_SCREEN_WIDTH - levelTextSize.x) / 2);
	m_font->DrawString(
		spriteBatch, 
		levelText, 
		XMFLOAT2(levelX * scaleX, 200 * scaleY), 
		Colors::LightGray, 
		0, 
		XMFLOAT2(0,0), 
		scaleX);

	// Draw the countdown number
	wstring countdownString = to_wstring(m_countdownNumber);
	const wchar_t *countdownText = countdownString.c_str();
	XMFLOAT3 countdownTextSize;
	XMStoreFloat3(&countdownTextSize, m_font->MeasureString(countdownText));
	int countdownX = (int)((SA3D_MAX_SCREEN_WIDTH - countdownTextSize.x) / 2);
	m_font->DrawString(
		spriteBatch, 
		countdownText, 
		XMFLOAT2(countdownX * scaleX, 325 * scaleY), 
		Colors::White, 
		0, 
		XMFLOAT2(0,0), 
		scaleX);
}
