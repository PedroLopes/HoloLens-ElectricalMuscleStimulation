using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class PushHandler : ContinuousHandler
    {
        protected override int defaultMaxWidth { get { return 150; } }
    }
}