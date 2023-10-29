namespace AzSharp.Json.Parsing;
public class JsonError
{
    public enum ErrorType
    {
        IO_ERROR,
        PARSE_ERROR,
    }
    public bool mErrored = false;
    public ErrorType mErrorType = ErrorType.IO_ERROR;
    public string mErrorMsg = "";

    public void SetError(ErrorType error_type, string error_msg)
    {
        mErrored = true;
        mErrorType = error_type;
        mErrorMsg = error_msg;
    }
    public void Reset()
    {
        mErrored = false;
    }
    public bool Errored()
    {
        return mErrored;
    }
    public string GetErrorMsg()
    {
        return $"JSON ERROR: {mErrorMsg} Error type: {mErrorType}.";
    }
}