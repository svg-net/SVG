using System;
using System.Collections.Generic;

namespace Svg.Css;

internal interface IExCssSelectorOps<T>
{
    Func<IEnumerable<T>, IEnumerable<T>> Type(string name);
    Func<IEnumerable<T>, IEnumerable<T>> Universal();
    Func<IEnumerable<T>, IEnumerable<T>> Id(string id);
    Func<IEnumerable<T>, IEnumerable<T>> Class(string clazz);
    Func<IEnumerable<T>, IEnumerable<T>> AttributeExists(string name);
    Func<IEnumerable<T>, IEnumerable<T>> AttributeExact(string name, string value);
    Func<IEnumerable<T>, IEnumerable<T>> AttributeIncludes(string name, string value);
    Func<IEnumerable<T>, IEnumerable<T>> AttributeDashMatch(string name, string value);
    Func<IEnumerable<T>, IEnumerable<T>> AttributePrefixMatch(string name, string value);
    Func<IEnumerable<T>, IEnumerable<T>> AttributeSuffixMatch(string name, string value);
    Func<IEnumerable<T>, IEnumerable<T>> AttributeSubstring(string name, string value);
    Func<IEnumerable<T>, IEnumerable<T>> FirstChild();
    Func<IEnumerable<T>, IEnumerable<T>> LastChild();
    Func<IEnumerable<T>, IEnumerable<T>> NthChild(int step, int offset);
    Func<IEnumerable<T>, IEnumerable<T>> OnlyChild();
    Func<IEnumerable<T>, IEnumerable<T>> Empty();
    Func<IEnumerable<T>, IEnumerable<T>> Child();
    Func<IEnumerable<T>, IEnumerable<T>> Descendant();
    Func<IEnumerable<T>, IEnumerable<T>> Adjacent();
    Func<IEnumerable<T>, IEnumerable<T>> GeneralSibling();
    Func<IEnumerable<T>, IEnumerable<T>> NthLastChild(int step, int offset);
    Func<IEnumerable<SvgElement>, IEnumerable<SvgElement>> AttributeNotMatch(string name, string value);
    Func<IEnumerable<T>, IEnumerable<T>> NthType(int step, int offset);
    Func<IEnumerable<T>, IEnumerable<T>> NthLastType(int step, int offset);
}
