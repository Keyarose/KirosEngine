using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.Windows;
using SlimDX.D3DCompiler;
using SlimDX.Direct3D11;

using Device = SlimDX.Direct3D11.Device;
using Resource = SlimDX.Direct3D11.Resource;

namespace KirosEngine
{
    /// <summary>
    /// Handles the setup and initialization of Direct 3D
    /// </summary>
    public class D3DCore : IDisposable
    {
        private Device _device;
        private RenderTargetView _renderTarget;
        private DeviceContext _context;
        private SwapChain _swapChain;
        private Texture2D _depthStencilBuffer;
        private DepthStencilState _depthStencilState;
        private DepthStencilView _depthStencilView;
        private RasterizerState _rasterizerState;

        private Matrix _projectionMatrix;
        private Matrix _worldMatrix;
        private Matrix _orthoMatrix;

        private DepthStencilState _depthDisabledStencilState;
        private BlendState _alphaEnabledBlendState;
        private BlendState _alphaDisabledBlendState;

        int _videoCardMemory;

        /// <summary>
        /// Accessor for the direct 3d device
        /// </summary>
        /// <returns>The Direct 3d Device</returns>
        public Device GetDevice()
        {
            return _device;
        }

        /// <summary>
        /// Accessor for the current render target
        /// </summary>
        /// <returns>The current RenderTargetView</returns>
        public RenderTargetView GetRenderTarget()
        {
            return _renderTarget;
        }

        /// <summary>
        /// Accessor for the current swap chain
        /// </summary>
        /// <returns>The current SwapChain</returns>
        public SwapChain GetSwapChain()
        {
            return _swapChain;
        }

        /// <summary>
        /// Accessor for the current projection matrix
        /// </summary>
        /// <returns>The Matrix containing the Projection values</returns>
        public Matrix GetProjectionMatrix()
        {
            return _projectionMatrix;
        }

        /// <summary>
        /// Accessor for the current world matrix
        /// </summary>
        /// <returns>The Matrix containing the World values</returns>
        public Matrix GetWorldMatrix()
        {
            return _worldMatrix;
        }

        /// <summary>
        /// Accessor for the current orthographic matirx
        /// </summary>
        /// <returns>The Matrix containing the Othographic values</returns>
        public Matrix GetOrthoMatrix()
        {
            return _orthoMatrix;
        }

        /// <summary>
        /// Begin drawing the scene starting with the given clear color
        /// </summary>
        /// <param name="clearColor">The color to be used as the clear color</param>
        public void BeginScene(Color4 clearColor)
        {
            _context.ClearRenderTargetView(_renderTarget, clearColor);
            _context.ClearDepthStencilView(_depthStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);
        }

        /// <summary>
        /// Finish drawing the scene and present it
        /// </summary>
        public void EndScene()
        {
            _swapChain.Present(0, PresentFlags.None);
        }

        /// <summary>
        /// Initialize the Direct 3D core
        /// </summary>
        /// <param name="windowForm">The RenderForm to be used</param>
        /// <param name="fullScreen">Run in full screen or not</param>
        /// <param name="screenNear">The near screen value</param>
        /// <param name="screenDepth">The far screen value</param>
        /// <returns>True if initialization was successful</returns>
        public bool Initialize(RenderForm windowForm, bool fullScreen, float screenNear, float screenDepth)
        {
            int numerator = 60, denominator = 1;
            float fieldOfView, screenAspect;
            int screenHeight = windowForm.Height;
            int screenWidth = windowForm.Width;

            Factory factory = new Factory();
            Adapter adapter = factory.GetAdapter(0);
            Output adapterOutput = adapter.GetOutput(0);
            ReadOnlyCollection<ModeDescription> numModes = adapterOutput.GetDisplayModeList(Format.R8G8B8A8_UNorm, DisplayModeEnumerationFlags.Interlaced);

            foreach (ModeDescription desc in numModes)
            {
                if (desc.Height == screenHeight && desc.Width == screenWidth)
                {
                    numerator = desc.RefreshRate.Numerator;
                    denominator = desc.RefreshRate.Denominator;
                }
            }

            //get the video card's information
            AdapterDescription adapterDesc = adapter.Description;
            _videoCardMemory = (int)(adapterDesc.DedicatedVideoMemory / 1024 / 1024);
            string videoCardDesc = adapterDesc.Description;

            //dispose of objects we're finished with
            adapterOutput.Dispose();
            adapter.Dispose();
            factory.Dispose();

            //swapchain description
            var discription = new SwapChainDescription()
            {
                BufferCount = 1,
                Usage = Usage.RenderTargetOutput,
                OutputHandle = windowForm.Handle,
                IsWindowed = !fullScreen,
                ModeDescription = new ModeDescription(screenWidth, screenHeight, new Rational(numerator, denominator), Format.B8G8R8A8_UNorm),
                SampleDescription = new SampleDescription(1, 0),
                Flags = SwapChainFlags.AllowModeSwitch,
                SwapEffect = SwapEffect.Discard
            };

            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, discription, out _device, out _swapChain);
            _context = _device.ImmediateContext;

            using (var resource = Resource.FromSwapChain<Texture2D>(_swapChain, 0))
            {
                _renderTarget = new RenderTargetView(_device, resource);
            }

            //buffer discription for the back buffer
            var depthDisc = new Texture2DDescription()
            {
                Height = screenHeight,
                Width = screenWidth,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.D24_UNorm_S8_UInt,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };

            _depthStencilBuffer = new Texture2D(_device, depthDisc);

            var depthStencilDisc = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,

                IsStencilEnabled = true,
                StencilReadMask = 0xff,
                StencilWriteMask = 0xff,

                FrontFace = new DepthStencilOperationDescription()
                {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Increment,
                    PassOperation = StencilOperation.Keep,
                    Comparison = Comparison.Always
                },

                BackFace = new DepthStencilOperationDescription()
                {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Decrement,
                    PassOperation = StencilOperation.Keep,
                    Comparison = Comparison.Always
                }
            };

            //create and set the depth stencil state
            _depthStencilState = DepthStencilState.FromDescription(_device, depthStencilDisc);
            _context.OutputMerger.DepthStencilState = _depthStencilState;

            var depthStencilViewDisc = new DepthStencilViewDescription()
            {
                Format = Format.D24_UNorm_S8_UInt,
                Dimension = DepthStencilViewDimension.Texture2D,
                MipSlice = 0
            };

            //create the depth stencil view and set the render target
            _depthStencilView = new DepthStencilView(_device, _depthStencilBuffer, depthStencilViewDisc);
            _context.OutputMerger.SetTargets(_depthStencilView, _renderTarget);

            //discription for the rasterizer
            var rasterDisc = new RasterizerStateDescription()
            {
                IsAntialiasedLineEnabled = false,
                CullMode = CullMode.Back,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                IsDepthClipEnabled = true,
                FillMode = FillMode.Solid,
                IsFrontCounterclockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f
            };

            //create the rasterizer state and set it
            _rasterizerState = RasterizerState.FromDescription(_device, rasterDisc);
            _context.Rasterizer.State = _rasterizerState;

            var viewport = new Viewport()
            {
                Width = screenWidth,
                Height = screenHeight,
                MinZ = 0.0f,
                MaxZ = 1.0f,
                X = 0.0f,
                Y = 0.0f
            };

            _context.Rasterizer.SetViewports(viewport);

            // Setup the projection matrix.
            fieldOfView = (float)Math.PI / 4.0f;
            screenAspect = (float)screenWidth / (float)screenHeight;

            // Create the projection matrix for 3D rendering.
            _projectionMatrix = Matrix.PerspectiveFovLH(fieldOfView, screenAspect, screenNear, screenDepth);
            _worldMatrix = Matrix.Identity;
            _orthoMatrix = Matrix.OrthoLH((float)screenWidth, (float)screenHeight, screenNear, screenDepth);

            var depthDisabledStencilDesc = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,
                IsStencilEnabled = true,
                StencilReadMask = 0xFF,
                StencilWriteMask = 0xFF,

                FrontFace = new DepthStencilOperationDescription()
                {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Increment,
                    PassOperation = StencilOperation.Keep,
                    Comparison = Comparison.Always
                },

                BackFace = new DepthStencilOperationDescription()
                {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Decrement,
                    PassOperation = StencilOperation.Keep,
                    Comparison = Comparison.Always
                }
            };

            _depthDisabledStencilState = DepthStencilState.FromDescription(_device, depthDisabledStencilDesc);

            var blendStateDesc = new BlendStateDescription();
            blendStateDesc.RenderTargets[0].BlendEnable = true;

            blendStateDesc.RenderTargets[0].SourceBlend = BlendOption.One;
            blendStateDesc.RenderTargets[0].DestinationBlend = BlendOption.InverseSourceAlpha;
            blendStateDesc.RenderTargets[0].BlendOperation = BlendOperation.Add;

            blendStateDesc.RenderTargets[0].SourceBlendAlpha = BlendOption.One;
            blendStateDesc.RenderTargets[0].DestinationBlendAlpha = BlendOption.Zero;
            blendStateDesc.RenderTargets[0].BlendOperationAlpha = BlendOperation.Add;

            blendStateDesc.RenderTargets[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;

            _alphaEnabledBlendState = BlendState.FromDescription(_device, blendStateDesc);

            blendStateDesc.RenderTargets[0].BlendEnable = false;

            _alphaDisabledBlendState = BlendState.FromDescription(_device, blendStateDesc);

            //keep the form from using the default windows fullsizing
            using (var factory1 = _swapChain.GetParent<Factory>())
            {
                factory1.SetWindowAssociation(windowForm.Handle, WindowAssociationFlags.IgnoreAltEnter);
            }

            //Allow fullsizing
            windowForm.KeyDown += (o, e) =>
            {
                if (e.Alt && e.KeyCode == Keys.Enter)
                    _swapChain.IsFullScreen = !_swapChain.IsFullScreen;
            };

            //Handle screen resizing
            windowForm.UserResized += (o, e) =>
            {
                _renderTarget.Dispose();

                _swapChain.ResizeBuffers(2, 0, 0, Format.R8G8B8A8_UNorm, SwapChainFlags.AllowModeSwitch);
                using (var resource = Resource.FromSwapChain<Texture2D>(_swapChain, 0))
                {
                    _renderTarget = new RenderTargetView(_device, resource);
                }

                _context.OutputMerger.SetTargets(_renderTarget);
            };

            return true;
        }

        public bool Initialize(IntPtr handle, int screenWidth, int screenHeight, bool fullScreen, float screenNear, float screenDepth)
        {
            int numerator = 60, denominator = 1;
            float fieldOfView, screenAspect;

            Factory factory = new Factory();
            Adapter adapter = factory.GetAdapter(0);
            Output adapterOutput = adapter.GetOutput(0);
            ReadOnlyCollection<ModeDescription> numModes = adapterOutput.GetDisplayModeList(Format.R8G8B8A8_UNorm, DisplayModeEnumerationFlags.Interlaced);

            foreach (ModeDescription desc in numModes)
            {
                if (desc.Height == screenHeight && desc.Width == screenWidth)
                {
                    numerator = desc.RefreshRate.Numerator;
                    denominator = desc.RefreshRate.Denominator;
                }
            }

            //get the video card's information
            AdapterDescription adapterDesc = adapter.Description;
            _videoCardMemory = (int)(adapterDesc.DedicatedVideoMemory / 1024 / 1024);
            string videoCardDesc = adapterDesc.Description;

            //dispose of objects we're finished with
            adapterOutput.Dispose();
            adapter.Dispose();
            factory.Dispose();

            //swapchain description
            var discription = new SwapChainDescription()
            {
                BufferCount = 1,
                Usage = Usage.RenderTargetOutput,
                OutputHandle = handle,
                IsWindowed = !fullScreen,
                ModeDescription = new ModeDescription(screenWidth, screenHeight, new Rational(numerator, denominator), Format.B8G8R8A8_UNorm),
                SampleDescription = new SampleDescription(1, 0),
                Flags = SwapChainFlags.AllowModeSwitch,
                SwapEffect = SwapEffect.Discard
            };

            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, discription, out _device, out _swapChain);
            _context = _device.ImmediateContext;

            using (var resource = Resource.FromSwapChain<Texture2D>(_swapChain, 0))
            {
                _renderTarget = new RenderTargetView(_device, resource);
            }

            //buffer discription for the back buffer
            var depthDisc = new Texture2DDescription()
            {
                Height = screenHeight,
                Width = screenWidth,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.D24_UNorm_S8_UInt,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };

            _depthStencilBuffer = new Texture2D(_device, depthDisc);

            var depthStencilDisc = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,

                IsStencilEnabled = true,
                StencilReadMask = 0xff,
                StencilWriteMask = 0xff,

                FrontFace = new DepthStencilOperationDescription()
                {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Increment,
                    PassOperation = StencilOperation.Keep,
                    Comparison = Comparison.Always
                },

                BackFace = new DepthStencilOperationDescription()
                {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Decrement,
                    PassOperation = StencilOperation.Keep,
                    Comparison = Comparison.Always
                }
            };

            //create and set the depth stencil state
            _depthStencilState = DepthStencilState.FromDescription(_device, depthStencilDisc);
            _context.OutputMerger.DepthStencilState = _depthStencilState;

            var depthStencilViewDisc = new DepthStencilViewDescription()
            {
                Format = Format.D24_UNorm_S8_UInt,
                Dimension = DepthStencilViewDimension.Texture2D,
                MipSlice = 0
            };

            //create the depth stencil view and set the render target
            _depthStencilView = new DepthStencilView(_device, _depthStencilBuffer, depthStencilViewDisc);
            _context.OutputMerger.SetTargets(_depthStencilView, _renderTarget);

            //discription for the rasterizer
            var rasterDisc = new RasterizerStateDescription()
            {
                IsAntialiasedLineEnabled = false,
                CullMode = CullMode.Back,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                IsDepthClipEnabled = true,
                FillMode = FillMode.Solid,
                IsFrontCounterclockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f
            };

            //create the rasterizer state and set it
            _rasterizerState = RasterizerState.FromDescription(_device, rasterDisc);
            _context.Rasterizer.State = _rasterizerState;

            var viewport = new Viewport()
            {
                Width = screenWidth,
                Height = screenHeight,
                MinZ = 0.0f,
                MaxZ = 1.0f,
                X = 0.0f,
                Y = 0.0f
            };

            _context.Rasterizer.SetViewports(viewport);

            // Setup the projection matrix.
            fieldOfView = (float)Math.PI / 4.0f;
            screenAspect = (float)screenWidth / (float)screenHeight;

            // Create the projection matrix for 3D rendering.
            _projectionMatrix = Matrix.PerspectiveFovLH(fieldOfView, screenAspect, screenNear, screenDepth);
            _worldMatrix = Matrix.Identity;
            _orthoMatrix = Matrix.OrthoLH((float)screenWidth, (float)screenHeight, screenNear, screenDepth);

            var depthDisabledStencilDesc = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,
                IsStencilEnabled = true,
                StencilReadMask = 0xFF,
                StencilWriteMask = 0xFF,

                FrontFace = new DepthStencilOperationDescription()
                {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Increment,
                    PassOperation = StencilOperation.Keep,
                    Comparison = Comparison.Always
                },

                BackFace = new DepthStencilOperationDescription()
                {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Decrement,
                    PassOperation = StencilOperation.Keep,
                    Comparison = Comparison.Always
                }
            };

            _depthDisabledStencilState = DepthStencilState.FromDescription(_device, depthDisabledStencilDesc);

            var blendStateDesc = new BlendStateDescription();
            blendStateDesc.RenderTargets[0].BlendEnable = true;

            blendStateDesc.RenderTargets[0].SourceBlend = BlendOption.One;
            blendStateDesc.RenderTargets[0].DestinationBlend = BlendOption.InverseSourceAlpha;
            blendStateDesc.RenderTargets[0].BlendOperation = BlendOperation.Add;

            blendStateDesc.RenderTargets[0].SourceBlendAlpha = BlendOption.One;
            blendStateDesc.RenderTargets[0].DestinationBlendAlpha = BlendOption.Zero;
            blendStateDesc.RenderTargets[0].BlendOperationAlpha = BlendOperation.Add;

            blendStateDesc.RenderTargets[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;

            _alphaEnabledBlendState = BlendState.FromDescription(_device, blendStateDesc);

            blendStateDesc.RenderTargets[0].BlendEnable = false;

            _alphaDisabledBlendState = BlendState.FromDescription(_device, blendStateDesc);
            
            return true;
        }

        /// <summary>
        /// Turn on z buffering for this device.
        /// </summary>
        public void TurnZBufferOn()
        {
            _context.OutputMerger.DepthStencilState = _depthStencilState;
        }

        /// <summary>
        /// Turn off z buffering for this device.
        /// </summary>
        public void TurnZBufferOff()
        {
            _context.OutputMerger.DepthStencilState = _depthDisabledStencilState;
        }

        /// <summary>
        /// Enable alpha blending for this device.
        /// </summary>
        public void TurnOnAlphaBlending()
        {
            Color4 blendFactor = new Color4(0, 0, 0, 0);

            _context.OutputMerger.BlendFactor = blendFactor;
            _context.OutputMerger.BlendState = _alphaEnabledBlendState;
        }

        /// <summary>
        /// Disable aplha blending for this device.
        /// </summary>
        public void TurnOffAlphaBlending()
        {
            Color4 blendFactor = new Color4(0, 0, 0, 0);

            _context.OutputMerger.BlendFactor = blendFactor;
            _context.OutputMerger.BlendState = _alphaDisabledBlendState;
        }

        public void Dispose()
        {
            _alphaEnabledBlendState.Dispose();
            _alphaDisabledBlendState.Dispose();
            _depthDisabledStencilState.Dispose();
            _rasterizerState.Dispose();
            _depthStencilView.Dispose();
            _depthStencilState.Dispose();
            _depthStencilBuffer.Dispose();
            _renderTarget.Dispose();
            _context.Dispose();
            _device.Dispose();
            _swapChain.Dispose();
        }
    }
}
