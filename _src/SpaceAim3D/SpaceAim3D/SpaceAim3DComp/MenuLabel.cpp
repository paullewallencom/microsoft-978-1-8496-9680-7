#include "pch.h"
#include "MenuLabel.h"

MenuLabel::MenuLabel(float x, float y, wstring text)
	: MenuItem(x, y, text, SA3D_MENU_NONE)
{
}

void MenuLabel::Draw(SpriteBatch *spriteBatch, float scaleX, float scaleY)
{
	const wchar_t *text = m_text.c_str();
	XMFLOAT2 position(m_X * scaleX, m_Y * scaleY);
	m_font->DrawString(
		spriteBatch, 
		text, 
		position, 
		Colors::LightGray, 
		0, 
		XMFLOAT2(0, 0), 
		scaleX);
}