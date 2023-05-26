using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform subject;
        Camera cam;
        public static Vector2 maxCoords;
        void Start()
        {
            cam = GetComponent<Camera>();
        }

        public void DoUpdate()
        {
            var bottomLeft = (Vector2)cam.ViewportToWorldPoint(new Vector2(0, 0));
            var centre = (Vector2)cam.ViewportToWorldPoint(Vector2.one * 0.5f);
            var difference = centre - bottomLeft;
            var minPoint = difference;
            var maxPoint = maxCoords - difference;

            Vector2 finalPosition = new Vector2(Mathf.Clamp(subject.position.x, minPoint.x, maxPoint.x), Mathf.Clamp(subject.position.y, minPoint.y, maxPoint.y));
            transform.position = new Vector3(finalPosition.x, finalPosition.y, transform.position.z);
        }
    }
}