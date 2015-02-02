using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.D3DCompiler;
using SlimDX.Windows;
using SlimDX.Direct3D11;
using KirosEngine.Textures;
using KirosEngine.Shader;

using Device = SlimDX.Direct3D11.Device;
using Marshal = System.Runtime.InteropServices.Marshal;
using Buffer = SlimDX.Direct3D11.Buffer;

namespace KirosEngine.ScreenText
{
    class Text
    {
        struct SentenceType
        {
            public Buffer vertexBuffer, indexBuffer;
            public int vertexCount, indexCount;
            public Vector4 color;

            /// <summary>
            /// Dispose of the buffers
            /// </summary>
            public void Dispose()
            {
                if(vertexBuffer != null)
                {
                    vertexBuffer.Dispose();
                }
                if(indexBuffer != null)
                {
                    indexBuffer.Dispose();
                }
            }
        }

        public static string DefaultTextShaderKey = "defaultText";

        private Device _device;
        private Font _font;
        private BaseShader _fontShader;
        private int _screenHeight, _screenWidth;
        private string _text;
        private Vector2 _position;

        private float _textWidth;

        private SentenceType _sentence;

        /// <summary>
        /// Public accessor for the text property
        /// </summary>
        public string Verse
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                this.UpdateSentence();
            }
        }

        /// <summary>
        /// Public accessor for the color property
        /// </summary>
        public Vector4 Color
        {
            get
            {
                return _sentence.color;
            }
            set
            {
                _sentence.color = value;
            }
        }

        /// <summary>
        /// Public accessor for the position property
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                if(_text != null)
                {
                    this.UpdateSentence();
                }                
            }
        }

        /// <summary>
        /// Public accessor for the width property
        /// </summary>
        public float TextWidth
        {
            get
            {
                return _textWidth;
            }
        }

        /// <summary>
        /// Base Constructor for Text
        /// </summary>
        /// <param name="device">The directx device</param>
        /// <param name="screenHeight">The screen height as an integer</param>
        /// <param name="screenWidth">The screen width as an integer</param>
        /// <param name="baseView">The base view matrix</param>
        /// <param name="textFont">The font object to be used by the text</param>
        /// <param name="shader">The shader to be used for the text</param>
        public Text(Device device, int screenHeight, int screenWidth, Font textFont, BaseShader shader)
        {
            _device = device;
            _screenHeight = screenHeight;
            _screenWidth = screenWidth;

            _font = textFont;
            _fontShader = shader;

            _sentence.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f); //text color defaults to black
        }

        /// <summary>
        /// Update the sentence for the new text
        /// </summary>
        private void UpdateSentence()
        {
            Vertex2D[] vertices;
            uint[] indices;
            BufferDescription vertBufferDisc, indexBufferDisc;

            _sentence.vertexCount = _text.Length * 6;
            _sentence.indexCount = _sentence.vertexCount;

            vertBufferDisc = new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = Marshal.SizeOf(typeof(Vertex2D)) * _sentence.vertexCount,
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            indexBufferDisc = new BufferDescription()
            {
                Usage = ResourceUsage.Default,
                SizeInBytes = sizeof(uint) * _sentence.indexCount,
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            //set the index array.
            indices = new uint[_sentence.indexCount];
            for (int i = 0; i < _sentence.indexCount; i++)
            {
                indices[i] = (uint)i;
            }

            //build the vertices
            float drawX = (float)(((_screenWidth / 2) * -1) + _position.X);
            float drawY = (float)((_screenHeight / 2) - _position.Y);
            vertices = _font.BuildVertexArrayF(_sentence.vertexCount, _text, drawX, drawY);

            _sentence.Dispose();

            DataStream vertStream = new DataStream(Marshal.SizeOf(typeof(Vertex2D)) * _sentence.vertexCount, true, true);
            foreach (Vertex2D vert in vertices)
            {
                vertStream.Write(vert);
            }
            vertStream.Position = 0;

            _sentence.vertexBuffer = new Buffer(_device, vertStream, vertBufferDisc);

            vertStream.Close();
            vertStream.Dispose();

            DataStream indexStream = new DataStream(sizeof(uint) * _sentence.indexCount, true, true);
            foreach (uint index in indices)
            {
                indexStream.Write(index);
            }
            indexStream.Position = 0;

            _sentence.indexBuffer = new Buffer(_device, indexStream, indexBufferDisc);

            indexStream.Close();
            indexStream.Dispose();

            _textWidth = _font.GetTextLength(_text);
        }

        //TODO: apply transforms to text
        /// <summary>
        /// Draw the text on the screen
        /// </summary>
        /// <param name="context">The directx device context</param>
        /// <param name="world">the world matrix</param>
        /// <param name="ortho">the orthograpic matrix</param>
        public void Draw(DeviceContext context, Matrix world, Matrix view, Matrix ortho)
        {
            if(_text != null || _text != string.Empty)
            {
                context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_sentence.vertexBuffer, Marshal.SizeOf(typeof(Vertex2D)), 0));
                context.InputAssembler.SetIndexBuffer(_sentence.indexBuffer, Format.R32_UInt, 0);
                context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

                _fontShader.Draw(context, _sentence.indexCount, world, ortho, view, _font.GetTexture(), _sentence.color);
            }
            else
            {
                //TODO: exception/error text was not set
            }
        }

        /// <summary>
        /// Dispose of the resources used by the text
        /// </summary>
        public void Dispose()
        {
            //font and shader are not disposed here and should not be in case they are used elsewhere
            _sentence.Dispose();
        }
    }
}
