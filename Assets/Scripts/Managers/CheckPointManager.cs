using UnityEngine;

namespace Managers
{
    public class CheckPointManager : MonoBehaviour
    {
        public static CheckPointManager Instance;
        private Vector3 lastCheckpointPosition;

        private void Awake()
        {
            Instance = this;
        }

        public void RecordPosition(Vector3 pos)
        {
            lastCheckpointPosition = pos;
        }

        public Vector3 GetCheckPointPosition()
        {
            return lastCheckpointPosition;
        }
        
    }
}