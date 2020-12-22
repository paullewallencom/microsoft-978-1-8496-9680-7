#pragma once
#include <memory>
#include <vector>
#include "SpriteBatch.h"
#include "DDSTextureLoader.h"
#include "Constants.h"
#include "MenuItem.h"
#include "MenuLabel.h"
#include "MenuButton.h"

using namespace std;
using namespace DirectX;

// The class representing the single menu
class Menu
{
public:
	// Prepares the menu by loading necessary resources
	void static Prepare(ID3D11Device *device);

	// Draws the menu on the screen
	void Draw(SpriteBatch *spriteBatch, float scaleX, float scaleY);

	// Adds a new label to the menu
	void AddLabel(float x, float y, wstring text);

	// Adds a new button to the menu
	void AddButton(float x, float y, float width, float height, wstring text, SA3D_MENU_ITEM id);

	// Called when the menu screen is pressed by the user
	// The method checks whether any button is pressed - if so, its identifier is returned
	SA3D_MENU_ITEM OnPress(float x, float y, float scaleX, float scaleY);

	// Removes all elements from the menu
	void Clear() { m_items.clear(); }

private:
	// Texture used as a background of the menu
	static ID3D11ShaderResourceView *m_textureBackground;

	// Data of all menu items
	vector<unique_ptr<MenuItem>> m_items;
};
