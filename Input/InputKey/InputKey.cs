using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Text;
using Silk.NET.SDL;

namespace TheAdventure.Input.InputKey
{
    internal unsafe class InputKey
    {
        public static InputKey? Instance { get; private set; }
        private Sdl sdl;
        private byte* keyboard;
        private int keyboardSize;
        public int MouseX { get; set; }
        public int MouseY { get; set; }
        public bool mouseButtonLeft { get; set; }
        public bool mouseButtonRight { get; set; }
        private HashSet<KeyCode> pressedKeys = new HashSet<KeyCode>();
        public InputKey(Sdl sdl)
        {
            if (Instance == null)
            {
                Instance = this;
            }
            this.sdl = sdl;
            int numKeysLocal;
            keyboard = sdl.GetKeyboardState(&numKeysLocal);
            keyboardSize = numKeysLocal;
        }

        public void SetKeyState(KeyCode key, bool isDown)
        {
            if (isDown)
            {
                pressedKeys.Add(key);
            }
            else
            {
                pressedKeys.Remove(key);
            }
        }

        public bool isHeldDown(KeyCode key)
        {
            if (pressedKeys.Contains(key))
                return true;
            return false;
        }
    }
}
