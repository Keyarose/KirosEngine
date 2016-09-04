using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;

namespace KirosEngine.Camera
{
    /// <summary>
    /// Basic camera object
    /// </summary>
    class BaseCamera
    {
        protected Vector3 _position;
        protected Vector3 _rotation;

        private Matrix _viewMatrix;

        /// <summary>
        /// Public accessor for the camera's position
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return _position;
            }
        }

        /// <summary>
        /// Public accessor for the camera's rotation
        /// </summary>
        public Vector3 Rotation
        {
            get
            {
                return _rotation;
            }
        }

        public BaseCamera()
        {
            _position = Vector3.Zero;

            _rotation = Vector3.Zero;
        }

        /// <summary>
        /// Set the camera's position
        /// </summary>
        /// <param name="newPosition">the new position to set to</param>
        public void SetPosition(Vector3 newPosition)
        {
            _position = newPosition;
        }

        /// <summary>
        /// Set the camera's rotation
        /// </summary>
        /// <param name="newRotation">the new rotation to set to</param>
        public void SetRotation(Vector3 newRotation)
        {
            _rotation = newRotation;
        }

        /// <summary>
        /// Get the camera's view matrix
        /// </summary>
        /// <returns>A matrix containing the view matrix</returns>
        public Matrix GetViewMatrix()
        {
            return _viewMatrix;
        }

        /// <summary>
        /// Perform updates on the camera's values
        /// </summary>
        /// <param name="time">The current time tick</param>
        public virtual void Update(float time)
        {

        }

        /// <summary>
        /// Perform all necessary actions to draw
        /// </summary>
        public virtual void Draw()
        {
            //TODO: refactor to calculate only if data has changed
            Vector3 up, lookAt, position;
            float yaw, pitch, roll;
            Matrix rotationMatrix;

            up = new Vector3(0.0f, 1.0f, 0.0f);
            position = Position;
            lookAt = new Vector3(0.0f, 0.0f, 1.0f);

            pitch = _rotation.X * 0.0174532925f;
            yaw = _rotation.Y * 0.0174532925f;
            roll = _rotation.Z * 0.0174532925f;

            rotationMatrix = Matrix.RotationYawPitchRoll(yaw, pitch, roll);
            Vector3.TransformCoordinate(lookAt, rotationMatrix);
            Vector3.TransformCoordinate(up, rotationMatrix);

            lookAt = position + lookAt;

            _viewMatrix = Matrix.LookAtLH(position, lookAt, up);
        }
    }
}
