using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;
using KirosEngine.Input;

using Keys = System.Windows.Forms.Keys;

namespace KirosEngine.Camera
{
    /// <summary>
    /// Defines a simple camera with controls for motion
    /// </summary>
    class SimpleCamera : BaseCamera
    {
        private float _prevTime;
        private float _timeDif;
        private float _forwardSpeed;
        private float _reverseSpeed;
        private float _upwardSpeed;
        private float _downwardSpeed;
        private float _rightTurnSpeed;
        private float _leftTurnSpeed;
        private float _lookUpSpeed;
        private float _lookDownSpeed;

        private KeyboardHandler _keyHandler;

        #region Action Constants
        /// <summary>
        /// The constant action identifier for forward motion
        /// </summary>
        public const string ForwardAction = "forwardCam";

        /// <summary>
        /// The constant action identifier for left motion
        /// </summary>
        public  const string LeftAction = "leftCam";

        /// <summary>
        /// The constant action identifier for right motion
        /// </summary>
        public const string RightAction = "rightCam";

        /// <summary>
        /// The constant action identifier for backwards motion
        /// </summary>
        public const string BackAction = "backCam";
        #endregion

        /// <summary>
        /// Basic constructor for the simple camera
        /// </summary>
        /// <param name="keyHandler"></param>
        public SimpleCamera(KeyboardHandler keyHandler) : base()
        {
            _keyHandler = keyHandler;
            _keyHandler.KeyHeld += KeyHeld;
            _keyHandler.KeyPressed += KeyPressed;
        }

        /// <summary>
        /// Handle being notified of a Key held event
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        void KeyHeld(object sender, KeyEventArgs e)
        {
            KeyboardBindings keyBindings = _keyHandler.KeyboardBindings;
            
            if(e.Key == keyBindings.GetKey(ForwardAction))
            {
                //TODO: forward movement
            }
            else if(e.Key == keyBindings.GetKey(LeftAction))
            {
                //TODO: left movement
            }
            else if(e.Key == keyBindings.GetKey(RightAction))
            {
                //TODO: right movement
            }
            else if(e.Key == keyBindings.GetKey(BackAction))
            {
                //TODO: backup movement
            }
        }

        /// <summary>
        /// Handle being notified of a Key pressed event
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        void KeyPressed(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Process the updates
        /// </summary>
        /// <param name="time">The current time</param>
        public override void Update(float time)
        {
            base.Update(time);

            //get the time since last update
            _timeDif = time - _prevTime;
            _prevTime = time;
        }

        //TODO: rework movement
        protected virtual void MoveForward()
        {
            float radians = _rotation.Y;
        }
    }
}
