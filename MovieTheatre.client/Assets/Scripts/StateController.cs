using System;
using Data;

namespace DefaultNamespace
{
    public class StateController
    {
        private static readonly Lazy<StateController> Lazy =
            new Lazy<StateController>(() => new StateController());

        public static StateController Instance => Lazy.Value;

        public string CurrentClientName { get; set; }
        public EventData CurrentEvent { get; set; }

        private StateController()
        {
        }
    }
}