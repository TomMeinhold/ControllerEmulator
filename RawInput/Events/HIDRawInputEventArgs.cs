namespace RawInput.Events
{
    public class HIDRawInputEventArgs
    {
        public HIDRawInputEventArgs(RawInputHidData data)
        {
            Data = data;
        }

        public RawInputHidData Data { get; private set; }
    }
}