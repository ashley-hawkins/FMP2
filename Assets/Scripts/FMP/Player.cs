using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace FMP
{
    public class Player : MonoBehaviour
    {
        int selectedItemIndex = 0;
        public Hotbar hotbar;
        public const int InventorySize = 40;
        public static Player instance;
        public List<ItemStack> inventory { get; private set; }

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
        
        public List<ItemStack> GetTotalAmounts()
        {
            return inventory.GroupBy(x => x.itemId).Select(x => new ItemStack { itemId = x.Key, amount = x.Sum(y => y.amount) }).ToList();
        }

        void Start()
        {
            inventory = Enumerable.Range(0, InventorySize).Select(_ => new ItemStack
            {
                itemId = ItemID.None,
                amount = 0,
            }).ToList();

            inventory[0] = new ItemStack
            {
                itemId = ItemID.StonePickaxe,
                amount = 1
            };
            inventory[1] = new ItemStack
            {
                itemId = ItemID.GoldPickaxe,
                amount = 1
            };

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
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {

                    // var wm = WorldManager.instance;
                    var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    // var gridCoords = wm.WorldToGrid(point);
                    // print(gridCoords);
                    // var block = new Block() { tileType = TileType.Grass };
                    // wm.SetBlock(gridCoords, block);
                    var stack = inventory[selectedItemIndex];
                    if (stack.itemId == ItemID.None) return;

                    var item = stack.item;
                    item.BeginUse(point, stack);
                    hotbar.UpdateDisplay(inventory);
                }
                if (Input.GetMouseButtonDown(1))
                {
                }
            }
            else
            {
                UnityEngine.EventSystems.PointerEventData ed = new(UnityEngine.EventSystems.EventSystem.current);
                //ed.
                //UnityEngine.EventSystems.EventSystem.current.RaycastAll

            }
            if (Input.GetKeyDown(KeyCode.U))
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
                Collider2D overlap = Physics2D.OverlapBox(centrePoint, new Vector2(16 * 1.8f, 0.00001f), 0, groundLayerMask);
                if (overlap != null)
                {
                    canJump = true;
                }
            }
            float maxSpeed = 8f * 7f;
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
                rb.AddForce(Vector2.up * rb.mass * 16.0f * 7, ForceMode2D.Impulse);
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
            if (Mathf.Abs(desiredSpeedDelta) < 1 && desiredSpeed == 0)
            {
                forceRequired = 0;
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

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

        public void AddItemStackToInventory(ItemStack itemStack)
        {
            var existingStack = inventory.Find(x => x.itemId == itemStack.itemId && x.amount != 0);
            if (existingStack != null)
            {
                existingStack.Add(itemStack.amount);
            }
            else
            {
                var emptyStack = inventory.Find(x => x.itemId == (int)ItemID.None || x.amount == 0);
                if (emptyStack != null)
                {
                    emptyStack.amount = 0;
                    emptyStack.itemId = itemStack.itemId;
                    emptyStack.Add(itemStack.amount);
                }
                else
                {
                    inventory.Add(itemStack);
                }
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("DroppedItem")) return;
            var di = other.GetComponent<DroppedItem>();
            AddItemStackToInventory(di.itemStack);
            hotbar.UpdateDisplay(inventory);
            Destroy(other.gameObject);
        }
    }
}
