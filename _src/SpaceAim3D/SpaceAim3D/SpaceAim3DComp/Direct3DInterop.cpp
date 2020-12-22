#include "pch.h"
#include "Direct3DInterop.h"
#include "Direct3DContentProvider.h"

using namespace Windows::Foundation;
using namespace Windows::UI::Core;
using namespace Microsoft::WRL;
using namespace Windows::Phone::Graphics::Interop;
using namespace Windows::Phone::Input::Interop;

namespace SpaceAim3DComp
{
	Direct3DInterop::Direct3DInterop() :
		m_timer(ref new BasicTimer())
	{
	}

	IDrawingSurfaceContentProvider^ Direct3DInterop::CreateContentProvider()
	{
		ComPtr<Direct3DContentProvider> provider = Make<Direct3DContentProvider>(this);
		return reinterpret_cast<IDrawingSurfaceContentProvider^>(provider.Get());
	}

	// IDrawingSurfaceManipulationHandler
	void Direct3DInterop::SetManipulationHost(DrawingSurfaceManipulationHost^ manipulationHost)
	{
		manipulationHost->PointerPressed +=
			ref new TypedEventHandler<DrawingSurfaceManipulationHost^, PointerEventArgs^>(this, &Direct3DInterop::OnPointerPressed);

		manipulationHost->PointerMoved +=
			ref new TypedEventHandler<DrawingSurfaceManipulationHost^, PointerEventArgs^>(this, &Direct3DInterop::OnPointerMoved);

		manipulationHost->PointerReleased +=
			ref new TypedEventHandler<DrawingSurfaceManipulationHost^, PointerEventArgs^>(this, &Direct3DInterop::OnPointerReleased);
	}

	void Direct3DInterop::RenderResolution::set(Windows::Foundation::Size renderResolution)
	{
		if (renderResolution.Width  != m_renderResolution.Width ||
			renderResolution.Height != m_renderResolution.Height)
		{
			m_renderResolution = renderResolution;

			if (m_renderer)
			{
				m_renderer->UpdateForRenderResolutionChange(m_renderResolution.Width, m_renderResolution.Height);
				RecreateSynchronizedTexture();
			}
		}
	}

	// Event Handlers
	void Direct3DInterop::OnPointerPressed(DrawingSurfaceManipulationHost^ sender, PointerEventArgs^ args)
	{
		m_renderer->OnPointerPressed(args->CurrentPoint->Position.X, args->CurrentPoint->Position.Y);
	}

	void Direct3DInterop::OnPointerMoved(DrawingSurfaceManipulationHost^ sender, PointerEventArgs^ args)
	{
	}

	void Direct3DInterop::OnPointerReleased(DrawingSurfaceManipulationHost^ sender, PointerEventArgs^ args)
	{
		LastScore = m_renderer->GetLastScore();

		// Get the last action and perform additional operations (like sending the result), if necessary
		SA3D_ACTION action = (SA3D_ACTION)m_renderer->OnPointerReleased(args->CurrentPoint->Position.X, args->CurrentPoint->Position.Y);
		switch (action)
		{
		case SA3D_ACTION_BACK_TO_MENU:
			ExitGame();
			break;

		case SA3D_ACTION_SEND_RESULT:
			SendResult();
			ExitGame();
			break;
		}

		// Save the result in the local rank
		if (action != SA3D_ACTION_NONE && LastScore > 0)
		{
			SaveResult();
		}
	}

	void Direct3DInterop::OnBackButtonPressed()
	{
		// Get the last action and check whether we want to exit to the main menu (in the managed part)
		SA3D_ACTION action = (SA3D_ACTION)m_renderer->OnBackButtonPressed();
		if (action == SA3D_ACTION_BACK_TO_MENU) 
		{ 
			ExitGame(); 
		}
	}

	// Interface With Direct3DContentProvider
	HRESULT Direct3DInterop::Connect(_In_ IDrawingSurfaceRuntimeHostNative* host)
	{
		m_renderer = ref new GameRenderer();
		m_renderer->Initialize();
		m_renderer->UpdateForWindowSizeChange(WindowBounds.Width, WindowBounds.Height);
		m_renderer->UpdateForRenderResolutionChange(m_renderResolution.Width, m_renderResolution.Height);

		// Restart timer after renderer has finished initializing.
		m_timer->Reset();

		return S_OK;
	}

	void Direct3DInterop::Disconnect()
	{
		m_renderer = nullptr;
	}

	HRESULT Direct3DInterop::PrepareResources(_In_ const LARGE_INTEGER* presentTargetTime, _Out_ BOOL* contentDirty)
	{
		*contentDirty = true;
		return S_OK;
	}

	HRESULT Direct3DInterop::GetTexture(_In_ const DrawingSurfaceSizeF* size, _Inout_ IDrawingSurfaceSynchronizedTextureNative** synchronizedTexture, _Inout_ DrawingSurfaceRectF* textureSubRectangle)
	{
		m_timer->Update();
		m_renderer->Update(m_timer->Total, m_timer->Delta);
		m_renderer->Render();

		RequestAdditionalFrame();

		return S_OK;
	}

	ID3D11Texture2D* Direct3DInterop::GetTexture()
	{
		return m_renderer->GetTexture();
	}
}
