using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class FollowCursorUiSpace : MonoBehaviour
    {
        void  Update()
        {
            transform.position = Input.mousePosition;
        }
    }
}
