using Newtonsoft.Json;

namespace DefaultOnionArchitecture.Application.Exceptions;

public class ExceptionModel : ErrorStatusCode
{
    public IEnumerable<string> errors { get; set; }
    public override string ToString()
        => JsonConvert.SerializeObject(this);


}

public class ErrorStatusCode
{
    public int statusCode { get; set; }
    public bool isValid { get; set; }
}
