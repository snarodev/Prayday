using UnityEngine;

namespace SnaroDev.LD60
{
    public class BoatMove : MonoBehaviour
    {
        private Vector3 startPos;

        [SerializeField]
        private float movementSpeed = 1;

        private void Start()
        {
            startPos = transform.position;
        }

        private void Update()
        {
            transform.position = startPos + new Vector3((Time.time * movementSpeed) % 10, Mathf.Sin(Time.time) * 0.1f, 0);
        }
    }
}
