#pragma once
#include "pch.h"
#include "BasicTimer.h"
#include "GameRenderer.h"
#include <DrawingSurfaceNative.h>
#include "LocalizedStrings.h"

namespace SpaceAim3DComp
{
	// Delegates related to our game
	public delegate void ExitGameHandler();
	public delegate void SendResultHandler();
	public delegate void SaveResultHandler();

	// Automatically generated delegates
	public delegate void RequestAdditionalFrameHandler();
	public delegate void RecreateSynchronizedTextureHandler();

	[Windows::Foundation::Metadata::WebHostHidden]
	public ref class Direct3DInterop sealed : public Windows::Phone::Input::Interop::IDrawingSurfaceManipulationHandler
	{
	public:
		Direct3DInterop();

		Windows::Phone::Graphics::Interop::IDrawingSurfaceContentProvider^ CreateContentProvider();

		// IDrawingSurfaceManipulationHandler
		virtual void SetManipulationHost(Windows::Phone::Input::Interop::DrawingSurfaceManipulationHost^ manipulationHost);

		// Automatically genereated events
		event RequestAdditionalFrameHandler^ RequestAdditionalFrame;
		event RecreateSynchronizedTextureHandler^ RecreateSynchronizedTexture;

		// Events dedicated to our game
		event ExitGameHandler^ ExitGame;
		event SendResultHandler^ SendResult;
		event SaveResultHandler^ SaveResult;

		// The property that stores the last score
		property int LastScore;

		// The property that allows to set a language for the translation mechanism
		property Platform::String^ LanguageCode
		{
			void set(Platform::String^ code)
			{
				LocalizedStrings::Load(code->Data());
			}
		}

		// Automatically generated properties
		property Windows::Foundation::Size WindowBounds;
		property Windows::Foundation::Size NativeResolution;
		property Windows::Foundation::Size RenderResolution
		{
			Windows::Foundation::Size get(){ return m_renderResolution; }
			void set(Windows::Foundation::Size renderResolution);
		}

		// The method that indicates whether vibrations are enabled
		void EnableVibrations(bool enabled) { m_renderer->EnableVibrations(enabled); }

		// The method called when the back button is pressed
		void OnBackButtonPressed();

	protected:
		// Event Handlers
		void OnPointerPressed(Windows::Phone::Input::Interop::DrawingSurfaceManipulationHost^ sender, Windows::UI::Core::PointerEventArgs^ args);
		void OnPointerMoved(Windows::Phone::Input::Interop::DrawingSurfaceManipulationHost^ sender, Windows::UI::Core::PointerEventArgs^ args);
		void OnPointerReleased(Windows::Phone::Input::Interop::DrawingSurfaceManipulationHost^ sender, Windows::UI::Core::PointerEventArgs^ args);

	internal:
		HRESULT STDMETHODCALLTYPE Connect(_In_ IDrawingSurfaceRuntimeHostNative* host);
		void STDMETHODCALLTYPE Disconnect();
		HRESULT STDMETHODCALLTYPE PrepareResources(_In_ const LARGE_INTEGER* presentTargetTime, _Out_ BOOL* contentDirty);
		HRESULT STDMETHODCALLTYPE GetTexture(_In_ const DrawingSurfaceSizeF* size, _Inout_ IDrawingSurfaceSynchronizedTextureNative** synchronizedTexture, _Inout_ DrawingSurfaceRectF* textureSubRectangle);
		ID3D11Texture2D* GetTexture();

	private:
		GameRenderer^ m_renderer;
		BasicTimer^ m_timer;
		Windows::Foundation::Size m_renderResolution;
	};

}
