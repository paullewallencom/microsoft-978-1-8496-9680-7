#pragma once
#include <string>
#include <map>
#include <vector>
#include <fstream>
#include <codecvt>
#include "Constants.h"

using namespace std;

// The class representing a mechanism of translations
class LocalizedStrings
{
public:
	// Loads translations for the particular language
	static void Load(wstring language);

	// Returns a translation by a key
	static wstring Get(wstring key);

private:
	// Prepares available language codes, e.g. "", "pl", and "pl-PL"
	static void PrepareLanguages(wstring language);

	// Prepares all translations
	static void PrepareTranslations();

	// Fields storing data of translations and language codes
	static map<wstring, wstring> m_translations;
	static vector<wstring> m_languages;
};
