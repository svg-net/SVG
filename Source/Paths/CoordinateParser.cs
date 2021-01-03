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

        private NumState _currState = NumState.separator;
        private NumState _newState = NumState.separator;
        private int i = 0;

        public int Position { get; private set; } = 0;
        public bool HasMore { get; private set; } = true;

        public CoordinateParser(ref ReadOnlySpan<char> _coords)
        {
            if (_coords.Length < 0) HasMore = false;
            if (char.IsLetter(_coords[0])) ++i;
        }

        private bool MarkState(bool state)
        {
            HasMore = state;
            ++i;
            return state;
        }

        public bool TryGetBool(out bool result, ref ReadOnlySpan<char> _coords)
        {
            while (i < _coords.Length && HasMore)
            {
                switch (_currState)
                {
                    case NumState.separator:
                        if (IsCoordSeparator(_coords[i]))
                        {
                            _newState = NumState.separator;
                        }
                        else if (_coords[i] == '0')
                        {
                            result = false;
                            _newState = NumState.separator;
                            Position = i + 1;
                            return MarkState(true);
                        }
                        else if (_coords[i] == '1')
                        {
                            result = true;
                            _newState = NumState.separator;
                            Position = i + 1;
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

        public bool TryGetFloat(out float result, ref ReadOnlySpan<char> _coords)
        {
            while (i < _coords.Length && HasMore)
            {
                switch (_currState)
                {
                    case NumState.separator:
                        if (char.IsNumber(_coords[i]))
                        {
                            _newState = NumState.integer;
                        }
                        else if (IsCoordSeparator(_coords[i]))
                        {
                            _newState = NumState.separator;
                        }
                        else
                        {
                            switch (_coords[i])
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
                        if (char.IsNumber(_coords[i]))
                        {
                            _newState = NumState.integer;
                        }
                        else if (_coords[i] == '.')
                        {
                            _newState = NumState.decPlace;
                        }
                        else
                        {
                            _newState = NumState.invalid;
                        }
                        break;
                    case NumState.integer:
                        if (char.IsNumber(_coords[i]))
                        {
                            _newState = NumState.integer;
                        }
                        else if (IsCoordSeparator(_coords[i]))
                        {
                            _newState = NumState.separator;
                        }
                        else
                        {
                            switch (_coords[i])
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
                        if (char.IsNumber(_coords[i]))
                        {
                            _newState = NumState.fraction;
                        }
                        else if (IsCoordSeparator(_coords[i]))
                        {
                            _newState = NumState.separator;
                        }
                        else
                        {
                            switch (_coords[i])
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
                        if (char.IsNumber(_coords[i]))
                        {
                            _newState = NumState.fraction;
                        }
                        else if (IsCoordSeparator(_coords[i]))
                        {
                            _newState = NumState.separator;
                        }
                        else
                        {
                            switch (_coords[i])
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
                        if (char.IsNumber(_coords[i]))
                        {
                            _newState = NumState.expValue;
                        }
                        else if (IsCoordSeparator(_coords[i]))
                        {
                            _newState = NumState.invalid;
                        }
                        else
                        {
                            switch (_coords[i])
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
                        if (char.IsNumber(_coords[i]))
                        {
                            _newState = NumState.expValue;
                        }
                        else
                        {
                            _newState = NumState.invalid;
                        }
                        break;
                    case NumState.expValue:
                        if (char.IsNumber(_coords[i]))
                        {
                            _newState = NumState.expValue;
                        }
                        else if (IsCoordSeparator(_coords[i]))
                        {
                            _newState = NumState.separator;
                        }
                        else
                        {
                            switch (_coords[i])
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
#if NETSTANDARD2_1 || NETCOREAPP2_2
                    result = float.Parse(_coords.Slice(Position, i - Position), NumberStyles.Float, CultureInfo.InvariantCulture);
#else
                    result = float.Parse(_coords.Slice(Position, i - Position).ToString(), NumberStyles.Float, CultureInfo.InvariantCulture);
#endif
                    Position = i;
                    _currState = _newState;
                    return MarkState(true);
                }
                else if (_newState != _currState && _currState == NumState.separator)
                {
                    Position = i;
                }

                if (_newState == NumState.invalid)
                {
                    result = float.MinValue;
                    return MarkState(false);
                }
                _currState = _newState;
                ++i;
            }

            if (_currState == NumState.separator || !HasMore || Position >= _coords.Length)
            {
                result = float.MinValue;
                return MarkState(false);
            }
            else
            {
#if NETSTANDARD2_1 || NETCOREAPP2_2
                result = float.Parse(_coords.Slice(Position, _coords.Length - Position), NumberStyles.Float, CultureInfo.InvariantCulture);
#else
                result = float.Parse(_coords.Slice(Position, _coords.Length - Position).ToString(), NumberStyles.Float, CultureInfo.InvariantCulture);
#endif
                Position = _coords.Length;
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
