#pragma once
#include "MenuItem.h"

// The class representing the single button, which can be placed in the menu
class MenuButton : public MenuItem
{
public:
	// Prepares necessary resources
	void static Prepare(ID3D11Device *device);

	// Initializes a new instance
	MenuButton(float x, float y, float width, float height, wstring text, SA3D_MENU_ITEM id);

	// Return width and height of the button
	float GetWidth() { return m_width; }
	float GetHeight() { return m_height; }

	// Draws the button on the screen
	void Draw(SpriteBatch *spriteBatch, float scaleX, float scaleY);

	// Returns a value indicating whether the button is pressed
	bool IsPressed(float x, float y);

private:
	// Dimensions of the button
	float m_width;
	float m_height;

	// Texture used as a background of the button
	static ID3D11ShaderResourceView *m_textureItem;
};

