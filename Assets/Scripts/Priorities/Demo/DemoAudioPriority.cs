using System;

namespace Dungeonesque.Priorities
{
    [Serializable]
    public struct DemoSoundTarget : IPriority
    {
        public int uuid;

        public DemoSoundTarget(int uuid)
        {
            this.uuid = uuid;
        }

        public int UUID => uuid;

        public PriorityType type => PriorityType.Audio;

        public float weight => 3f;
    }
}