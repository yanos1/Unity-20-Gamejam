using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "UnityVersionData", menuName = "Scriptable Objects/UnityVersionData")]
    public class UnityVersionData : ScriptableObject
    {
        public bool updateSpeed;
        public bool updateJump;
        public float jumpPower;
        public float speed;
        public Sprite sprite;
    }
}
