using System;
using System.Globalization;

namespace Svg
{
    internal class CoordinateParser
    {
        private enum NumState
        {
            invalid,
            separator,
            prefix,
            integer,
            decPlace,
            fraction,
            exponent,
            expPrefix,
            expValue
        }

        private NumState _currState;
        private NumState _newState;
        private int i;
        private int _position;
        private bool _hasMore;

        public void Init(ref ReadOnlySpan<char> chars)
        {
            _currState = NumState.separator;
            _newState = NumState.separator;
            i = 0;
            _position = 0;
            _hasMore = chars.Length <= 0 ? false : true;
            if (char.IsLetter(chars[0])) ++i;
        }

        private bool MarkState(bool state)
        {
            _hasMore = state;
            ++i;
            return state;
        }

        public bool TryGetBool(out bool result, ref ReadOnlySpan<char> chars)
        {
            var charsLength = chars.Length;

            while (i < charsLength && _hasMore)
            {
                switch (_currState)
                {
                    case NumState.separator:
                        if (IsCoordSeparator(chars[i]))
                        {
                            _newState = NumState.separator;
                        }
                        else if (chars[i] == '0')
                        {
                            result = false;
                            _newState = NumState.separator;
                            _position = i + 1;
                            return MarkState(true);
                        }
                        else if (chars[i] == '1')
                        {
                            result = true;
                            _newState = NumState.separator;
                            _position = i + 1;
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
                ++i;
            }
            result = false;
            return MarkState(false);
        }

        public bool TryGetFloat(out float result, ref ReadOnlySpan<char> chars)
        {
            var charsLength = chars.Length;

            while (i < charsLength && _hasMore)
            {
                switch (_currState)
                {
                    case NumState.separator:
                        if (char.IsNumber(chars[i]))
                        {
                            _newState = NumState.integer;
                        }
                        else if (IsCoordSeparator(chars[i]))
                        {
                            _newState = NumState.separator;
                        }
                        else
                        {
                            switch (chars[i])
                            {
                                case '.':
                                    _newState = NumState.decPlace;
                                    break;
                                case '+':
                                case '-':
                                    _newState = NumState.prefix;
                                    break;
                                default:
                                    _newState = NumState.invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.prefix:
                        if (char.IsNumber(chars[i]))
                        {
                            _newState = NumState.integer;
                        }
                        else if (chars[i] == '.')
                        {
                            _newState = NumState.decPlace;
                        }
                        else
                        {
                            _newState = NumState.invalid;
                        }
                        break;
                    case NumState.integer:
                        if (char.IsNumber(chars[i]))
                        {
                            _newState = NumState.integer;
                        }
                        else if (IsCoordSeparator(chars[i]))
                        {
                            _newState = NumState.separator;
                        }
                        else
                        {
                            switch (chars[i])
                            {
                                case '.':
                                    _newState = NumState.decPlace;
                                    break;
                                case 'E':
                                case 'e':
                                    _newState = NumState.exponent;
                                    break;
                                case '+':
                                case '-':
                                    _newState = NumState.prefix;
                                    break;
                                default:
                                    _newState = NumState.invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.decPlace:
                        if (char.IsNumber(chars[i]))
                        {
                            _newState = NumState.fraction;
                        }
                        else if (IsCoordSeparator(chars[i]))
                        {
                            _newState = NumState.separator;
                        }
                        else
                        {
                            switch (chars[i])
                            {
                                case 'E':
                                case 'e':
                                    _newState = NumState.exponent;
                                    break;
                                case '+':
                                case '-':
                                    _newState = NumState.prefix;
                                    break;
                                default:
                                    _newState = NumState.invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.fraction:
                        if (char.IsNumber(chars[i]))
                        {
                            _newState = NumState.fraction;
                        }
                        else if (IsCoordSeparator(chars[i]))
                        {
                            _newState = NumState.separator;
                        }
                        else
                        {
                            switch (chars[i])
                            {
                                case '.':
                                    _newState = NumState.decPlace;
                                    break;
                                case 'E':
                                case 'e':
                                    _newState = NumState.exponent;
                                    break;
                                case '+':
                                case '-':
                                    _newState = NumState.prefix;
                                    break;
                                default:
                                    _newState = NumState.invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.exponent:
                        if (char.IsNumber(chars[i]))
                        {
                            _newState = NumState.expValue;
                        }
                        else if (IsCoordSeparator(chars[i]))
                        {
                            _newState = NumState.invalid;
                        }
                        else
                        {
                            switch (chars[i])
                            {
                                case '+':
                                case '-':
                                    _newState = NumState.expPrefix;
                                    break;
                                default:
                                    _newState = NumState.invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.expPrefix:
                        if (char.IsNumber(chars[i]))
                        {
                            _newState = NumState.expValue;
                        }
                        else
                        {
                            _newState = NumState.invalid;
                        }
                        break;
                    case NumState.expValue:
                        if (char.IsNumber(chars[i]))
                        {
                            _newState = NumState.expValue;
                        }
                        else if (IsCoordSeparator(chars[i]))
                        {
                            _newState = NumState.separator;
                        }
                        else
                        {
                            switch (chars[i])
                            {
                                case '.':
                                    _newState = NumState.decPlace;
                                    break;
                                case '+':
                                case '-':
                                    _newState = NumState.prefix;
                                    break;
                                default:
                                    _newState = NumState.invalid;
                                    break;
                            }
                        }
                        break;
                }

                if (_currState != NumState.separator && _newState < _currState)
                {
#if NETSTANDARD2_1 || NETCORE || NETCOREAPP2_2 || NETCOREAPP3_0
                    result = float.Parse(chars.Slice(_position, i - _position), NumberStyles.Float, CultureInfo.InvariantCulture);
#else
                    result = float.Parse(chars.Slice(_position, i - _position).ToString(), NumberStyles.Float, CultureInfo.InvariantCulture);
#endif
                    _position = i;
                    _currState = _newState;
                    return MarkState(true);
                }
                else if (_newState != _currState && _currState == NumState.separator)
                {
                    _position = i;
                }

                if (_newState == NumState.invalid)
                {
                    result = float.MinValue;
                    return MarkState(false);
                }
                _currState = _newState;
                ++i;
            }

            if (_currState == NumState.separator || !_hasMore || _position >= charsLength)
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
