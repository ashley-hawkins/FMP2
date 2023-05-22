using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform subject;
        Camera cam;
        static Vector2 maxCoords = new(100, 100);
        // Start is called before the first frame update
        void Start()
        {
            cam = GetComponent<Camera>();
        }

        // Update is called once per frame
        public void DoUpdate()
        {
            var bottomLeft = (Vector2)cam.ViewportToWorldPoint(new Vector2(1, 0));
            var centre = (Vector2)cam.ViewportToWorldPoint(Vector2.one * 0.5f);
            var difference = centre - bottomLeft;
            var minPoint = difference;
            var maxPoint = maxCoords - difference;

            transform.position = new Vector3(subject.position.x, subject.position.y, transform.position.z);
        }
    }
}