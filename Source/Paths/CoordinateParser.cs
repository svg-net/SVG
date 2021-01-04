using System;
using System.Globalization;

namespace Svg
{
    internal class CoordinateParser
    {
        private enum NumState
        {
            Invalid,
            Separator,
            Prefix,
            Integer,
            DecPlace,
            Fraction,
            Exponent,
            ExpPrefix,
            ExpValue
        }

        private NumState _currState;
        private NumState _newState;
        private int _charsPosition;
        private int _position;
        private bool _hasMore;

        public void Init(ref ReadOnlySpan<char> chars)
        {
            _currState = NumState.Separator;
            _newState = NumState.Separator;
            _charsPosition = 0;
            _position = 0;
            _hasMore = chars.Length > 0;
            if (char.IsLetter(chars[0])) ++_charsPosition;
        }

        private bool MarkState(bool state)
        {
            _hasMore = state;
            ++_charsPosition;
            return state;
        }

        public bool TryGetBool(out bool result, ref ReadOnlySpan<char> chars)
        {
            var charsLength = chars.Length;

            while (_charsPosition < charsLength && _hasMore)
            {
                switch (_currState)
                {
                    case NumState.Separator:
                        var currentChar = chars[_charsPosition];
                        if (IsCoordSeparator(currentChar))
                        {
                            _newState = NumState.Separator;
                        }
                        else if (currentChar == '0')
                        {
                            result = false;
                            _newState = NumState.Separator;
                            _position = _charsPosition + 1;
                            return MarkState(true);
                        }
                        else if (currentChar == '1')
                        {
                            result = true;
                            _newState = NumState.Separator;
                            _position = _charsPosition + 1;
                            return MarkState(true);
                        }
                        else
                        {
                            result = false;
                            return MarkState(false);
                        }
                        break;
                    default:
                        result = false;
                        return MarkState(false);
                }
                ++_charsPosition;
            }
            result = false;
            return MarkState(false);
        }

        public bool TryGetFloat(out float result, ref ReadOnlySpan<char> chars)
        {
            var charsLength = chars.Length;

            while (_charsPosition < charsLength && _hasMore)
            {
                var currentChar = chars[_charsPosition];

                switch (_currState)
                {
                    case NumState.Separator:
                        if (char.IsNumber(currentChar))
                        {
                            _newState = NumState.Integer;
                        }
                        else if (IsCoordSeparator(currentChar))
                        {
                            _newState = NumState.Separator;
                        }
                        else
                        {
                            switch (currentChar)
                            {
                                case '.':
                                    _newState = NumState.DecPlace;
                                    break;
                                case '+':
                                case '-':
                                    _newState = NumState.Prefix;
                                    break;
                                default:
                                    _newState = NumState.Invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.Prefix:
                        if (char.IsNumber(currentChar))
                        {
                            _newState = NumState.Integer;
                        }
                        else if (currentChar == '.')
                        {
                            _newState = NumState.DecPlace;
                        }
                        else
                        {
                            _newState = NumState.Invalid;
                        }
                        break;
                    case NumState.Integer:
                        if (char.IsNumber(currentChar))
                        {
                            _newState = NumState.Integer;
                        }
                        else if (IsCoordSeparator(currentChar))
                        {
                            _newState = NumState.Separator;
                        }
                        else
                        {
                            switch (currentChar)
                            {
                                case '.':
                                    _newState = NumState.DecPlace;
                                    break;
                                case 'E':
                                case 'e':
                                    _newState = NumState.Exponent;
                                    break;
                                case '+':
                                case '-':
                                    _newState = NumState.Prefix;
                                    break;
                                default:
                                    _newState = NumState.Invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.DecPlace:
                        if (char.IsNumber(currentChar))
                        {
                            _newState = NumState.Fraction;
                        }
                        else if (IsCoordSeparator(currentChar))
                        {
                            _newState = NumState.Separator;
                        }
                        else
                        {
                            switch (currentChar)
                            {
                                case 'E':
                                case 'e':
                                    _newState = NumState.Exponent;
                                    break;
                                case '+':
                                case '-':
                                    _newState = NumState.Prefix;
                                    break;
                                default:
                                    _newState = NumState.Invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.Fraction:
                        if (char.IsNumber(currentChar))
                        {
                            _newState = NumState.Fraction;
                        }
                        else if (IsCoordSeparator(currentChar))
                        {
                            _newState = NumState.Separator;
                        }
                        else
                        {
                            switch (currentChar)
                            {
                                case '.':
                                    _newState = NumState.DecPlace;
                                    break;
                                case 'E':
                                case 'e':
                                    _newState = NumState.Exponent;
                                    break;
                                case '+':
                                case '-':
                                    _newState = NumState.Prefix;
                                    break;
                                default:
                                    _newState = NumState.Invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.Exponent:
                        if (char.IsNumber(currentChar))
                        {
                            _newState = NumState.ExpValue;
                        }
                        else if (IsCoordSeparator(currentChar))
                        {
                            _newState = NumState.Invalid;
                        }
                        else
                        {
                            switch (currentChar)
                            {
                                case '+':
                                case '-':
                                    _newState = NumState.ExpPrefix;
                                    break;
                                default:
                                    _newState = NumState.Invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.ExpPrefix:
                        if (char.IsNumber(currentChar))
                        {
                            _newState = NumState.ExpValue;
                        }
                        else
                        {
                            _newState = NumState.Invalid;
                        }
                        break;
                    case NumState.ExpValue:
                        if (char.IsNumber(currentChar))
                        {
                            _newState = NumState.ExpValue;
                        }
                        else if (IsCoordSeparator(currentChar))
                        {
                            _newState = NumState.Separator;
                        }
                        else
                        {
                            switch (currentChar)
                            {
                                case '.':
                                    _newState = NumState.DecPlace;
                                    break;
                                case '+':
                                case '-':
                                    _newState = NumState.Prefix;
                                    break;
                                default:
                                    _newState = NumState.Invalid;
                                    break;
                            }
                        }
                        break;
                }

                if (_currState != NumState.Separator && _newState < _currState)
                {
#if NETSTANDARD2_1 || NETCORE || NETCOREAPP2_2 || NETCOREAPP3_0
                    result = float.Parse(chars.Slice(_position, _charsPosition - _position), NumberStyles.Float, CultureInfo.InvariantCulture);
#else
                    result = float.Parse(chars.Slice(_position, _charsPosition - _position).ToString(), NumberStyles.Float, CultureInfo.InvariantCulture);
#endif
                    _position = _charsPosition;
                    _currState = _newState;
                    return MarkState(true);
                }
                else if (_newState != _currState && _currState == NumState.Separator)
                {
                    _position = _charsPosition;
                }

                if (_newState == NumState.Invalid)
                {
                    result = float.MinValue;
                    return MarkState(false);
                }
                _currState = _newState;
                ++_charsPosition;
            }

            if (_currState == NumState.Separator || !_hasMore || _position >= charsLength)
            {
                result = float.MinValue;
                return MarkState(false);
            }
            else
            {
#if NETSTANDARD2_1 || NETCORE || NETCOREAPP2_2 || NETCOREAPP3_0
                result = float.Parse(chars.Slice(_position, charsLength - _position), NumberStyles.Float, CultureInfo.InvariantCulture);
#else
                result = float.Parse(chars.Slice(_position, charsLength - _position).ToString(), NumberStyles.Float, CultureInfo.InvariantCulture);
#endif
                _position = charsLength;
                return MarkState(true);
            }
        }

        private static bool IsCoordSeparator(char value)
        {
            switch (value)
            {
                case ' ':
                case '\t':
                case '\n':
                case '\r':
                case ',':
                    return true;
            }
            return false;
        }
    }
}
