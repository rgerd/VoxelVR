

private var oscHandler: Osc = null;
public var controller : Transform;
public var remoteIp : String;
public var sendToPort : int;
public var listenerPort : int;
public var BodyView : GameObject;



// Start is called just before any of the Update methods is called the first time.
public function Start()
{

    var udp:UDPPacketIO  = GetComponent("UDPPacketIO");
    udp.init(remoteIp, sendToPort, listenerPort);

    oscHandler = GetComponent("Osc");
    oscHandler.init(udp);

    //oscHandler.SetAddressHandler("/1/push1", Example);
}

// Update is called every frame, if the MonoBehaviour is enabled.
function Update()
{
    var message : String = BodyView.GetComponent("BodySourceView").getString();
    //Debug.Log("/" + message);
    var oscM : OscMessage = null;
    oscM = Osc.StringToOscMessage("/" + message);
    oscHandler.Send(oscM);

}

function OnDisable()
{
    // close OSC UDP socket
    Debug.Log("closing OSC UDP socket in OnDisable");
    oscHandler.Cancel();
    oscHandler = null;
}


