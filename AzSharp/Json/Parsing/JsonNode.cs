using System;
using System.Collections.Generic;
using System.IO;

namespace AzSharp.Json.Parsing;

public class JsonNode
{
    public static JsonNode SimpleTextLoad(string text)
    {
        JsonNode node = new();
        JsonError error = new();
        node.LoadText(text, error);
        return node;
    }
    private enum ListExpectation
    {
        OPEN_BRACKET,
        ELEMENT_OR_CLOSED_BRACKET,
        COMMA_OR_CLOSED_BRACKET,
    }
    private enum DictExpectation
    {
        OPEN_BRACKET,
        KEY_OR_CLOSED_BRACKET,
        SEMICOLON,
        ELEMENT,
        COMMA_OR_CLOSED_BRACKET
    }
    private class TextParseReturn
    {
        public string mString = "";
        public int mIndex;
        public TextParseReturn(string _string, int index)
        {
            mString = _string;
            mIndex = index;
        }
    }

    private JsonNodeType mType = JsonNodeType.MISSING;
    private string mString = string.Empty;
    private int mInt;
    private float mFloat;
    private bool mBool;
    private List<JsonNode>? mList;
    private Dictionary<string, JsonNode>? mDictionary;

    public JsonNodeType GetNodeType()
    {
        return mType;
    }
    public bool IsFailed()
    {
        return mType == JsonNodeType.FAILED;
    }

    public JsonNode() { }

    public JsonNode(JsonNode copy)
    {
        SetNodeType(copy.mType, false);
        switch (mType)
        {
            case JsonNodeType.LIST:
                List<JsonNode> new_list = new List<JsonNode>();
                foreach (JsonNode to_copy in copy.AsList())
                {
                    new_list.Add(new JsonNode(to_copy));
                }
                SetList(new_list);
                break;
            case JsonNodeType.DICTIONARY:
                Dictionary<string, JsonNode> neW_dict = new Dictionary<string, JsonNode>();
                foreach (KeyValuePair<string, JsonNode> pair in copy.AsDict())
                {
                    neW_dict.Add(pair.Key, new JsonNode(pair.Value));
                }
                SetDict(neW_dict);
                break;
            case JsonNodeType.INT:
                SetInt(copy.AsInt());
                break;
            case JsonNodeType.FLOAT:
                SetFloat(copy.AsFloat());
                break;
            case JsonNodeType.BOOL:
                SetBool(copy.AsBool());
                break;
            case JsonNodeType.STRING:
                SetString(copy.AsString());
                break;
        }
    }
    public JsonNode(JsonNodeType type)
    {
        SetNodeType(type, true);
    }
    public JsonNode(string _text)
    {
        SetString(_text);
    }
    public JsonNode(int _integer)
    {
        SetInt(_integer);
    }
    public JsonNode(float _floating)
    {
        SetFloat(_floating);
    }
    public JsonNode(bool _boolean)
    {
        SetBool(_boolean);
    }
    public JsonNode(List<JsonNode> _list)
    {
        SetList(_list);
    }
    public JsonNode(Dictionary<string, JsonNode> _dictionary)
    {
        SetDict(_dictionary);
    }
    private void SetNodeType(JsonNodeType new_type, bool set_default_value)
    {
        if (mType == new_type) { return; }
        // Null the references to lists or dictionaries to GC them if nessecary.
        mList = null;
        mDictionary = null;
        mType = new_type;
        // Set default values
        if (set_default_value)
            switch (mType)
            {
                case JsonNodeType.LIST:
                    {
                        mList = new List<JsonNode>();
                        break;
                    }
                case JsonNodeType.DICTIONARY:
                    {
                        mDictionary = new Dictionary<string, JsonNode>();
                        break;
                    }
                case JsonNodeType.INT:
                    {
                        mInt = 0;
                        break;
                    }
                case JsonNodeType.FLOAT:
                    {
                        mFloat = 0.0f;
                        break;
                    }
                case JsonNodeType.BOOL:
                    {
                        mBool = false;
                        break;
                    }
                case JsonNodeType.STRING:
                    {
                        mString = "";
                        break;
                    }
            }
    }

    private void AssertNodeType(JsonNodeType assert_type)
    {
        if (assert_type == mType)
        {
            return;
        }
        throw new InvalidOperationException($"Asserted json node of type {mType} is not desired type of {assert_type}");
    }
    public string AsString()
    {
        AssertNodeType(JsonNodeType.STRING);
        return mString;
    }
    public int AsInt()
    {
        AssertNodeType(JsonNodeType.INT);
        return mInt;
    }
    public float AsFloat()
    {
        AssertNodeType(JsonNodeType.FLOAT);
        return mFloat;
    }
    public bool AsBool()
    {
        AssertNodeType(JsonNodeType.BOOL);
        return mBool;
    }
    public List<JsonNode> AsList()
    {
        AssertNodeType(JsonNodeType.LIST);
        if (mList == null)
        {
            throw new InvalidOperationException("List is null while getting dictionary of json node");
        }
        return mList;
    }
    public Dictionary<string, JsonNode> AsDict()
    {
        AssertNodeType(JsonNodeType.DICTIONARY);
        if (mDictionary == null)
        {
            throw new InvalidOperationException("Dictionary is null while getting dictionary of json node");
        }
        return mDictionary;
    }
    public JsonNode DictNode(string key)
    {
        var dict = AsDict();
        if (!dict.ContainsKey(key))
        {
            throw new InvalidOperationException("Can't find a key of a json dictionary node");
        }
        return dict[key];
    }

    public void SetString(string _string)
    {
        SetNodeType(JsonNodeType.STRING, false);
        mString = _string;
    }
    public void SetInt(int _int)
    {
        SetNodeType(JsonNodeType.INT, false);
        mInt = _int;
    }
    public void SetFloat(float _float)
    {
        SetNodeType(JsonNodeType.FLOAT, false);
        mFloat = _float;
    }
    public void SetBool(bool _bool)
    {
        SetNodeType(JsonNodeType.BOOL, false);
        mBool = _bool;
    }
    public void SetList(List<JsonNode> _list)
    {
        SetNodeType(JsonNodeType.LIST, false);
        mList = _list;
    }
    public void SetDict(Dictionary<string, JsonNode> _dict)
    {
        SetNodeType(JsonNodeType.DICTIONARY, false);
        mDictionary = _dict;
    }
    public void SetNothing()
    {
        SetNodeType(JsonNodeType.NOTHING, false);
    }
    public void LoadFile(string path, JsonError error)
    {
        if (!File.Exists(path))
        {
            Fail(error, JsonError.ErrorType.IO_ERROR, "Failed to find file at path: " + path);
            return;
        }
        string text = File.ReadAllText(path);
        if (text == null || text.Length == 0)
        {
            Fail(error, JsonError.ErrorType.IO_ERROR, "Failed to load text from file path: " + path);
            return;
        }
        LoadText(text, error);
    }

    private bool LengthCheck(string text, int index, int len)
    {
        if (index + len >= text.Length) { return false; }
        return true;
    }

    public void LoadText(string text, JsonError error, bool treat_whitespace = true)
    {
        if (treat_whitespace)
        {
            bool in_string_element = false;
            int i = 0;
            // strip of whitespace in non string elements
            while (true)
            {
                if (i >= text.Length) { break; }
                char iterated_char = text[i];
                if (iterated_char == '"')
                {
                    in_string_element = !in_string_element;
                }
                else if (!in_string_element && char.IsWhiteSpace(iterated_char))
                {
                    // Erase whitespace if we're not in a string element
                    text = text.Remove(i, 1);
                    continue;
                }
                i++;
            }
        }
        BuildNode(text, 0, error);
    }

    private int BuildNode(string text, int index, JsonError error)
    {
        char first_char = text[index];
        if (char.IsDigit(first_char))
        {
            index = BuildNumber(text, index, error);
        }
        else
        {
            switch (first_char)
            {
                case 't':
                    {
                        index = BuildBool(text, index, error);
                        break;
                    }
                case 'f':
                    {
                        index = BuildBool(text, index, error);
                        break;
                    }
                case 'n':
                    {
                        index = BuildNothing(text, index, error);
                        break;
                    }
                case '-':
                    {
                        index = BuildNumber(text, index, error);
                        break;
                    }
                case '{':
                    {
                        index = BuildDictionary(text, index, error);
                        break;
                    }
                case '[':
                    {
                        index = BuildList(text, index, error);
                        break;
                    }
                case '"':
                    {
                        index = BuildString(text, index, error);
                        break;
                    }
                default:
                    {
                        Fail(error, JsonError.ErrorType.PARSE_ERROR, "Unable to find a build case while building node");
                        break;
                    }
            }
        }
        return index;
    }

    private int BuildNumber(string text, int index, JsonError error)
    {
        bool floaty = false;
        int start_index = index;
        bool outside_number_string = false;
        while (true)
        {
            if (index >= text.Length)
            {
                if (index == start_index)
                {
                    Fail(error, JsonError.ErrorType.PARSE_ERROR, "Tried to build a number with no valid characters.");
                    return index;
                }
                break;
            }
            char cur_char = text[index];
            if (!char.IsDigit(cur_char))
            {
                switch (cur_char)
                {
                    case '-':
                        {
                            if (index != start_index)
                            {
                                Fail(error, JsonError.ErrorType.PARSE_ERROR, "Non-front minus character found while building a number.");
                                return index;
                            }
                            break;
                        }
                    case '.':
                        {
                            if (index == start_index)
                            {
                                Fail(error, JsonError.ErrorType.PARSE_ERROR, "Front dot character found while building a number.");
                                return index;
                            }
                            if (floaty == true)
                            {
                                Fail(error, JsonError.ErrorType.PARSE_ERROR, "Multiple dot characters found while building a number.");
                                return index;
                            }
                            floaty = true;
                            break;
                        }
                    default:
                        {
                            outside_number_string = true;
                            break;
                        }
                }
            }
            if (outside_number_string) { break; }
            index++;
        }
        string substring = text.Substring(start_index, index - start_index);
        if (floaty)
        {
            SetFloat(float.Parse(substring));
        }
        else
        {
            SetInt(int.Parse(substring));
        }
        return index;
    }
    private int BuildBool(string text, int index, JsonError error)
    {
        bool bool_value = false;
        bool found_any = false;
        const int true_len = 4;
        const int false_len = 5;
        if (LengthCheck(text, index, true_len) && text.Substring(index, true_len) == "true")
        {
            bool_value = true;
            found_any = true;
            index += true_len;
        }
        else if (LengthCheck(text, index, false_len) && text.Substring(index, false_len) == "false")
        {
            bool_value = false;
            found_any = true;
            index += false_len;
        }
        if (!found_any)
        {
            Fail(error, JsonError.ErrorType.PARSE_ERROR, "Didn't find either a \"true\" or a \"false\" while building a bool value.");
            return index;
        }
        SetBool(bool_value);
        return index;
    }
    private int BuildNothing(string text, int index, JsonError error)
    {
        const int null_len = 4;
        if (!(LengthCheck(text, index, null_len) && text.Substring(index, null_len) == "null"))
        {
            Fail(error, JsonError.ErrorType.PARSE_ERROR, "Didn't find a \"null\" while building a null value.");
            return index;
        }
        SetNothing();
        index += null_len;
        return index;
    }
    private int BuildDictionary(string text, int index, JsonError error)
    {
        DictExpectation expect = DictExpectation.OPEN_BRACKET;
        bool loop = true;
        string key = "";
        Dictionary<string, JsonNode> built_dictionary = new Dictionary<string, JsonNode>();
        while (loop)
        {
            if (index >= text.Length)
            {
                Fail(error, JsonError.ErrorType.PARSE_ERROR, "Reached end of text while building a dictionary node.");
                return index;
            }
            char iterated_char = text[index];
            switch (expect)
            {
                case DictExpectation.OPEN_BRACKET:
                    {
                        if (iterated_char != '{')
                        {
                            Fail(error, JsonError.ErrorType.PARSE_ERROR, "Tried to build a dictionary without an open bracket at the start.");
                            return index;
                        }
                        index++;
                        expect = DictExpectation.KEY_OR_CLOSED_BRACKET;
                        break;
                    }
                case DictExpectation.KEY_OR_CLOSED_BRACKET:
                    {
                        if (iterated_char == '}')
                        {
                            loop = false;
                            index++;
                        }
                        else if (iterated_char == '"')
                        {
                            TextParseReturn parsed = ParseText(text, index, error);
                            index = parsed.mIndex;
                            if (error.Errored())
                            {
                                return index;
                            }
                            key = parsed.mString;
                            expect = DictExpectation.SEMICOLON;
                        }
                        else
                        {
                            Fail(error, JsonError.ErrorType.PARSE_ERROR, "Unhandled character while expecting a key start or dictionary close bracket.");
                            return index;
                        }
                        break;
                    }
                case DictExpectation.SEMICOLON:
                    {
                        if (iterated_char != ':')
                        {
                            Fail(error, JsonError.ErrorType.PARSE_ERROR, "Expected semicolon while parsing a dictionary, didnt get one.");
                            return index;
                        }
                        expect = DictExpectation.ELEMENT;
                        index++;
                        break;
                    }
                case DictExpectation.ELEMENT:
                    {
                        JsonNode new_node = new JsonNode();
                        index = new_node.BuildNode(text, index, error);
                        if (error.Errored())
                        {
                            return index;
                        }
                        built_dictionary[key] = new_node;
                        expect = DictExpectation.COMMA_OR_CLOSED_BRACKET;
                        break;
                    }
                case DictExpectation.COMMA_OR_CLOSED_BRACKET:
                    {
                        switch (iterated_char)
                        {
                            case ',':
                                {
                                    index++;
                                    expect = DictExpectation.KEY_OR_CLOSED_BRACKET;
                                    break;
                                }
                            case '}':
                                {
                                    loop = false;
                                    index++;
                                    break;
                                }
                            default:
                                {
                                    Fail(error, JsonError.ErrorType.PARSE_ERROR, "Didn't get a comma or a closed bracket when expecting one during dictionary building. Instead got at index of ");
                                    return index;
                                }
                        }
                        break;
                    }
            }
        }
        SetDict(built_dictionary);
        return index;
    }
    private int BuildList(string text, int index, JsonError error)
    {
        ListExpectation expect = ListExpectation.OPEN_BRACKET;
        bool loop = true;
        List<JsonNode> built_nodes = new List<JsonNode>();
        while (loop)
        {
            if (index >= text.Length)
            {
                Fail(error, JsonError.ErrorType.PARSE_ERROR, "Reached end of text while building a list node.");
                return index;
            }
            char iterated_char = text[index];
            switch (expect)
            {
                case ListExpectation.OPEN_BRACKET:
                    {
                        if (iterated_char != '[')
                        {
                            Fail(error, JsonError.ErrorType.PARSE_ERROR, "Tried to build a list that doesn't start with open bracket.");
                            return index;
                        }
                        expect = ListExpectation.ELEMENT_OR_CLOSED_BRACKET;
                        index++;
                        break;
                    }
                case ListExpectation.ELEMENT_OR_CLOSED_BRACKET:
                    {
                        if (iterated_char == ']')
                        {
                            loop = false;
                            index++;
                        }
                        else
                        {
                            JsonNode new_node = new JsonNode();
                            index = new_node.BuildNode(text, index, error);
                            if (error.Errored())
                            {
                                return index;
                            }
                            built_nodes.Add(new_node);
                            expect = ListExpectation.COMMA_OR_CLOSED_BRACKET;
                        }
                        break;
                    }
                case ListExpectation.COMMA_OR_CLOSED_BRACKET:
                    {
                        switch (iterated_char)
                        {
                            case ',':
                                {
                                    expect = ListExpectation.ELEMENT_OR_CLOSED_BRACKET;
                                    break;
                                }
                            case ']':
                                {
                                    loop = false;
                                    break;
                                }
                            default:
                                {
                                    Fail(error, JsonError.ErrorType.PARSE_ERROR, "Didn't find a comma or a close bracket after an element in built list.");
                                    return index;
                                }
                        }
                        index++;
                        break;
                    }
            }
        }
        SetList(built_nodes);
        return index;
    }
    private int BuildString(string text, int index, JsonError error)
    {
        TextParseReturn parsed = ParseText(text, index, error);
        if (error.Errored())
        {
            return parsed.mIndex;
        }
        SetString(parsed.mString);
        return parsed.mIndex;
    }
    private void Fail(JsonError error, JsonError.ErrorType error_type, string error_msg)
    {
        SetNodeType(JsonNodeType.FAILED, false);
        error.SetError(error_type, error_msg);
    }
    private string EscapeString(string to_escape)
    {
        string escaped_string = "";
        int index = 0;
        bool escaped_char = false;
        while (true)
        {
            if (index >= to_escape.Length)
            {
                break;
            }
            char iterated_char = to_escape[index];
            // Process an escaped character.
            if (escaped_char)
            {
                switch (iterated_char)
                {
                    case 'b':
                        {
                            escaped_string += '\b';
                            break;
                        }
                    case 'f':
                        {
                            escaped_string += '\f';
                            break;
                        }
                    case 'n':
                        {
                            escaped_string += '\n';
                            break;
                        }
                    case 'r':
                        {
                            escaped_string += '\r';
                            break;
                        }
                    case 't':
                        {
                            escaped_string += '\t';
                            break;
                        }
                    case '"':
                        {
                            escaped_string += '\"';
                            break;
                        }
                    case '\\':
                        {
                            escaped_string += '\\';
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                escaped_char = false;
            }
            // Process an unescaped character.
            else
            {
                switch (iterated_char)
                {
                    case '\\':
                        {
                            escaped_char = true;
                            break;
                        }
                    default:
                        {
                            escaped_string += iterated_char;
                            break;
                        }
                }
            }
            index++;
        }
        return escaped_string;
    }
    // Parses text and escapes the string, returning the index at which it ended at and the string in a struct
    private TextParseReturn ParseText(string text, int index, JsonError error)
    {
        TextParseReturn parse_return = new TextParseReturn("", index);
        int quotations = 0;
        bool escape = false;
        while (true)
        {
            if (parse_return.mIndex >= text.Length)
            {
                Fail(error, JsonError.ErrorType.PARSE_ERROR, "Reached end of text while parsing text.");
                return parse_return;
            }
            char iterated_char = text[parse_return.mIndex];
            // If an unescaped non-quotations character is not inbetween the first and second quotation.
            if (iterated_char != '"' && quotations != 1 && escape == false)
            {
                Fail(error, JsonError.ErrorType.PARSE_ERROR, "Tried to parse an unescaped non-quotation character outside of quotations while building a string node.");
                return parse_return;
            }
            switch (iterated_char)
            {
                case '"':
                    {
                        if (escape)
                        {
                            escape = false;
                            parse_return.mString += iterated_char;
                        }
                        else
                        {
                            quotations++;
                        }
                        break;
                    }
                case '\\':
                    {
                        escape = true;
                        parse_return.mString += iterated_char;
                        break;
                    }
                default:
                    {
                        escape = false;
                        parse_return.mString += iterated_char;
                        break;
                    }
            }
            parse_return.mIndex++;
            if (quotations >= 2) { break; }
        }
        parse_return.mString = EscapeString(parse_return.mString);
        return parse_return;
    }
    // Returns an unescaped string in quotations
    private string UnescapeAndQuoteString(string to_unescape)
    {
        string passed_string = "";
        //Unescape characters.
        int index = 0;
        while (true)
        {
            if (index >= to_unescape.Length) { break; }
            char string_char = to_unescape[index];
            switch (string_char)
            {
                case '\b':
                    {
                        passed_string += "\\b";
                        break;
                    }
                case '\f':
                    {
                        passed_string += "\\f";
                        break;
                    }
                case '\n':
                    {
                        passed_string += "\\n";
                        break;
                    }
                case '\r':
                    {
                        passed_string += "\\r";
                        break;
                    }
                case '\t':
                    {
                        passed_string += "\\t";
                        break;
                    }
                case '\"':
                    {
                        passed_string += "\\\"";
                        break;
                    }
                case '\\':
                    {
                        passed_string += "\\\\";
                        break;
                    }
                default:
                    {
                        passed_string += string_char;
                        break;
                    }
            }
            index++;
        }
        passed_string = "\"" + passed_string + "\"";
        return passed_string;
    }
    private string StringTabs(int amount)
    {
        string tabs = "";
        for (int i = 0; i < amount; i++)
        {
            tabs += '\t';
        }
        return tabs;
    }

    public string TurnIntoText(int tabs, bool make_indents, bool inlist)
    {
        string return_string = "";
        switch (mType)
        {
            case JsonNodeType.MISSING:
                {
                    return_string = "MISSING";
                    break;
                }
            case JsonNodeType.FAILED:
                {
                    return_string = "FAILED";
                    break;
                }
            case JsonNodeType.NOTHING:
                {
                    return_string = "null";
                    break;
                }
            case JsonNodeType.DICTIONARY:
                {
                    if (make_indents && !inlist)
                    {
                        if (tabs != 0) { return_string += '\n'; }
                        return_string += StringTabs(tabs);
                    }
                    return_string += "{";
                    tabs++;
                    if (mDictionary == null)
                    {
                        throw new InvalidOperationException("Dictionary was null during turining into text");
                    }
                    foreach (var pair in mDictionary)
                    {
                        if (make_indents)
                        {
                            return_string += '\n';
                            return_string += StringTabs(tabs);
                        }
                        return_string += UnescapeAndQuoteString(pair.Key);
                        return_string += ":";
                        if (make_indents) { return_string += " "; }
                        return_string += pair.Value.TurnIntoText(tabs, make_indents, false);
                        return_string += ",";
                    }
                    tabs--;
                    if (make_indents)
                    {
                        return_string += '\n';
                        return_string += StringTabs(tabs);
                    }
                    return_string += "}";
                    break;
                }
            case JsonNodeType.LIST:
                {
                    if (make_indents && !inlist)
                    {
                        if (tabs != 0) { return_string += '\n'; }
                        return_string += StringTabs(tabs);
                    }
                    return_string += "[";
                    tabs++;
                    if (mList == null)
                    {
                        throw new InvalidOperationException("List was null during turining into text");
                    }
                    foreach (var list_node in mList)
                    {
                        if (make_indents)
                        {
                            return_string += '\n';
                            return_string += StringTabs(tabs);
                        }
                        return_string += list_node.TurnIntoText(tabs, make_indents, true);
                        return_string += ",";
                    }
                    tabs--;
                    if (make_indents)
                    {
                        return_string += '\n';
                        return_string += StringTabs(tabs);
                    }
                    return_string += "]";
                    break;
                }
            case JsonNodeType.STRING:
                {
                    return_string = UnescapeAndQuoteString(mString);
                    break;
                }
            case JsonNodeType.INT:
                {
                    return_string = mInt.ToString();
                    break;
                }
            case JsonNodeType.FLOAT:
                {
                    return_string = mFloat.ToString("0.00");
                    break;
                }
            case JsonNodeType.BOOL:
                {
                    if (mBool == true)
                    {
                        return_string = "true";
                    }
                    else
                    {
                        return_string = "false";
                    }
                    break;
                }
        }
        return return_string;
    }

    public string ToText(bool make_indents)
    {
        return TurnIntoText(0, make_indents, false);
    }
    public void SaveFile(string path, bool make_indents = true)
    {
        File.WriteAllText(path, ToText(make_indents));
    }
}
