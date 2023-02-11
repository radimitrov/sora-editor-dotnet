namespace IO.Github.Rosemoe.Sora.Event;

public class EventReceiver : Java.Lang.Object, IEventReceiver
{
	public event EventHandler<EventReceiverArgs> Receive;
	public void OnReceive(Java.Lang.Object res, Unsubscribe unsub)
	{
		Receive?.Invoke(this, new EventReceiverArgs()
		{
			Result = res,
			Unsubscribe = unsub
		});
	}

	public class EventReceiverArgs : EventArgs
	{
		public Java.Lang.Object Result { get; init; }
		public Unsubscribe Unsubscribe { get; init; }
	}
}
