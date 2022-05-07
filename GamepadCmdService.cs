using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace GamepadCmdMapper
{
    /// <summary>
    /// Manage gamepad buttons detection and test if the buttons 
    /// pressed by the user match with one of a parameter defined in the parameters file
    /// </summary>
    public class GamepadCmdService
    {
        private Parameter[] _macros = new Parameter[0];
        private Gamepad _gamepad = new Gamepad();
        private bool _wasConnected = true;
        private Parameter.ButtonFlags _pressedButton = 0;
        private Parameter.ButtonFlags _pressedButtonPrev = 0;
        private int _pressedButtonDuration = 0;

        /// <summary>
        /// Initialize service with parameters
        /// </summary>
        public bool Init(string filePath, out string initMsg)
        {
            List<Parameter> lstMacros = new List<Parameter>();

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] line = sr.ReadLine().Split(';');
                        lstMacros.Add(new Parameter(line[0], int.Parse(line[1]), line[2]));
                    }
                }

                _macros = lstMacros.ToArray();

                initMsg = string.Concat("Init OK :", Environment.NewLine, ToString());
            }
            catch (Exception ex)
            {
                initMsg = string.Concat("Init Error :", Environment.NewLine, ex.Message);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Run gamepad buttons detection and detect if one of the pattern defined in parameters is matching
        /// In case of matching, run the corresponding command and continue the detection
        /// </summary>
        public string GetState(int delayMs)
        {
            string state = string.Empty;

            if (!_gamepad.Refresh())
            {
                if (_wasConnected)
                {
                    _wasConnected = false;
                    state = "Please connect a controller.";
                }
            }
            else
            {
                if (!_wasConnected)
                {
                    _wasConnected = true;
                    state = "Controller connected on port " + _gamepad.GetPort();
                }

                _pressedButtonPrev = _pressedButton;
                _pressedButton = _gamepad.GetPressedButton();

                // New button or new button combinaison pressed
                if (_pressedButton != _pressedButtonPrev)
                {
#if DEBUG
                    if (_pressedButton != Parameter.ButtonFlags.None)
                    {
                        state = "Button pressed : " + _pressedButton;
                    }
#endif

                    // Re-init duration counter
                    _pressedButtonDuration = delayMs;
                }
                else
                {
                    // Increment counter
                    _pressedButtonDuration += delayMs;

                    // Check for matching macros
                    foreach (Parameter macro in _macros.Where(x => x.IsMatched(_pressedButton, _pressedButtonDuration)))
                    {
                        state = macro.ToString();
                        macro.DoCommand();
                    }
                }
            }

            return state;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Parameter macro in _macros)
            {
                sb.AppendLine(macro.ToString());
            }

            return sb.ToString();
        }
    }
}
