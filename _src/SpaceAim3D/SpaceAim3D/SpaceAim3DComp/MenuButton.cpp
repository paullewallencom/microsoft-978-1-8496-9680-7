#include "pch.h"
#include "MenuButton.h"

ID3D11ShaderResourceView *MenuButton::m_textureItem;

MenuButton::MenuButton(float x, float y, float width, float height, wstring text, SA3D_MENU_ITEM id) 
	: MenuItem(x, y, text, id), 
	m_width(width), 
	m_height(height)
{
}

void MenuButton::Prepare(ID3D11Device *device)
{
	CreateDDSTextureFromFile(
		device, 
		SA3D_MENU_ITEM_BACKGROUND_FILE, 
		nullptr, 
		&m_textureItem, 
		MAXSSIZE_T);
}

void MenuButton::Draw(SpriteBatch *spriteBatch, float scaleX, float scaleY)
{
	// Draw item background
	float x = m_X * scaleX;
	float y = m_Y * scaleY;
	float width = (m_X + m_width) * scaleX - x;
	float height = (m_Y + m_height) * scaleY - y;
	XMFLOAT2 position(x, y);
	RECT rectangle = { 0, 0, (LONG)width, (LONG)height };
	spriteBatch->Draw(
		m_textureItem, 
		position, 
		&rectangle, 
		Colors::White, 
		0, 
		XMFLOAT2(0, 0), 
		1.0f);

	// Draw caption
	const wchar_t *captionText = m_text.c_str();
	XMFLOAT3 captionSize;
	XMStoreFloat3(&captionSize, m_font->MeasureString(captionText));
	XMFLOAT2 captionPosition(
		x + (m_width - captionSize.x) * scaleX / 2,
		y + (m_height - captionSize.y) * scaleY / 2);
	m_font->DrawString(
		spriteBatch, 
		captionText, 
		captionPosition, 
		Colors::Black, 
		0, 
		XMFLOAT2(0, 0), 
		scaleX);
}

bool MenuButton::IsPressed(float x, float y)
{
	return x >= m_X && x <= m_X + m_width 
		&& y >= m_Y && y <= m_Y + m_height;
}