using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class BossTrigger : MonoBehaviour
    {
        public Transform lookAt;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            var rotation = transform.rotation.eulerAngles;
            rotation.z = Vector2.SignedAngle(Vector2.right, transform.position - lookAt.position);
            transform.rotation = Quaternion.Euler(rotation);
        }
        void OnTriggerEnter2D(Collider2D collider)
        {
            var plr = collider.GetComponent<Player>();
            if (plr != null)
            {
                // END GAME HERE
                print("Game should end here");
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
            }
        }
    }
}
