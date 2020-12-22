#pragma once
#include "MenuItem.h"

// The class representing the single label, which can be placed in the menu
class MenuLabel : public MenuItem
{
public:
	// Initializes the label
	MenuLabel(float x, float y, wstring text);

	// Draws the label on the screen
	void Draw(SpriteBatch *spriteBatch, float scaleX, float scaleY);
};
