using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMP
{
    public class Player : MonoBehaviour
    {
        int selectedItemIndex = 0;
        public Hotbar hotbar;
        public static int InventorySize = 10;
        public static Player instance;
        List<ItemStack> inventory = new();

        Rigidbody2D rb;
        LayerMask groundLayerMask;
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;

            rb = GetComponent<Rigidbody2D>();
            groundLayerMask = LayerMask.GetMask("Default");
        }
        // Start is called before the first frame update
        void Start()
        {
            int pickId = (int)ItemID.StonePickaxe;
            inventory.Add(new ItemStack
            {
                itemId = pickId,
                item = ItemManager.instance.Items[pickId],
                amount = 1
            });
            pickId = (int)ItemID.GoldPickaxe;
            inventory.Add(new ItemStack
            {
                itemId = pickId,
                item = ItemManager.instance.Items[pickId],
                amount = 1
            });
            hotbar.UpdateDisplay(inventory);
            hotbar.DisplaySelectedIndex(selectedItemIndex);
            TeleportToSpawn();
        }

        void TeleportToSpawn()
        {
            print("Teleporting to spawn");
            Vector3 newPos = (Vector3Int)WorldManager.instance.worldInfo.spawnPoint * 16;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }

        void SetSelectedItem(int index)
        {
            selectedItemIndex = index;
            hotbar.DisplaySelectedIndex(index);
        }
        // Update is called once per frame
        void Update()
        {
            // Detect left click from player:
            //   Get co-ordinates in the tile grid and call SetBlock on the WorldManager with the block that the player is holding or if the player is holding a pickaxe then call BreakBlock
            if (Input.GetMouseButtonDown(0))
            {
                
                // var wm = WorldManager.instance;
                var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                // var gridCoords = wm.WorldToGrid(point);
                // print(gridCoords);
                // var block = new Block() { tileType = TileType.Grass };
                // wm.SetBlock(gridCoords, block);
                inventory[selectedItemIndex].item.BeginUse(point, inventory[selectedItemIndex]);
                hotbar.UpdateDisplay(inventory);
            }
            if (Input.GetMouseButtonDown(1))
            {
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                TeleportToSpawn();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetSelectedItem(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetSelectedItem(1);                
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetSelectedItem(2);                
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SetSelectedItem(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SetSelectedItem(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SetSelectedItem(5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                SetSelectedItem(6);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                SetSelectedItem(7);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                SetSelectedItem(8);
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                SetSelectedItem(9);
            }
            ProcessMovement();
        }

        void ProcessMovement()
        {
            bool canJump = false;
            {
                //var collider = sr.GetComponent<BoxCollider2D>();
                Vector3 centrePoint = new Vector3(transform.position.x, transform.position.y - 16f);
                //Debug.DrawLine(centrePoint, centrePoint + Vector3.up * 0.1f, Color.black, 1f);
                //print(centrePoint);
                Collider2D overlap = Physics2D.OverlapBox(centrePoint, new Vector2(32 * 1.8f, 0.00001f), 0, groundLayerMask);
                if (overlap != null)
                {
                    canJump = true;
                }
            }
            float maxSpeed = 8f * 6f;
            float desiredSpeed = 0;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                desiredSpeed = -maxSpeed;
            }
            if (Input.GetKey(KeyCode.S))
            {
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                desiredSpeed = maxSpeed;
            }
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && canJump)
            {
                rb.AddForce(Vector2.up * rb.mass * 16.0f * 6, ForceMode2D.Impulse);
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                //var loadTilemaps = GameObject.Find("Scene Scripts").GetComponent<LoadTilemap>();
                //loadTilemaps.maps[TileGroup.ForegroundBasic].SetTile(new Vector3Int(400, 0), loadTilemaps.tiles[2]);
                transform.position = new Vector2(160, 400);
            }

            float currentSpeed = rb.velocity.x;

            float desiredSpeedDelta = Mathf.Round(desiredSpeed - currentSpeed);

            float forceRequired = 10f * rb.mass * (desiredSpeedDelta != 0 ? Mathf.Sign(desiredSpeedDelta) : 0);
            if (desiredSpeed == 0) forceRequired /= 2f;
            if (Mathf.Abs(desiredSpeedDelta) < 1 && desiredSpeed == 0) forceRequired = 0;

            if (forceRequired != 0)
            {
                if (desiredSpeed != 0)
                {
                    var desiredDirection = Mathf.Sign(desiredSpeed);
                    //FaceDirection(desiredDirection > 0);
                }
            }

            rb.AddForce(Vector2.right * forceRequired * Time.deltaTime * 250 * 8);

            //animator.SetBool("Walking", desiredSpeed != 0);
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            inventory.RemoveAll(x => x.itemId == (int)ItemID.None);
            if (!other.CompareTag("DroppedItem")) return;
            var di = other.GetComponent<DroppedItem>();
            var existingStack = inventory.Find(x => x.itemId == di.itemStack.itemId);
            if (existingStack != null)
            {
                existingStack.Add(di.itemStack.amount);
            }
            else
            {
                inventory.Add(di.itemStack);
            }
            hotbar.UpdateDisplay(inventory);
            Destroy(other.gameObject);
        }
    }
}
