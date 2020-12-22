#include "pch.h"
#include "LocalizedStrings.h"

map<wstring, wstring> LocalizedStrings::m_translations;
vector<wstring> LocalizedStrings::m_languages;

void LocalizedStrings::Load(wstring language)
{
	PrepareLanguages(language);
	PrepareTranslations();
}

wstring LocalizedStrings::Get(wstring key)
{
	if (m_translations.count(key) == 1)
	{
		return m_translations[key];
	}
	else
	{
		return L"";
	}
}

void LocalizedStrings::PrepareLanguages(wstring language)
{
	m_languages.push_back(L"");
	UINT dashIndex = language.find(L"-");
	if (dashIndex != string::npos)
	{
		wstring main = language.substr(0, dashIndex);
		m_languages.push_back(main);
	}
	m_languages.push_back(language);
}

void LocalizedStrings::PrepareTranslations()
{
	wifstream file;
	locale utf8locale = locale(locale::empty(), new codecvt_utf8<wchar_t>);
	file.imbue(utf8locale);
	for (UINT i = 0; i < m_languages.size(); i++)
	{
		wstring part = m_languages[i];
		if (part.size() > 0) { part += L"."; }
		wstring filename = SA3D_LOCALIZED_STRINGS_PATH + part + SA3D_LOCALIZED_STRINGS_EXTENSION;
		file.open(filename);
		if (!file.fail())
		{
			wstring line;
			while (!file.eof())
			{
				getline(file, line);
				UINT semicolonIndex = line.find(L";");
				if (semicolonIndex != wstring::npos)
				{
					wstring key = line.substr(0, semicolonIndex);
					wstring value = line.substr(semicolonIndex + 1);
					m_translations[key] = value;
				}
			}
		}
		file.close();
	}
}
