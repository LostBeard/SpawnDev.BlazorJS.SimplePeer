using Microsoft.JSInterop;
using SpawnDev.BlazorJS.JSObjects;

namespace SpawnDev.BlazorJS.SimplePeer
{
    /// <summary>
    /// The buffer module from node.js, for the browser.<br/>
    /// As this class is packaged inside SimplePeer the class name is usually changed to a more minimal version and not publicly available.
    /// https://github.com/feross/buffer
    /// </summary>
    public class NodeBuffer : TypedArray<byte>
    {
        /// <summary>
        /// Returns a copy of the Javascript typed array as a .Net array
        /// </summary>
        /// <param name="values"></param>
        public static explicit operator byte[]?(NodeBuffer? values) => values == null ? null : values.ToArray();
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public NodeBuffer(IJSInProcessObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Extracts a section of an array and returns a new array. See also Array.prototype.slice().
        /// </summary>
        /// <returns></returns>
        public NodeBuffer Slice() => JSRef!.Call<NodeBuffer>("slice");
        /// <summary>
        /// Extracts a section of an array and returns a new array. See also Array.prototype.slice().
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public NodeBuffer Slice(long start) => JSRef!.Call<NodeBuffer>("slice", start);
        /// <summary>
        /// Extracts a section of an array and returns a new array. See also Array.prototype.slice().
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public NodeBuffer Slice(long start, long end) => JSRef!.Call<NodeBuffer>("slice", start, end);
        /// <summary>
        /// The subarray() method of TypedArray instances returns a new typed array on the same ArrayBuffer store and with the same element types as this typed array. The begin offset is inclusive and the end offset is exclusive.
        /// </summary>
        /// <returns></returns>
        public NodeBuffer SubArray() => JSRef!.Call<NodeBuffer>("subarray");
        /// <summary>
        /// The subarray() method of TypedArray instances returns a new typed array on the same ArrayBuffer store and with the same element types as this typed array. The begin offset is inclusive and the end offset is exclusive.
        /// </summary>
        /// <param name="start">Element to begin at. The offset is inclusive. The whole array will be included in the new view if this value is not specified.</param>
        /// <returns></returns>
        public NodeBuffer SubArray(long start) => JSRef!.Call<NodeBuffer>("subarray", start);
        /// <summary>
        /// The subarray() method of TypedArray instances returns a new typed array on the same ArrayBuffer store and with the same element types as this typed array. The begin offset is inclusive and the end offset is exclusive.
        /// </summary>
        /// <param name="start">Element to begin at. The offset is inclusive. The whole array will be included in the new view if this value is not specified.</param>
        /// <param name="end">Element to end at. The offset is exclusive. If not specified, all elements from the one specified by begin to the end of the array are included in the new view.</param>
        /// <returns></returns>
        public NodeBuffer SubArray(long start, long end) => JSRef!.Call<NodeBuffer>("subarray", start, end);
    }
}
