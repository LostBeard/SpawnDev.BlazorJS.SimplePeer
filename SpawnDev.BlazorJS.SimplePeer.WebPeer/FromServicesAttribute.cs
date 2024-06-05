namespace SpawnDev.BlazorJS.SimplePeer.WebPeer
{
    /// <summary>
    /// Method parameters marked with the FromServices attribute will be resolved from the called side peer service provider
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Parameter)]
    public class FromServicesAttribute : Attribute { }
}
