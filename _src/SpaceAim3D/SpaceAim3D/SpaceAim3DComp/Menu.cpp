#include "pch.h"
#include "Menu.h"

ID3D11ShaderResourceView *Menu::m_textureBackground;

void Menu::Prepare(ID3D11Device *device)
{
	CreateDDSTextureFromFile(
		device, 
		SA3D_MENU_BACKGROUND_FILE, 
		nullptr, 
		&m_textureBackground, 
		MAXSSIZE_T);
	MenuItem::Prepare(device);
	MenuButton::Prepare(device);
}

void Menu::Draw(SpriteBatch *spriteBatch, float scaleX, float scaleY)
{
	// Draw the background texture
	spriteBatch->Draw(
		m_textureBackground, 
		XMFLOAT2(0, 0), 
		nullptr, 
		Colors::White, 
		0.0f, 
		XMFLOAT2(0, 0), 
		scaleX);

	// Draw all menu items
	for (auto item = m_items.begin(); item != m_items.end(); item++)
	{
		item->get()->Draw(spriteBatch, scaleX, scaleY);
	}
}

void Menu::AddLabel(float x, float y, wstring text)
{
	unique_ptr<MenuItem> item(new MenuLabel(x, y, text));
	m_items.push_back(move(item));
}

void Menu::AddButton(float x, float y, float width, float height, wstring text, SA3D_MENU_ITEM id)
{
	unique_ptr<MenuItem> item(new MenuButton(x, y, width, height, text, id));
	m_items.push_back(move(item));
}

SA3D_MENU_ITEM Menu::OnPress(float x, float y, float scaleX, float scaleY)
{
	x = x * SA3D_MAX_SCREEN_WIDTH / 800;
	y = y * SA3D_MAX_SCREEN_HEIGHT / 480;
	if (scaleX != scaleY) { x *= scaleY; }

	for (auto item = m_items.begin(); item != m_items.end(); item++)
	{
		if (item->get()->IsPressed(x, y))
		{
			return item->get()->GetId();
		}
	}

	return SA3D_MENU_NONE;
}
