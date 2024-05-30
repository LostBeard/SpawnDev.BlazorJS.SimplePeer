using Microsoft.JSInterop;

namespace SpawnDev.BlazorJS.SimplePeer
{
    /// <summary>
    /// Duplex streams are streams that implement both the Readable and Writable interfaces.<br/>
    /// https://nodejs.org/api/stream.html#class-streamduplex<br/>
    /// https://nodejs.org/api/stream.html#class-streamreadable<br/>
    /// https://nodejs.org/api/stream.html#class-streamwritable
    /// </summary>
    public class Duplex : EventEmitter
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public Duplex(IJSInProcessObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// If false then the stream will automatically end the writable side when the readable side ends. Set initially by the allowHalfOpen constructor option, which defaults to true.
        /// </summary>
        public bool AllowHalfOpen => JSRef!.Get<bool>("allowHalfOpen");
        /// <summary>
        /// Is true after 'close' has been emitted.
        /// </summary>
        //public bool Closed => JSRef!.Get<bool>("closed");
        /// <summary>
        /// Is true after readable.destroy() has been called.
        /// </summary>
        public bool Destroyed => JSRef!.Get<bool>("destroyed");
        /// <summary>
        /// Returns error if the stream has been destroyed with an error.
        /// </summary>
        public NodeError? Errored => JSRef!.Get<NodeError?>("errored");
        /// <summary>
        /// Destroy the stream. Optionally emit an 'error' event, and emit a 'close' event (unless emitClose is set to false). After this call, the writable stream has ended and subsequent calls to write() or end() will result in an ERR_STREAM_DESTROYED error. This is a destructive and immediate way to destroy a stream. Previous calls to write() may not have drained, and may trigger an ERR_STREAM_DESTROYED error. Use end() instead of destroy if data should flush before close, or wait for the 'drain' event before destroying the stream.
        /// </summary>
        public void Destroy() => JSRef!.CallVoid("destroy");
        /// <summary>
        /// Destroy the stream. Optionally emit an 'error' event, and emit a 'close' event (unless emitClose is set to false). After this call, the writable stream has ended and subsequent calls to write() or end() will result in an ERR_STREAM_DESTROYED error. This is a destructive and immediate way to destroy a stream. Previous calls to write() may not have drained, and may trigger an ERR_STREAM_DESTROYED error. Use end() instead of destroy if data should flush before close, or wait for the 'drain' event before destroying the stream.
        /// </summary>
        /// <param name="error"></param>
        public void Destroy(NodeError error) => JSRef!.CallVoid("destroy", error);
        /// <summary>
        /// The 'error' event may be emitted by a Readable implementation at any time. Typically, this may occur if the underlying stream is unable to generate data due to an underlying internal failure, or when a stream implementation attempts to push an invalid chunk of data.
        /// </summary>
        public JSEventCallback<NodeError> OnError { get => new JSEventCallback<NodeError>("error", On, RemoveListener); set { } }
        /// <summary>
        /// The 'close' event is emitted when the stream and any of its underlying resources (a file descriptor, for example) have been closed. The event indicates that no more events will be emitted, and no further computation will occur.
        /// </summary>
        public JSEventCallback OnClose { get => new JSEventCallback("close", On, RemoveListener); set { } }
        #region Writable
        /// <summary>
        /// Is true if it is safe to call writable.write(), which means the stream has not been destroyed, errored, or ended.
        /// </summary>
        public bool Writable => JSRef!.Get<bool>("writable");
        /// <summary>
        /// Is true after writable.end() has been called. This property does not indicate whether the data has been flushed, for this use writable.writableFinished instead.
        /// </summary>
        //public bool WritableEnded => JSRef!.Get<bool>("writableEnded");
        /// <summary>
        /// Number of times writable.uncork() needs to be called in order to fully uncork the stream.
        /// </summary>
        //public bool WritableCorked => JSRef!.Get<bool>("writableCorked");
        /// <summary>
        /// Is set to true immediately before the 'finish' event is emitted.
        /// </summary>
        //public bool WritableFinished => JSRef!.Get<bool>("writableFinished");
        /// <summary>
        /// Is true if the stream's buffer has been full and stream will emit 'drain'.
        /// </summary>
        //public bool WritableNeedDrain => JSRef!.Get<bool>("writableNeedDrain");
        /// <summary>
        /// Return the value of highWaterMark passed when creating this Writable.
        /// </summary>
        public int WritableHighWaterMark => JSRef!.Get<int>("writableHighWaterMark");
        /// <summary>
        /// This property contains the number of bytes (or objects) in the queue ready to be written. The value provides introspection data regarding the status of the highWaterMark.
        /// </summary>
        public int WritableLength => JSRef!.Get<int>("writableLength");
        /// <summary>
        /// The writable.cork() method forces all written data to be buffered in memory. The buffered data will be flushed when either the stream.uncork() or stream.end() methods are called.
        /// </summary>
        public void Cork() => JSRef!.CallVoid("cork");
        /// <summary>
        /// The writable.uncork() method flushes all data buffered since stream.cork() was called.<br/>
        /// When using writable.cork() and writable.uncork() to manage the buffering of writes to a stream, defer calls to writable.uncork() using process.nextTick(). Doing so allows batching of all writable.write() calls that occur within a given Node.js event loop phase.<br/>
        /// If the writable.cork() method is called multiple times on a stream, the same number of calls to writable.uncork() must be called to flush the buffered data.<br/>
        /// </summary>
        public void Uncork() => JSRef!.CallVoid("uncork");
        /// <summary>
        /// Calling the writable.end() method signals that no more data will be written to the Writable. The optional chunk and encoding arguments allow one final additional chunk of data to be written immediately before closing the stream.
        /// </summary>
        /// <param name="chunk">
        /// chunk &lt;string> | &lt;byte[]> | &lt;NodeBuffer> | &lt;TypedArray> | &lt;DataView> | &lt;any> Optional data to write. For streams not operating in object mode, chunk must be a &lt;string>, &lt;Buffer>, &lt;TypedArray> or &lt;DataView>. For object mode streams, chunk may be any JavaScript value other than null.
        /// </param>
        /// <param name="encoding">encoding &lt;string> | &lt;null> The encoding, if chunk is a string. Default: 'utf8'</param>
        /// <param name="callback">callback &lt;Function> Callback for when this chunk of data is flushed.</param>
        public void End(string chunk, string? encoding, Action callback) => JSRef!.CallVoid("end", chunk, encoding, Callback.CreateOne(callback));
        /// <summary>
        /// Calling the writable.end() method signals that no more data will be written to the Writable. The optional chunk and encoding arguments allow one final additional chunk of data to be written immediately before closing the stream.
        /// </summary>
        /// <param name="chunk">
        /// chunk &lt;string> | &lt;byte[]> | &lt;NodeBuffer> | &lt;TypedArray> | &lt;DataView> | &lt;any> Optional data to write. For streams not operating in object mode, chunk must be a &lt;string>, &lt;Buffer>, &lt;TypedArray> or &lt;DataView>. For object mode streams, chunk may be any JavaScript value other than null.
        /// </param>
        /// <param name="callback">callback &lt;Function> Callback for when this chunk of data is flushed.</param>
        public void End(object chunk, Action callback) => JSRef!.CallVoid("end", chunk, Callback.CreateOne(callback));
        /// <summary>
        /// Calling the writable.end() method signals that no more data will be written to the Writable. The optional chunk and encoding arguments allow one final additional chunk of data to be written immediately before closing the stream.
        /// </summary>
        /// <param name="chunk">
        /// chunk &lt;string> | &lt;byte[]> | &lt;NodeBuffer> | &lt;TypedArray> | &lt;DataView> | &lt;any> Optional data to write. For streams not operating in object mode, chunk must be a &lt;string>, &lt;Buffer>, &lt;TypedArray> or &lt;DataView>. For object mode streams, chunk may be any JavaScript value other than null.
        /// </param>
        public void End(object chunk) => JSRef!.CallVoid("end", chunk);
        /// <summary>
        /// Calling the writable.end() method signals that no more data will be written to the Writable. The optional chunk and encoding arguments allow one final additional chunk of data to be written immediately before closing the stream.
        /// </summary>
        /// <param name="callback">callback &lt;Function> Callback for when this chunk of data is flushed.</param>
        public void End(Action callback) => JSRef!.CallVoid("end", Callback.CreateOne(callback));
        /// <summary>
        /// Calling the writable.end() method signals that no more data will be written to the Writable. The optional chunk and encoding arguments allow one final additional chunk of data to be written immediately before closing the stream.
        /// </summary>
        public void End() => JSRef!.CallVoid("end");
        /// <summary>
        /// The writable.write() method writes some data to the stream, and calls the supplied callback once the data has been fully handled. If an error occurs, the callback will be called with the error as its first argument. The callback is called asynchronously and before 'error' is emitted.
        /// </summary>
        /// <param name="chunk">
        /// chunk &lt;string> | &lt;byte[]> | &lt;NodeBuffer> | &lt;TypedArray> | &lt;DataView> | &lt;any> Optional data to write. For streams not operating in object mode, chunk must be a &lt;string>, &lt;Buffer>, &lt;TypedArray> or &lt;DataView>. For object mode streams, chunk may be any JavaScript value other than null.
        /// </param>
        /// <param name="encoding">encoding &lt;string> | &lt;null> The encoding, if chunk is a string. Default: 'utf8'</param>
        /// <param name="callback">callback &lt;Function> Callback for when this chunk of data is flushed.</param>
        /// <returns>Returns: bool false if the stream wishes for the calling code to wait for the 'drain' event to be emitted before continuing to write additional data; otherwise true.</returns>
        public bool Write(string chunk, string? encoding, Action callback) => JSRef!.Call<bool>("write", chunk, Callback.CreateOne(callback));
        /// <summary>
        /// The writable.write() method writes some data to the stream, and calls the supplied callback once the data has been fully handled. If an error occurs, the callback will be called with the error as its first argument. The callback is called asynchronously and before 'error' is emitted.
        /// </summary>
        /// <param name="chunk">
        /// chunk &lt;string> | &lt;byte[]> | &lt;NodeBuffer> | &lt;TypedArray> | &lt;DataView> | &lt;any> Optional data to write. For streams not operating in object mode, chunk must be a &lt;string>, &lt;Buffer>, &lt;TypedArray> or &lt;DataView>. For object mode streams, chunk may be any JavaScript value other than null.
        /// </param>
        /// <param name="callback">callback &lt;Function> Callback for when this chunk of data is flushed.</param>
        /// <returns>Returns: bool false if the stream wishes for the calling code to wait for the 'drain' event to be emitted before continuing to write additional data; otherwise true.</returns>
        public bool Write(object chunk, Action callback) => JSRef!.Call<bool>("write", chunk, Callback.CreateOne(callback));
        /// <summary>
        /// The writable.write() method writes some data to the stream, and calls the supplied callback once the data has been fully handled. If an error occurs, the callback will be called with the error as its first argument. The callback is called asynchronously and before 'error' is emitted.
        /// </summary>
        /// <param name="chunk">
        /// chunk &lt;string> | &lt;byte[]> | &lt;NodeBuffer> | &lt;TypedArray> | &lt;DataView> | &lt;any> Optional data to write. For streams not operating in object mode, chunk must be a &lt;string>, &lt;Buffer>, &lt;TypedArray> or &lt;DataView>. For object mode streams, chunk may be any JavaScript value other than null.
        /// </param>
        /// <returns>Returns: bool false if the stream wishes for the calling code to wait for the 'drain' event to be emitted before continuing to write additional data; otherwise true.</returns>
        public bool Write(object chunk) => JSRef!.Call<bool>("write", chunk);
        /// <summary>
        /// The writable.setDefaultEncoding() method sets the default encoding for a Writable stream.
        /// </summary>
        /// <param name="encoding"></param>
        public void SetDefaultEncoding(string encoding) => JSRef!.CallVoid("setDefaultEncoding", encoding);
        /// <summary>
        /// If a call to stream.write(chunk) returns false, the 'drain' event will be emitted when it is appropriate to resume writing data to the stream.
        /// </summary>
        public JSEventCallback OnDrain { get => new JSEventCallback("drain", On, RemoveListener); set { } }
        /// <summary>
        /// The 'finish' event is emitted after the stream.end() method has been called, and all data has been flushed to the underlying system.
        /// </summary>
        public JSEventCallback OnFinish { get => new JSEventCallback("finish", On, RemoveListener); set { } }
        #endregion

        #region Readable
        /// <summary>
        /// The readable.setEncoding() method sets the character encoding for data read from the Readable stream.
        /// </summary>
        /// <param name="encoding"></param>
        public void SetEncoding(string encoding) => JSRef!.CallVoid("setEncoding", encoding);
        /// <summary>
        /// The readable.isPaused() method returns the current operating state of the Readable. This is used primarily by the mechanism that underlies the readable.pipe() method. In most typical cases, there will be no reason to use this method directly.
        /// </summary>
        /// <returns></returns>
        public bool IsPaused() => JSRef!.Call<bool>("isPaused");
        /// <summary>
        /// The readable.pause() method will cause a stream in flowing mode to stop emitting 'data' events, switching out of flowing mode. Any data that becomes available will remain in the internal buffer.<br/>
        /// The readable.pause() method has no effect if there is a 'readable' event listener.
        /// </summary>
        public void Pause() => JSRef!.CallVoid("pause");
        /// <summary>
        /// The readable.read() method reads data out of the internal buffer and returns it. If no data is available to be read, null is returned. By default, the data is returned as a Buffer object unless an encoding has been specified using the readable.setEncoding() method or the stream is operating in object mode.<br/>
        /// The optional size argument specifies a specific number of bytes to read. If size bytes are not available to be read, null will be returned unless the stream has ended, in which case all of the data remaining in the internal buffer will be returned.<br/>
        /// If the size argument is not specified, all of the data contained in the internal buffer will be returned.<br/>
        /// The size argument must be less than or equal to 1 GiB.<br/>
        /// The readable.read() method should only be called on Readable streams operating in paused mode. In flowing mode, readable.read() is called automatically until the internal buffer is fully drained.<br/>
        /// Each call to readable.read() returns a chunk of data, or null. The chunks are not concatenated. A while loop is necessary to consume all data currently in the buffer. When reading a large file .read() may return null, having consumed all buffered content so far, but there is still more data to come not yet buffered. In this case a new 'readable' event will be emitted when there is more data in the buffer. Finally the 'end' event will be emitted when there is no more data to come.
        /// </summary>
        /// <returns></returns>
        public NodeBuffer? Read() => JSRef!.Call<NodeBuffer?>("read");
        /// <summary>
        /// The readable.read() method reads data out of the internal buffer and returns it. If no data is available to be read, null is returned. By default, the data is returned as a Buffer object unless an encoding has been specified using the readable.setEncoding() method or the stream is operating in object mode.<br/>
        /// The optional size argument specifies a specific number of bytes to read. If size bytes are not available to be read, null will be returned unless the stream has ended, in which case all of the data remaining in the internal buffer will be returned.<br/>
        /// If the size argument is not specified, all of the data contained in the internal buffer will be returned.<br/>
        /// The size argument must be less than or equal to 1 GiB.<br/>
        /// The readable.read() method should only be called on Readable streams operating in paused mode. In flowing mode, readable.read() is called automatically until the internal buffer is fully drained.<br/>
        /// Each call to readable.read() returns a chunk of data, or null. The chunks are not concatenated. A while loop is necessary to consume all data currently in the buffer. When reading a large file .read() may return null, having consumed all buffered content so far, but there is still more data to come not yet buffered. In this case a new 'readable' event will be emitted when there is more data in the buffer. Finally the 'end' event will be emitted when there is no more data to come.
        /// </summary>
        /// <returns></returns>
        public NodeBuffer? Read(int size) => JSRef!.Call<NodeBuffer?>("read", size);
        /// <summary>
        /// The readable.read() method reads data out of the internal buffer and returns it. If no data is available to be read, null is returned. By default, the data is returned as a Buffer object unless an encoding has been specified using the readable.setEncoding() method or the stream is operating in object mode.<br/>
        /// The optional size argument specifies a specific number of bytes to read. If size bytes are not available to be read, null will be returned unless the stream has ended, in which case all of the data remaining in the internal buffer will be returned.<br/>
        /// If the size argument is not specified, all of the data contained in the internal buffer will be returned.<br/>
        /// The size argument must be less than or equal to 1 GiB.<br/>
        /// The readable.read() method should only be called on Readable streams operating in paused mode. In flowing mode, readable.read() is called automatically until the internal buffer is fully drained.<br/>
        /// Each call to readable.read() returns a chunk of data, or null. The chunks are not concatenated. A while loop is necessary to consume all data currently in the buffer. When reading a large file .read() may return null, having consumed all buffered content so far, but there is still more data to come not yet buffered. In this case a new 'readable' event will be emitted when there is more data in the buffer. Finally the 'end' event will be emitted when there is no more data to come.
        /// </summary>
        /// <returns></returns>
        public T Read<T>() => JSRef!.Call<T>("read");
        /// <summary>
        /// The readable.read() method reads data out of the internal buffer and returns it. If no data is available to be read, null is returned. By default, the data is returned as a Buffer object unless an encoding has been specified using the readable.setEncoding() method or the stream is operating in object mode.<br/>
        /// The optional size argument specifies a specific number of bytes to read. If size bytes are not available to be read, null will be returned unless the stream has ended, in which case all of the data remaining in the internal buffer will be returned.<br/>
        /// If the size argument is not specified, all of the data contained in the internal buffer will be returned.<br/>
        /// The size argument must be less than or equal to 1 GiB.<br/>
        /// The readable.read() method should only be called on Readable streams operating in paused mode. In flowing mode, readable.read() is called automatically until the internal buffer is fully drained.<br/>
        /// Each call to readable.read() returns a chunk of data, or null. The chunks are not concatenated. A while loop is necessary to consume all data currently in the buffer. When reading a large file .read() may return null, having consumed all buffered content so far, but there is still more data to come not yet buffered. In this case a new 'readable' event will be emitted when there is more data in the buffer. Finally the 'end' event will be emitted when there is no more data to come.
        /// </summary>
        /// <returns></returns>
        public T Read<T>(int size) => JSRef!.Call<T>("read", size);
        /// <summary>
        /// Is true if it is safe to call readable.read(), which means the stream has not been destroyed or emitted 'error' or 'end'.
        /// </summary>
        public bool Readable => JSRef!.Get<bool>("readable");
        /// <summary>
        /// Getter for the property encoding of a given Readable stream. The encoding property can be set using the readable.setEncoding() method.
        /// </summary>
        //public string? ReadableEncoding => JSRef!.Get<string?>("readableEncoding");
        /// <summary>
        /// Becomes true when 'end' event is emitted.
        /// </summary>
        //public bool ReadableEnded => JSRef!.Get<bool>("readableEnded");
        /// <summary>
        /// This property reflects the current state of a Readable stream as described in the Three states section.
        /// </summary>
        public bool ReadableFlowing => JSRef!.Get<bool>("readableFlowing");
        /// <summary>
        /// Returns the value of highWaterMark passed when creating this Readable.
        /// </summary>
        public int ReadableHighWaterMark => JSRef!.Get<int>("readableHighWaterMark");
        /// <summary>
        /// This property contains the number of bytes (or objects) in the queue ready to be read. The value provides introspection data regarding the status of the highWaterMark.
        /// </summary>
        public int ReadableLength => JSRef!.Get<int>("readableLength");
        /// <summary>
        /// chunk &lt;Buffer> | &lt;string> | &lt;any> The chunk of data. For streams that are not operating in object mode, the chunk will be either a string or Buffer. For streams that are in object mode, the chunk can be any JavaScript value other than null.<br/>
        /// The 'data' event is emitted whenever the stream is relinquishing ownership of a chunk of data to a consumer. This may occur whenever the stream is switched in flowing mode by calling readable.pipe(), readable.resume(), or by attaching a listener callback to the 'data' event. The 'data' event will also be emitted whenever the readable.read() method is called and a chunk of data is available to be returned.<br/>
        /// Attaching a 'data' event listener to a stream that has not been explicitly paused will switch the stream into flowing mode. Data will then be passed as soon as it is available.<br/>
        /// The listener callback will be passed the chunk of data as a string if a default encoding has been specified for the stream using the readable.setEncoding() method; otherwise the data will be passed as a Buffer.
        /// </summary>
        public JSEventCallback<NodeBuffer> OnData { get => new JSEventCallback<NodeBuffer>("data", On, RemoveListener); set { } }
        /// <summary>
        /// The 'end' event is emitted when there is no more data to be consumed from the stream.<br/>
        /// The 'end' event will not be emitted unless the data is completely consumed. This can be accomplished by switching the stream into flowing mode, or by calling stream.read() repeatedly until all data has been consumed.
        /// </summary>
        public JSEventCallback OnEnd { get => new JSEventCallback("end", On, RemoveListener); set { } }
        /// <summary>
        /// The 'pause' event is emitted when stream.pause() is called and readableFlowing is not false.
        /// </summary>
        public JSEventCallback OnPause { get => new JSEventCallback("pause", On, RemoveListener); set { } }
        /// <summary>
        /// The 'readable' event is emitted when there is data available to be read from the stream or when the end of the stream has been reached. Effectively, the 'readable' event indicates that the stream has new information. If data is available, stream.read() will return that data.<br/>
        /// If the end of the stream has been reached, calling stream.read() will return null and trigger the 'end' event. This is also true if there never was any data to be read. For instance, in the following example, foo.txt is an empty file:<br/>
        /// In some cases, attaching a listener for the 'readable' event will cause some amount of data to be read into an internal buffer.<br/>
        /// In general, the readable.pipe() and 'data' event mechanisms are easier to understand than the 'readable' event. However, handling 'readable' might result in increased throughput.<br/>
        /// If both 'readable' and 'data' are used at the same time, 'readable' takes precedence in controlling the flow, i.e. 'data' will be emitted only when stream.read() is called. The readableFlowing property would become false. If there are 'data' listeners when 'readable' is removed, the stream will start flowing, i.e. 'data' events will be emitted without calling .resume().
        /// </summary>
        public JSEventCallback OnReadable { get => new JSEventCallback("readable", On, RemoveListener); set { } }
        /// <summary>
        /// The 'resume' event is emitted when stream.resume() is called and readableFlowing is not true.
        /// </summary>
        public JSEventCallback OnResume { get => new JSEventCallback("resume", On, RemoveListener); set { } }
        #endregion
    }
}
