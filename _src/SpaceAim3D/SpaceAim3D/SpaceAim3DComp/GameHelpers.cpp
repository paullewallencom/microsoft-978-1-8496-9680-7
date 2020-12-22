#include "pch.h"
#include "GameHelpers.h"

int GameHelpers::RandInt(int max)
{
	return rand() % max; 
}

float GameHelpers::RandFloat()
{ 
	return (rand()%1000) / 1000.0f; 
}

float GameHelpers::RandFloat(float minValue, float maxValue)
{ 
	return minValue + (maxValue - minValue) * GameHelpers::RandFloat(); 
}

XMFLOAT3 GameHelpers::GetColor(byte r, byte g, byte b)
{ 
	return XMFLOAT3(r/255.0f, g/255.0f, b/255.0f); 
}

void GameHelpers::Debug(float x, float y, float z)
{
	wstring text = to_wstring(x) + L" " + to_wstring(y) + L" " + to_wstring(z) + L'\n';
	OutputDebugString(text.c_str());
}

void GameHelpers::Debug(float value)
{
	wstring text = to_wstring(value) + L'\n';
	OutputDebugString(text.c_str());
}

