using System;
using System.Globalization;

namespace Svg
{
    public enum NumState
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

    public ref struct CoordinateParserState
    {
        public NumState CurrNumState;
        public NumState NewNumState;
        public int CharsPosition;
        public int Position;
        public bool HasMore;

        public CoordinateParserState(ref ReadOnlySpan<char> chars)
        {
            CurrNumState = NumState.Separator;
            NewNumState = NumState.Separator;
            CharsPosition = 0;
            Position = 0;
            HasMore = chars.Length > 0;
            if (char.IsLetter(chars[0])) ++CharsPosition;
        }
    }

    public static class CoordinateParser
    {
        private static bool MarkState(bool hasMode, ref CoordinateParserState state)
        {
            state.HasMore = hasMode;
            ++state.CharsPosition;
            return hasMode;
        }

        public static bool TryGetBool(out bool result, ref ReadOnlySpan<char> chars, ref CoordinateParserState state)
        {
            var charsLength = chars.Length;

            while (state.CharsPosition < charsLength && state.HasMore)
            {
                switch (state.CurrNumState)
                {
                    case NumState.Separator:
                        var currentChar = chars[state.CharsPosition];
                        if (IsCoordSeparator(currentChar))
                        {
                            state.NewNumState = NumState.Separator;
                        }
                        else if (currentChar == '0')
                        {
                            result = false;
                            state.NewNumState = NumState.Separator;
                            state.Position = state.CharsPosition + 1;
                            return MarkState(true, ref state);
                        }
                        else if (currentChar == '1')
                        {
                            result = true;
                            state.NewNumState = NumState.Separator;
                            state.Position = state.CharsPosition + 1;
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
                ++state.CharsPosition;
            }
            result = false;
            return MarkState(false, ref state);
        }

        public static bool TryGetFloat(out float result, ref ReadOnlySpan<char> chars, ref CoordinateParserState state)
        {
            var charsLength = chars.Length;

            while (state.CharsPosition < charsLength && state.HasMore)
            {
                var currentChar = chars[state.CharsPosition];

                switch (state.CurrNumState)
                {
                    case NumState.Separator:
                        if (char.IsNumber(currentChar))
                        {
                            state.NewNumState = NumState.Integer;
                        }
                        else if (IsCoordSeparator(currentChar))
                        {
                            state.NewNumState = NumState.Separator;
                        }
                        else
                        {
                            switch (currentChar)
                            {
                                case '.':
                                    state.NewNumState = NumState.DecPlace;
                                    break;
                                case '+':
                                case '-':
                                    state.NewNumState = NumState.Prefix;
                                    break;
                                default:
                                    state.NewNumState = NumState.Invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.Prefix:
                        if (char.IsNumber(currentChar))
                        {
                            state.NewNumState = NumState.Integer;
                        }
                        else if (currentChar == '.')
                        {
                            state.NewNumState = NumState.DecPlace;
                        }
                        else
                        {
                            state.NewNumState = NumState.Invalid;
                        }
                        break;
                    case NumState.Integer:
                        if (char.IsNumber(currentChar))
                        {
                            state.NewNumState = NumState.Integer;
                        }
                        else if (IsCoordSeparator(currentChar))
                        {
                            state.NewNumState = NumState.Separator;
                        }
                        else
                        {
                            switch (currentChar)
                            {
                                case '.':
                                    state.NewNumState = NumState.DecPlace;
                                    break;
                                case 'E':
                                case 'e':
                                    state.NewNumState = NumState.Exponent;
                                    break;
                                case '+':
                                case '-':
                                    state.NewNumState = NumState.Prefix;
                                    break;
                                default:
                                    state.NewNumState = NumState.Invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.DecPlace:
                        if (char.IsNumber(currentChar))
                        {
                            state.NewNumState = NumState.Fraction;
                        }
                        else if (IsCoordSeparator(currentChar))
                        {
                            state.NewNumState = NumState.Separator;
                        }
                        else
                        {
                            switch (currentChar)
                            {
                                case 'E':
                                case 'e':
                                    state.NewNumState = NumState.Exponent;
                                    break;
                                case '+':
                                case '-':
                                    state.NewNumState = NumState.Prefix;
                                    break;
                                default:
                                    state.NewNumState = NumState.Invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.Fraction:
                        if (char.IsNumber(currentChar))
                        {
                            state.NewNumState = NumState.Fraction;
                        }
                        else if (IsCoordSeparator(currentChar))
                        {
                            state.NewNumState = NumState.Separator;
                        }
                        else
                        {
                            switch (currentChar)
                            {
                                case '.':
                                    state.NewNumState = NumState.DecPlace;
                                    break;
                                case 'E':
                                case 'e':
                                    state.NewNumState = NumState.Exponent;
                                    break;
                                case '+':
                                case '-':
                                    state.NewNumState = NumState.Prefix;
                                    break;
                                default:
                                    state.NewNumState = NumState.Invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.Exponent:
                        if (char.IsNumber(currentChar))
                        {
                            state.NewNumState = NumState.ExpValue;
                        }
                        else if (IsCoordSeparator(currentChar))
                        {
                            state.NewNumState = NumState.Invalid;
                        }
                        else
                        {
                            switch (currentChar)
                            {
                                case '+':
                                case '-':
                                    state.NewNumState = NumState.ExpPrefix;
                                    break;
                                default:
                                    state.NewNumState = NumState.Invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.ExpPrefix:
                        if (char.IsNumber(currentChar))
                        {
                            state.NewNumState = NumState.ExpValue;
                        }
                        else
                        {
                            state.NewNumState = NumState.Invalid;
                        }
                        break;
                    case NumState.ExpValue:
                        if (char.IsNumber(currentChar))
                        {
                            state.NewNumState = NumState.ExpValue;
                        }
                        else if (IsCoordSeparator(currentChar))
                        {
                            state.NewNumState = NumState.Separator;
                        }
                        else
                        {
                            switch (currentChar)
                            {
                                case '.':
                                    state.NewNumState = NumState.DecPlace;
                                    break;
                                case '+':
                                case '-':
                                    state.NewNumState = NumState.Prefix;
                                    break;
                                default:
                                    state.NewNumState = NumState.Invalid;
                                    break;
                            }
                        }
                        break;
                }

                if (state.CurrNumState != NumState.Separator && state.NewNumState < state.CurrNumState)
                {
#if NETSTANDARD2_1 || NETCORE || NETCOREAPP2_2 || NETCOREAPP3_0
                    result = float.Parse(chars.Slice(state.Position, state.CharsPosition - state.Position), NumberStyles.Float, CultureInfo.InvariantCulture);
#else
                    result = float.Parse(chars.Slice(state.Position, state.CharsPosition - state.Position).ToString(), NumberStyles.Float, CultureInfo.InvariantCulture);
#endif
                    state.Position = state.CharsPosition;
                    state.CurrNumState = state.NewNumState;
                    return MarkState(true, ref state);
                }
                else if (state.NewNumState != state.CurrNumState && state.CurrNumState == NumState.Separator)
                {
                    state.Position = state.CharsPosition;
                }

                if (state.NewNumState == NumState.Invalid)
                {
                    result = float.MinValue;
                    return MarkState(false, ref state);
                }
                state.CurrNumState = state.NewNumState;
                ++state.CharsPosition;
            }

            if (state.CurrNumState == NumState.Separator || !state.HasMore || state.Position >= charsLength)
            {
                result = float.MinValue;
                return MarkState(false, ref state);
            }
            else
            {
#if NETSTANDARD2_1 || NETCORE || NETCOREAPP2_2 || NETCOREAPP3_0
                result = float.Parse(chars.Slice(state.Position, charsLength - state.Position), NumberStyles.Float, CultureInfo.InvariantCulture);
#else
                result = float.Parse(chars.Slice(state.Position, charsLength - state.Position).ToString(), NumberStyles.Float, CultureInfo.InvariantCulture);
#endif
                state.Position = charsLength;
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
