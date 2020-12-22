#include "pch.h"
#include "GameRenderer.h"

using namespace DirectX;
using namespace Microsoft::WRL;
using namespace Microsoft::WRL::Wrappers;
using namespace Windows::Foundation;
using namespace Windows::UI::Core;
using namespace Windows::System;

GameRenderer::GameRenderer() :
	m_loadingComplete(false),
	m_accelerometer(Accelerometer::GetDefault()),
	m_vibration(VibrationDevice::GetDefault())
{
	m_soundPlayer = unique_ptr<XAudio2SoundPlayer>(new XAudio2SoundPlayer(48000));
	m_saveFile = Windows::Storage::ApplicationData::Current->LocalFolder->Path->Data();
	m_saveFile.append(L"\\");
	m_saveFile.append(SA3D_SAVE_FILE);
}

void GameRenderer::CreateDeviceResources()
{
	Direct3DBase::CreateDeviceResources();

	// Load the game
	LoadGame();

	// Read sound effect for the rocket crash
	SoundFileReader soundFileReader(SA3D_CRASH_SOUND_FILE);
	m_soundCrash = m_soundPlayer->AddSound(soundFileReader.GetSoundFormat(), soundFileReader.GetSoundData());

	// Load the planet model
	m_modelPlanet.SetColors(Planet::GetColors());
	m_modelPlanet.Load(SA3D_PLANET_MODEL_FILE, m_d3dDevice);

	// Load the asteroid model
	m_modelAsteroid.SetColors(Asteroid::GetColors());
	m_modelAsteroid.Load(SA3D_ASTEROID_MODEL_FILE, m_d3dDevice);

	// Load the first level
	LoadLevel();

	// Load shaders
	auto loadVSTask = DX::ReadDataAsync("SimpleVertexShader.cso");
	auto loadPSTask = DX::ReadDataAsync("SimplePixelShader.cso");

	auto createVSTask = loadVSTask.then([this](Platform::Array<byte>^ fileData) {
		DX::ThrowIfFailed(m_d3dDevice->CreateVertexShader(fileData->Data, fileData->Length, nullptr, &m_vertexShader));
		const D3D11_INPUT_ELEMENT_DESC vertexDesc[] = 
		{
			{ "POSITION", 0, DXGI_FORMAT_R32G32B32_FLOAT, 0, 0,  D3D11_INPUT_PER_VERTEX_DATA, 0 },
			{ "COLOR",    0, DXGI_FORMAT_R32G32B32_FLOAT, 0, 12, D3D11_INPUT_PER_VERTEX_DATA, 0 },
		};
		DX::ThrowIfFailed(m_d3dDevice->CreateInputLayout(vertexDesc, ARRAYSIZE(vertexDesc), fileData->Data, fileData->Length, &m_inputLayout));
	});

	auto createPSTask = loadPSTask.then([this](Platform::Array<byte>^ fileData) {
		DX::ThrowIfFailed(m_d3dDevice->CreatePixelShader(fileData->Data, fileData->Length, nullptr, &m_pixelShader));
		CD3D11_BUFFER_DESC constantBufferDesc(sizeof(MVPConstantBuffer), D3D11_BIND_CONSTANT_BUFFER);
		DX::ThrowIfFailed(m_d3dDevice->CreateBuffer(&constantBufferDesc, nullptr, &m_constantBuffer));
	});

	auto completeTask = (createPSTask && createVSTask).then([this] () { m_loadingComplete = true; });
}

void GameRenderer::CreateWindowSizeDependentResources()
{
	Direct3DBase::CreateWindowSizeDependentResources();

	// Prepare the rocket display, the countdown feature, and the menu mechanism
	m_rocketDisplay.Prepare(m_d3dDevice.Get());
	m_countdown.Prepare(m_d3dDevice.Get());
	Menu::Prepare(m_d3dDevice.Get());

	// Prepare the sprite batch
	m_sbDrawing = unique_ptr<SpriteBatch>(new SpriteBatch(m_d3dContext.Get()));

	// Calculate scale factors
	m_scaleX = m_renderTargetSize.Width / SA3D_MAX_SCREEN_WIDTH;
	m_scaleY = m_renderTargetSize.Height / SA3D_MAX_SCREEN_HEIGHT;

	// Update the projection matrix
	UpdateProjectionMatrix();
}

void GameRenderer::Update(float timeTotal, float timeDelta)
{
	switch (m_game.GetState())
	{
	case SA3D_STATE_STARTING:
		UpdateStarting(timeTotal, timeDelta); break;
	case SA3D_STATE_PLAYING:
		UpdatePlaying(timeTotal, timeDelta); break;
	}
}

void GameRenderer::UpdateStarting(float timeTotal, float timeDelta)
{
	UpdatePlaying(timeTotal, timeDelta);
	m_countdown.Update(timeTotal);
	if (m_countdown.GetCountdownNumber() == 0)
	{
		m_game.SetState(SA3D_STATE_PLAYING);
	}
}

void GameRenderer::UpdatePlaying(float timeTotal, float timeDelta)
{
	MoveRocket();
	DetectCollisions();

	float distance = m_rocket.Fly(timeDelta * 1.5f * m_game.GetSpeedFactor());
	m_game.IncreaseScore(distance);

	UpdateViewMatrix();
	UpdatePlanetAndAsteroids(timeTotal);
}

void GameRenderer::Render()
{
	const float backgroundColor[] = { 0.0f, 0.0f, 0.0f, 1.0f };
	m_d3dContext->OMSetRenderTargets(1, m_renderTargetView.GetAddressOf(), m_depthStencilView.Get());
	m_d3dContext->ClearRenderTargetView(m_renderTargetView.Get(), backgroundColor);
	m_d3dContext->ClearDepthStencilView(m_depthStencilView.Get(), D3D11_CLEAR_DEPTH, 1.0f, 0);
	if (!m_loadingComplete) 
	{ 
		return; 
	}

	m_sbDrawing->Begin();
	switch (m_game.GetState())
	{
	case SA3D_STATE_STARTING: RenderStarting(); break;
	case SA3D_STATE_PLAYING: RenderPlaying(); break;
	case SA3D_STATE_PAUSE: RenderPause(); break;
	case SA3D_STATE_RESULT: RenderResult(); break;
	}
	m_sbDrawing->End();
}

void GameRenderer::RenderStarting()
{
	RenderPlaying();
	wstring levelTranslation = LocalizedStrings::Get(L"Level");
	m_countdown.Draw(
		m_sbDrawing.get(), 
		levelTranslation, 
		m_renderTargetSize.Height, 
		m_renderTargetSize.Width, 
		m_game.GetLevel(), 
		m_scaleX, 
		m_scaleY);
}

void GameRenderer::RenderPlaying()
{
	RenderPlanetAndAsteroids();
	m_rocketDisplay.Draw(
		m_sbDrawing.get(), 
		m_renderTargetSize.Height, 
		m_renderTargetSize.Width, 
		m_game.GetLevel(), 
		m_game.GetScore(), 
		m_game.GetRocketsNumber(), 
		m_scaleX, 
		m_scaleY);
}

void GameRenderer::RenderPause() 
{
	m_menuPause.Draw(m_sbDrawing.get(), m_scaleX, m_scaleY);
}

void GameRenderer::RenderResult() 
{
	m_menuResult.Draw(m_sbDrawing.get(), m_scaleX, m_scaleY);
}

UINT GameRenderer::OnPointerPressed(float x, float y)
{
	switch (m_game.GetState())
	{
	case SA3D_STATE_STARTING:
		return OnPointerPressedStarting(x, y);
	case SA3D_STATE_PLAYING:
		return OnPointerPressedPlaying(x, y);
	default:
		return SA3D_ACTION_NONE;
	}
}

SA3D_ACTION GameRenderer::OnPointerPressedStarting(float x, float y)
{
	m_rocket.StartEngine();
	return SA3D_ACTION_NONE;
}

SA3D_ACTION GameRenderer::OnPointerPressedPlaying(float x, float y)
{
	m_rocket.StartEngine();
	return SA3D_ACTION_NONE;
}

UINT GameRenderer::OnPointerReleased(float x, float y)
{
	switch (m_game.GetState())
	{
	case SA3D_STATE_STARTING:
		return OnPointerReleasedStarting(x, y);
	case SA3D_STATE_PLAYING:
		return OnPointerReleasedPlaying(x, y);
	case SA3D_STATE_PAUSE:
		return OnPointerReleasedPause(x, y);
	case SA3D_STATE_RESULT:
		return OnPointerReleasedResult(x, y);
	}
	return SA3D_ACTION_NONE;
}

SA3D_ACTION GameRenderer::OnPointerReleasedStarting(float x, float y)
{
	m_rocket.StopEngine();
	return SA3D_ACTION_NONE;
}

SA3D_ACTION GameRenderer::OnPointerReleasedPlaying(float x, float y)
{
	m_rocket.StopEngine();
	return SA3D_ACTION_NONE;
}

SA3D_ACTION GameRenderer::OnPointerReleasedPause(float x, float y)
{ 
	switch (m_menuPause.OnPress(x, y, m_scaleX, m_scaleY))
	{
	case SA3D_MENU_PAUSE_RESTART:
		m_game.Restart();
		LoadLevel();
		return SA3D_ACTION_RESTART;
	case SA3D_MENU_PAUSE_RESUME:
		m_game.SetState(SA3D_STATE_PLAYING);
		break;
	case SA3D_MENU_PAUSE_BACK:
		ClearSavedGame();
		return SA3D_ACTION_BACK_TO_MENU;		
	}

	return SA3D_ACTION_NONE;
}

SA3D_ACTION GameRenderer::OnPointerReleasedResult(float x, float y)
{ 
	switch (m_menuResult.OnPress(x, y, m_scaleX, m_scaleY))
	{
	case SA3D_MENU_RESULT_RESTART:
		m_game.Restart();
		LoadLevel();
		return SA3D_ACTION_RESTART;
	case SA3D_MENU_RESULT_SEND:
		return SA3D_ACTION_SEND_RESULT;
	case SA3D_MENU_RESULT_BACK:
		return SA3D_ACTION_BACK_TO_MENU; 
	}

	return SA3D_ACTION_NONE;
}

UINT GameRenderer::OnBackButtonPressed()
{
	switch (m_game.GetState())
	{
	case SA3D_STATE_STARTING:
	case SA3D_STATE_PLAYING:
		PrepareMenus();
		m_game.SetState(SA3D_STATE_PAUSE);
		break;
	case SA3D_STATE_PAUSE:
		m_game.SetState(SA3D_STATE_PLAYING);
		break;
	case SA3D_STATE_RESULT:
		return SA3D_ACTION_BACK_TO_MENU;
	}

	return SA3D_ACTION_NONE;
}

void GameRenderer::UpdateProjectionMatrix()
{
	// Calculate the aspect ratio and the field of view
	float aspectRatio = m_windowBounds.Width / m_windowBounds.Height;
	float fovAngleY = 70.0f * XM_PI / 180.0f;
	if (aspectRatio < 1.0f) 
	{ 
		fovAngleY /= aspectRatio; 
	}

	// Calculate the projection matrix
	m_projectionMatrix = XMMatrixTranspose(XMMatrixPerspectiveFovRH(fovAngleY, aspectRatio, 0.01f, 500.0f));
	for_each(m_asteroids.begin(), m_asteroids.end(), [&](unique_ptr<Asteroid>& a) { a->UpdateProjectionMatrix(&m_projectionMatrix); });
	m_planet.UpdateProjectionMatrix(&m_projectionMatrix);
}

void GameRenderer::LoadLevel()
{
	SaveGame();

	m_planet.Initialize(m_game.GetDistanceToPlanet());
	m_planet.SetBuffers(m_modelPlanet.GetVertexBuffer(), m_modelPlanet.GetIndexBuffer(), m_modelPlanet.GetIndicesCount());
	m_asteroids.clear();

	for (int z = 0; z < m_game.GetDistanceToPlanet(); z++)
	{
		for (int i = 0; i < m_game.GetAsteroidsPerLayer(); i++)
		{
			unique_ptr<Asteroid> asteroid(new Asteroid());
			asteroid->Initialize(z);
			asteroid->SetBuffers(m_modelAsteroid.GetVertexBuffer(), m_modelAsteroid.GetIndexBuffer(), m_modelAsteroid.GetIndicesCount());
			m_asteroids.push_back(move(asteroid));
		}
	}
	reverse(m_asteroids.begin(), m_asteroids.end());

	m_rocket.Initialize();
	UpdateProjectionMatrix();
	m_game.SetState(SA3D_STATE_STARTING);

	m_countdown.SetCountdownNumber(SA3D_COUNTDOWN_NUMBER);
	m_countdown.SetStartTime(0);
}

void GameRenderer::DetectCollisions()
{
	// Check whether the planet is reached
	bool isPlanetHit = CollisionDetector::IsHit(m_rocket.GetLocationSimple(), m_planet.GetLocation(), m_planet.GetScale());
	if (isPlanetHit) 
	{ 
		ReachPlanet(); 
	}

	// Check whether any asteroid is hit
	bool isAnyAsteroidHit = false;
	for (auto a = m_asteroids.begin(); a != m_asteroids.end(); ++a)
	{
		bool isAsteroidHit = CollisionDetector::IsHit(m_rocket.GetLocationSimple(), a->get()->GetLocation(), a->get()->GetScale());
		if (isAsteroidHit) 
		{ 
			isAnyAsteroidHit = true;
			break; 
		}
	}

	// Check whether the rocket missed the planet
	bool missPlanet = m_rocket.GetZ() < -m_game.GetDistanceToPlanet() - 5;
	if (isAnyAsteroidHit || missPlanet)
	{ 
		Crash(); 
	}
}

void GameRenderer::ReachPlanet()
{
	Vibrate(500);
	m_game.ReachPlanet();
	LoadLevel();
}

void GameRenderer::Crash()
{
	Vibrate(1000);
	m_soundPlayer->PlaySound(m_soundCrash);
	m_game.CrashWithAsteroid();

	if (m_game.GetRocketsNumber() > 0) 
	{ 
		LoadLevel(); 
	}
	else 
	{
		PrepareMenus();
		m_game.SetState(SA3D_STATE_RESULT);
		ClearSavedGame();
	}
}

void GameRenderer::MoveRocket()
{
	if (m_accelerometer != nullptr)
	{
		m_accelerometerReading = m_accelerometer->GetCurrentReading();
		m_rocket.Move(
			(float)-m_accelerometerReading->AccelerationY,
			(float)m_accelerometerReading->AccelerationX);
	}
}

void GameRenderer::UpdateViewMatrix()
{
	XMVECTOR eye = m_rocket.GetLocation();
	XMVECTOR direction = XMVectorSet(0.0, 0.0, -1.0f, 0.0f);
	XMVECTOR up = XMVectorSet(0.0f, 1.0f, 0.0f, 0.0f);
	m_viewMatrix = XMMatrixTranspose(XMMatrixLookToRH(eye, direction, up));
}

void GameRenderer::UpdatePlanetAndAsteroids(float timeTotal)
{
	m_planet.Update(&m_viewMatrix, timeTotal);
	float rocketZ = m_rocket.GetZ();
	float asteroidZ = 0.0f;
	for_each(m_asteroids.begin(), m_asteroids.end(), [&](unique_ptr<Asteroid>& a) 
	{
		asteroidZ = a->GetZ();
		if (asteroidZ < rocketZ && abs(asteroidZ - rocketZ) <= SA3D_ASTEROIDS_DISTANCE_FILTER)
		{ 
			a->Update(&m_viewMatrix, timeTotal); 
		}
	});
}

void GameRenderer::RenderPlanetAndAsteroids()
{
	UINT stride = sizeof(VertexData);
	UINT offset = 0;
	m_d3dContext->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST);
	m_d3dContext->IASetInputLayout(m_inputLayout.Get());
	m_d3dContext->VSSetShader(m_vertexShader.Get(), nullptr, 0);
	m_d3dContext->PSSetShader(m_pixelShader.Get(), nullptr, 0);
	m_d3dContext->VSSetConstantBuffers(0, 1, m_constantBuffer.GetAddressOf());

	// Render the planet
	m_d3dContext->IASetVertexBuffers(0, 1, m_modelPlanet.GetVertexBuffer().GetAddressOf(), &stride, &offset);
	m_d3dContext->IASetIndexBuffer(m_modelPlanet.GetIndexBuffer().Get(), DXGI_FORMAT_R16_UINT, 0);
	m_planet.Render(m_d3dContext, m_constantBuffer);

	// Render all asteroids
	m_d3dContext->IASetVertexBuffers(0, 1, m_modelAsteroid.GetVertexBuffer().GetAddressOf(), &stride, &offset);
	m_d3dContext->IASetIndexBuffer(m_modelAsteroid.GetIndexBuffer().Get(), DXGI_FORMAT_R16_UINT, 0);
	float rocketZ = m_rocket.GetZ();
	float asteroidZ = 0.0f;
	for_each(m_asteroids.begin(), m_asteroids.end(), [&](unique_ptr<Asteroid>& a)
	{
		asteroidZ = a->GetZ();
		if (asteroidZ < rocketZ && abs(asteroidZ - rocketZ) <= SA3D_ASTEROIDS_DISTANCE_FILTER)
		{ 
			a->Render(m_d3dContext, m_constantBuffer); 
		}
	});
}

void GameRenderer::PrepareMenus()
{
	wstring scoreText = LocalizedStrings::Get(L"Score") + L": " + to_wstring(m_game.GetScore());
	wstring levelText = LocalizedStrings::Get(L"Level") + L": " + to_wstring(m_game.GetLevel());
	wstring rocketsText = LocalizedStrings::Get(L"Rockets") + L": " + to_wstring(m_game.GetRocketsNumber());
	wstring noRocketsText = LocalizedStrings::Get(L"OutOfRockets");
	wstring resumeText = LocalizedStrings::Get(L"Resume");
	wstring sendText = LocalizedStrings::Get(L"SendResult");
	wstring restartText = LocalizedStrings::Get(L"Restart");
	wstring backText = LocalizedStrings::Get(L"Back");

	// Create the Pause menu
	m_menuPause.Clear();
	m_menuPause.AddLabel(100, 50,  scoreText);
	m_menuPause.AddLabel(100, 130, levelText);
	m_menuPause.AddLabel(100, 210, rocketsText);
	m_menuPause.AddButton(100, 350, 500, 350, resumeText, SA3D_MENU_PAUSE_RESUME);
	m_menuPause.AddButton(650, 350, 550, 150, restartText, SA3D_MENU_PAUSE_RESTART);
	m_menuPause.AddButton(650, 550, 550, 150, backText, SA3D_MENU_PAUSE_BACK);

	// Create the Result menu
	m_menuResult.Clear();
	m_menuResult.AddLabel(100, 50, scoreText);
	m_menuResult.AddLabel(100, 130, levelText);
	m_menuResult.AddLabel(100, 210, noRocketsText);
	m_menuResult.AddButton(100, 350, 500, 350, sendText, SA3D_MENU_RESULT_SEND);
	m_menuResult.AddButton(650, 350, 550, 150, restartText, SA3D_MENU_RESULT_RESTART);
	m_menuResult.AddButton(650, 550, 550, 150, backText, SA3D_MENU_RESULT_BACK);
}

void GameRenderer::SaveGame()
{
	FileHandle file(CreateFile2(m_saveFile.c_str(), GENERIC_WRITE, FILE_SHARE_WRITE, CREATE_ALWAYS, NULL));
	if (file.Get() != INVALID_HANDLE_VALUE)
	{
		char data[SA3D_SAVE_FILE_SIZE];
		sprintf_s(
			data, 
			SA3D_SAVE_FILE_SIZE, 
			"%04i;%09i;%04i", 
			m_game.GetLevel(), 
			m_game.GetScore(), 
			m_game.GetRocketsNumber());
		WriteFile(file.Get(), data, ARRAYSIZE(data), nullptr, nullptr);
	}
}

void GameRenderer::LoadGame()
{
	FileHandle file(CreateFile2(m_saveFile.c_str(), GENERIC_READ, FILE_SHARE_READ, OPEN_EXISTING, NULL));
	if (file.Get() != INVALID_HANDLE_VALUE)
	{
		char data[SA3D_SAVE_FILE_SIZE];
		DWORD readBytes;
		if (ReadFile(file.Get(), data, ARRAYSIZE(data), &readBytes, nullptr) && readBytes == SA3D_SAVE_FILE_SIZE)
		{
			string text(data);
			string levelText = text.substr(0, 4);
			string scoreText = text.substr(5, 9);
			string rocketsText = text.substr(15, 4);
			int level = atoi(levelText.c_str());
			int score = atoi(scoreText.c_str());
			int rockets = atoi(rocketsText.c_str());
			m_game.SetLevel(level);
			m_game.SetScore(score);
			m_game.SetRocketsNumber(rockets);
		}
	}
}

void GameRenderer::ClearSavedGame()
{
	FileHandle file(CreateFile2(m_saveFile.c_str(), GENERIC_WRITE, FILE_SHARE_WRITE, CREATE_ALWAYS, NULL));
	if (file.Get() != INVALID_HANDLE_VALUE)
	{
		char data[1] = { 0 };
		WriteFile(file.Get(), data, ARRAYSIZE(data), nullptr, nullptr);
	}
}

void GameRenderer::Vibrate(int ms)
{
	if (m_vibrationsEnabled)
	{
		TimeSpan vibrationPeriod;
		vibrationPeriod.Duration = ms * 10000;
		m_vibration->Vibrate(vibrationPeriod);
	}
}
