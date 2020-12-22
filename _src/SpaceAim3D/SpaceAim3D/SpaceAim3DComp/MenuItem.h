#pragma once
#include <string>
#include "Constants.h"
#include "SpriteBatch.h"
#include "SpriteFont.h"
#include "DDSTextureLoader.h"
using namespace std;
using namespace DirectX;

// The class representing a single item (either a button or a label), 
// which can be placed in the menu
class MenuItem
{
public:
	// Prepares necessary resources
	void static Prepare(ID3D11Device *device);

	// Initializes basic data, like a location, a text, and an identifier
	MenuItem(float x, float y, wstring text, SA3D_MENU_ITEM id);

	// Methods returning basic data
	float GetX() { return m_X; }
	float GetY() { return m_Y; }
	wstring GetText() { return m_text; }
	SA3D_MENU_ITEM GetId() { return m_id; }

	// Draws the item on the screen
	// It is a purely virtual method
	virtual void Draw(SpriteBatch *spriteBatch, float scaleX, float scaleY) = 0;

	// Returns a value indicating whether the item is pressed
	virtual bool IsPressed(float x, float y) { return false; }

protected:
	// X and Y coordinates
	float m_X;
	float m_Y;

	// A displayed text
	wstring m_text;

	// An identifier
	SA3D_MENU_ITEM m_id;

	// A font used to write the text
	static unique_ptr<SpriteFont> m_font;
};
