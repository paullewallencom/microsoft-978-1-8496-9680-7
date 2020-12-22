#include "pch.h"
#include "MenuItem.h"

unique_ptr<SpriteFont> MenuItem::m_font;

MenuItem::MenuItem(float x, float y, wstring text, SA3D_MENU_ITEM id) : 
	m_X(x), 
	m_Y(y), 
	m_text(text), 
	m_id(id)
{
}

void MenuItem::Prepare(ID3D11Device *device)
{
	m_font = unique_ptr<SpriteFont>(new SpriteFont(device, SA3D_MENU_ITEM_FONT_FILE));
}
