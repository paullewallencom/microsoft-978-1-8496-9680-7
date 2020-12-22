#pragma once
#include <string>
#include "SpriteBatch.h"
#include "SpriteFont.h"
#include "Constants.h"

using namespace std;
using namespace DirectX;

// The class representing the countdown feature
class Countdown
{
public:
	// Prepares the countdown feature by loading the font
	void Prepare(ID3D11Device *device);

	// Manages the logic of the countdown feature, e.g. by decreasing the counter value
	void Update(float time);

	// Draws the countdown on the screen
	void Draw(SpriteBatch *spriteBatch, wstring levelTranslation, float height, float width, int level, float scaleX, float scaleY);

	// Sets the time when the counter value has been modified for the last time
	void SetStartTime(float time) { m_startTime = time; }

	// Sets the counter value
	void SetCountdownNumber(int nr) { m_countdownNumber = nr; }

	// Returns the time when the counter value has been modified for the last time
	float GetStartTime() { return m_startTime; }

	// Returns the counter value
	int GetCountdownNumber() { return m_countdownNumber; }

private:
	// The font used to draw the text
	unique_ptr<SpriteFont> m_font;

	// The time when the counter value has been modified for the last time
	float m_startTime;

	// The current counter value
	int m_countdownNumber; 
};

