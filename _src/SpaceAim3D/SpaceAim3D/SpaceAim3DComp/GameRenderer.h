#pragma once

#include "Direct3DBase.h"
#include "VertexData.h"
#include "MVPConstantBuffer.h"
#include "ModelLoader.h"
#include "Planet.h"
#include "Asteroid.h"
#include "Constants.h"
#include "Rocket.h"
#include "Game.h"
#include "CollisionDetector.h"
#include "RocketDisplay.h"
#include "SpriteBatch.h"
#include "Countdown.h"
#include "LocalizedStrings.h"
#include "Menu.h"
#include "External\XAudio2SoundPlayer.h"
#include "External\SoundFileReader.h"

using namespace Microsoft::WRL;
using namespace Windows::Devices::Sensors;
using namespace Windows::Phone::Devices::Notification;

ref class GameRenderer sealed : public Direct3DBase
{
public:
	// Initializes a new instance
	GameRenderer();

	// Create resources
	virtual void CreateDeviceResources() override;
	virtual void CreateWindowSizeDependentResources() override;

	// Render and update the content
	virtual void Render() override;
	void Update(float timeTotal, float timeDelta);

	// Handle touch input and pressing the back button
	UINT OnPointerPressed(float x, float y);
	UINT OnPointerReleased(float x, float y);
	UINT OnBackButtonPressed();

	// Returns the current game score
	int GetLastScore() { return m_game.GetScore(); }

	// Indicates whether vibrations are enabled
	void EnableVibrations(bool enabled) { m_vibrationsEnabled = enabled; }

private:
	// Updates the projection matrix
	void UpdateProjectionMatrix();

	// Loads the current level
	void LoadLevel();

	// Called when the rocket reaches the target planet
	// Loads the next level
	void ReachPlanet();

	// Called when the rocket crashes
	// Loads the same level, if the player has more rockets
	void Crash();

	// Checks whether the rocket reaches the planet or hits any asteroid
	void DetectCollisions();

	// Moves the rocket, based on values obtained from the accelerometer
	void MoveRocket();

	// Updates the view matrix
	void UpdateViewMatrix();

	// Updates data of the target planet and all asteroids
	void UpdatePlanetAndAsteroids(float timeTotal);

	// Renders the target planet and all asteroids
	void RenderPlanetAndAsteroids();

	// Prepares the Pause and Result menus
	void PrepareMenus();

	// Methods handling updating, depending on the current game state
	void UpdateStarting(float timeTotal, float timeDelta);
	void UpdatePlaying(float timeTotal, float timeDelta);

	// Methods handling rendering, depending on the current game state
	void RenderStarting();
	void RenderPlaying();
	void RenderPause();
	void RenderResult();

	// Methods handling touch input, depending on the current game state
	SA3D_ACTION OnPointerPressedStarting(float x, float y);
	SA3D_ACTION OnPointerPressedPlaying(float x, float y);
	SA3D_ACTION OnPointerReleasedStarting(float x, float y);
	SA3D_ACTION OnPointerReleasedPlaying(float x, float y);
	SA3D_ACTION OnPointerReleasedPause(float x, float y);
	SA3D_ACTION OnPointerReleasedResult(float x, float y);

	// Methods for loading and saving the current game state, as well as removing it
	void LoadGame();
	void SaveGame();
	void ClearSavedGame();

	// A value indicating whether the loading process is completed
	bool m_loadingComplete;

	// Projection and view matrices
	XMMATRIX m_projectionMatrix;
	XMMATRIX m_viewMatrix;

	// Input layout, shaders, and a constant buffer
	ComPtr<ID3D11InputLayout> m_inputLayout;
	ComPtr<ID3D11VertexShader> m_vertexShader;
	ComPtr<ID3D11PixelShader> m_pixelShader;
	ComPtr<ID3D11Buffer> m_constantBuffer;

	// Loaders of models representing the planet and the asteroid
	ModelLoader m_modelPlanet;
	ModelLoader m_modelAsteroid;

	// Objects representing the planet, all asteoids, and the rocket
	Planet m_planet;
	vector<unique_ptr<Asteroid>> m_asteroids;
	Rocket m_rocket;

	// Game data
	Game m_game;

	// Rocket display data
	RocketDisplay m_rocketDisplay;

	// Sprite batch used for drawing 2D graphics
	unique_ptr<SpriteBatch> m_sbDrawing;

	// Scale factors
	float m_scaleX;
	float m_scaleY;

	// Countdown data
	Countdown m_countdown;

	// Menus data
	Menu m_menuPause;
	Menu m_menuResult;

	// Fields representing the accelerometer and data read from it
	Accelerometer^ m_accelerometer;
	AccelerometerReading^ m_accelerometerReading;

	// A path to the file, where the game state is saved
	wstring m_saveFile;

	// A mechanism of playing sounds, as well as an identifier of the crash sound
	unique_ptr<XAudio2SoundPlayer> m_soundPlayer;
	int m_soundCrash;

	// A field representing the vibration device
	VibrationDevice^ m_vibration;

	// A method that starts vibrations
	void Vibrate(int ms);

	// A value indicating whether vibrations are currently enabled
	bool m_vibrationsEnabled;
};
