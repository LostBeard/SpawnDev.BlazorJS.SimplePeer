using Microsoft.JSInterop;
using SpawnDev.BlazorJS.JSObjects;

namespace SpawnDev.BlazorJS.SimplePeer
{
    /// <summary>
    /// A generic JavaScript &lt;Error&gt; object that does not denote any specific circumstance of why the error occurred. Error objects capture a "stack trace" detailing the point in the code at which the Error was instantiated, and may provide a text description of the error.<br/>
    /// https://nodejs.org/api/errors.html#class-error
    /// </summary>
    public class NodeError : Error
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public NodeError(IJSInProcessObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// The error.code property is a string label that identifies the kind of error. error.code is the most stable way to identify an error. It will only change between major versions of Node.js. In contrast, error.message strings may change between any versions of Node.js. See Node.js error codes for details about specific codes.
        /// </summary>
        public string? Code => JSRef!.Get<string?>("code");
    }
}
