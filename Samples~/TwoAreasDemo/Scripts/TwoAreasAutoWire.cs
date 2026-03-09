using UnityEngine;
using UGC.Drag.Runtime;

namespace UGC.Drag.Samples
{
    public sealed class TwoAreasAutoWire : MonoBehaviour
    {
        public DragService service;

        private void Awake()
        {
            if (service == null)
            {
                service = FindObjectOfType<DragService>(true);
            }

            if (service == null)
            {
                return;
            }

            var sources = FindObjectsOfType<DragSource>(true);
            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i].service == null)
                {
                    sources[i].service = service;
                }
            }
        }
    }
}

