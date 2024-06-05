namespace SpawnDev.BlazorJS.SimplePeer.WebPeer
{
    /// <summary>
    /// Method parameters marked with the CallSide attribute will be resolved on the called side
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Parameter)]
    public class CallSideAttribute : Attribute { }
}
