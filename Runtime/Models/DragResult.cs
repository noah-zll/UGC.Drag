namespace UGC.Drag.Models
{
    public readonly struct DragResult
    {
        public DragResult(bool success, Runtime.DropTarget target, string reason)
        {
            Success = success;
            Target = target;
            Reason = reason;
        }

        public bool Success { get; }

        public Runtime.DropTarget Target { get; }

        public string Reason { get; }
    }
}

