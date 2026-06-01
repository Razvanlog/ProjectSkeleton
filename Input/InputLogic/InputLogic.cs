using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Text;
using TheAdventure.Camera;
using TheAdventure.Entities.Player;
using TheAdventure.GameLogic;
namespace TheAdventure.Input.InputLogic
{
    internal class InputLogic
    {
        private Sdl sdl;
        private GameLogic.GameLogic gameLogic;
        private InputKey.InputKey inputManager;
        private int mousePosX;
        private int mousePosY;

        public InputLogic(Sdl sdl, GameLogic.GameLogic gameLogic, InputKey.InputKey inputKey)
        {
            this.sdl = sdl;
            this.gameLogic = gameLogic;
            this.inputManager = inputKey;
        }

        public bool ProcessInput()
        {
            var ev = new Event();

            while (sdl.PollEvent(ref ev) != 0)
            {
                if (ev.Type == (uint)EventType.Quit)
                {
                    return true;
                }

                switch (ev.Type)
                {
                    case (uint)EventType.Mousemotion:
                        mousePosX = ev.Motion.X;
                        mousePosY = ev.Motion.Y;
                        break;

                    case (uint)EventType.Mousebuttondown:
                        if (ev.Button.Button == (byte)MouseButton.Primary)
                            inputManager.mouseButtonLeft = true;
                        else if (ev.Button.Button == (byte)MouseButton.Secondary)
                            inputManager.mouseButtonRight = true;
                        break;
                    case (uint)EventType.Mousebuttonup:
                        if (ev.Button.Button == (byte)MouseButton.Primary)
                            inputManager.mouseButtonLeft = false;
                        else if ((ev.Button.Button == (byte)MouseButton.Secondary))
                            inputManager.mouseButtonRight = false;
                        break;
                    case (uint)EventType.Keydown:
                        var keyDown = (KeyCode)ev.Key.Keysym.Scancode;
                        inputManager.SetKeyState(keyDown, true);
                        if ((KeyCode)ev.Key.Keysym.Scancode == KeyCode.Q)
                            return true;
                        break;
                    case (uint)EventType.Keyup:
                        var keyUp = (KeyCode)ev.Key.Keysym.Scancode;
                        inputManager.SetKeyState(keyUp, false);
                        break;

                    case (uint)EventType.Windowevent:
                        if (ev.Window.Event == (byte)WindowEventID.TakeFocus)
                        {
                            unsafe
                            {
                                sdl.SetWindowInputFocus(sdl.GetWindowFromID(ev.Window.WindowID));
                            }
                        }
                        break;
                }
                if (gameLogic.Camera != null)
                {
                    var worldMousePosition = gameLogic.Camera.toWorldCoordinates(new Silk.NET.Maths.Vector2D<int>(mousePosX, mousePosY));
                    inputManager.MouseX = worldMousePosition.X;
                    inputManager.MouseY = worldMousePosition.Y;
                }
            }
            return false;
        }
    }
}
