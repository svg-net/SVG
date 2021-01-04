using System;
using System.Globalization;

namespace Svg
{
    internal enum NumState
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

    internal struct CoordinateParserState
    {
        public NumState _currState;
        public NumState _newState;
        public int _charsPosition;
        public int _position;
        public bool _hasMore;

        public CoordinateParserState(ref ReadOnlySpan<char> chars)
        {
            _currState = NumState.Separator;
            _newState = NumState.Separator;
            _charsPosition = 0;
            _position = 0;
            _hasMore = chars.Length > 0;
            if (char.IsLetter(chars[0])) ++_charsPosition;
        }
    }

    internal static class CoordinateParser
    {
        private static bool MarkState(bool hasMode, ref CoordinateParserState state)
        {
            state._hasMore = hasMode;
            ++state._charsPosition;
            return hasMode;
        }

        public static bool TryGetBool(out bool result, ref ReadOnlySpan<char> chars, ref CoordinateParserState state)
        {
            var charsLength = chars.Length;

            while (state._charsPosition < charsLength && state._hasMore)
            {
                switch (state._currState)
                {
                    case NumState.Separator:
                        var currentChar = chars[state._charsPosition];
                        if (IsCoordSeparator(currentChar))
                        {
                            state._newState = NumState.Separator;
                        }
                        else if (currentChar == '0')
                        {
                            result = false;
                            state._newState = NumState.Separator;
                            state._position = state._charsPosition + 1;
                            return MarkState(true, ref state);
                        }
                        else if (currentChar == '1')
                        {
                            result = true;
                            state._newState = NumState.Separator;
                            state._position = state._charsPosition + 1;
                            return MarkState(true, ref state);
                        }
                        else
                        {
                            result = false;
                            return MarkState(false, ref state);
                        }
                        break;
                    default:
                        result = false;
                        return MarkState(false, ref state);
                }
                ++state._charsPosition;
            }
            result = false;
            return MarkState(false, ref state);
        }

        public static bool TryGetFloat(out float result, ref ReadOnlySpan<char> chars, ref CoordinateParserState state)
        {
            var charsLength = chars.Length;

            while (state._charsPosition < charsLength && state._hasMore)
            {
                var currentChar = chars[state._charsPosition];

                switch (state._currState)
                {
                    case NumState.Separator:
                        if (char.IsNumber(currentChar))
                        {
                            state._newState = NumState.Integer;
                        }
                        else if (IsCoordSeparator(currentChar))
                        {
                            state._newState = NumState.Separator;
                        }
                        else
                        {
                            switch (currentChar)
                            {
                                case '.':
                                    state._newState = NumState.DecPlace;
                                    break;
                                case '+':
                                case '-':
                                    state._newState = NumState.Prefix;
                                    break;
                                default:
                                    state._newState = NumState.Invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.Prefix:
                        if (char.IsNumber(currentChar))
                        {
                            state._newState = NumState.Integer;
                        }
                        else if (currentChar == '.')
                        {
                            state._newState = NumState.DecPlace;
                        }
                        else
                        {
                            state._newState = NumState.Invalid;
                        }
                        break;
                    case NumState.Integer:
                        if (char.IsNumber(currentChar))
                        {
                            state._newState = NumState.Integer;
                        }
                        else if (IsCoordSeparator(currentChar))
                        {
                            state._newState = NumState.Separator;
                        }
                        else
                        {
                            switch (currentChar)
                            {
                                case '.':
                                    state._newState = NumState.DecPlace;
                                    break;
                                case 'E':
                                case 'e':
                                    state._newState = NumState.Exponent;
                                    break;
                                case '+':
                                case '-':
                                    state._newState = NumState.Prefix;
                                    break;
                                default:
                                    state._newState = NumState.Invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.DecPlace:
                        if (char.IsNumber(currentChar))
                        {
                            state._newState = NumState.Fraction;
                        }
                        else if (IsCoordSeparator(currentChar))
                        {
                            state._newState = NumState.Separator;
                        }
                        else
                        {
                            switch (currentChar)
                            {
                                case 'E':
                                case 'e':
                                    state._newState = NumState.Exponent;
                                    break;
                                case '+':
                                case '-':
                                    state._newState = NumState.Prefix;
                                    break;
                                default:
                                    state._newState = NumState.Invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.Fraction:
                        if (char.IsNumber(currentChar))
                        {
                            state._newState = NumState.Fraction;
                        }
                        else if (IsCoordSeparator(currentChar))
                        {
                            state._newState = NumState.Separator;
                        }
                        else
                        {
                            switch (currentChar)
                            {
                                case '.':
                                    state._newState = NumState.DecPlace;
                                    break;
                                case 'E':
                                case 'e':
                                    state._newState = NumState.Exponent;
                                    break;
                                case '+':
                                case '-':
                                    state._newState = NumState.Prefix;
                                    break;
                                default:
                                    state._newState = NumState.Invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.Exponent:
                        if (char.IsNumber(currentChar))
                        {
                            state._newState = NumState.ExpValue;
                        }
                        else if (IsCoordSeparator(currentChar))
                        {
                            state._newState = NumState.Invalid;
                        }
                        else
                        {
                            switch (currentChar)
                            {
                                case '+':
                                case '-':
                                    state._newState = NumState.ExpPrefix;
                                    break;
                                default:
                                    state._newState = NumState.Invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.ExpPrefix:
                        if (char.IsNumber(currentChar))
                        {
                            state._newState = NumState.ExpValue;
                        }
                        else
                        {
                            state._newState = NumState.Invalid;
                        }
                        break;
                    case NumState.ExpValue:
                        if (char.IsNumber(currentChar))
                        {
                            state._newState = NumState.ExpValue;
                        }
                        else if (IsCoordSeparator(currentChar))
                        {
                            state._newState = NumState.Separator;
                        }
                        else
                        {
                            switch (currentChar)
                            {
                                case '.':
                                    state._newState = NumState.DecPlace;
                                    break;
                                case '+':
                                case '-':
                                    state._newState = NumState.Prefix;
                                    break;
                                default:
                                    state._newState = NumState.Invalid;
                                    break;
                            }
                        }
                        break;
                }

                if (state._currState != NumState.Separator && state._newState < state._currState)
                {
#if NETSTANDARD2_1 || NETCORE || NETCOREAPP2_2 || NETCOREAPP3_0
                    result = float.Parse(chars.Slice(state._position, state._charsPosition - state._position), NumberStyles.Float, CultureInfo.InvariantCulture);
#else
                    result = float.Parse(chars.Slice(state._position, state._charsPosition - state._position).ToString(), NumberStyles.Float, CultureInfo.InvariantCulture);
#endif
                    state._position = state._charsPosition;
                    state._currState = state._newState;
                    return MarkState(true, ref state);
                }
                else if (state._newState != state._currState && state._currState == NumState.Separator)
                {
                    state._position = state._charsPosition;
                }

                if (state._newState == NumState.Invalid)
                {
                    result = float.MinValue;
                    return MarkState(false, ref state);
                }
                state._currState = state._newState;
                ++state._charsPosition;
            }

            if (state._currState == NumState.Separator || !state._hasMore || state._position >= charsLength)
            {
                result = float.MinValue;
                return MarkState(false, ref state);
            }
            else
            {
#if NETSTANDARD2_1 || NETCORE || NETCOREAPP2_2 || NETCOREAPP3_0
                result = float.Parse(chars.Slice(state._position, charsLength - state._position), NumberStyles.Float, CultureInfo.InvariantCulture);
#else
                result = float.Parse(chars.Slice(state._position, charsLength - state._position).ToString(), NumberStyles.Float, CultureInfo.InvariantCulture);
#endif
                state._position = charsLength;
                return MarkState(true, ref state);
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
