using System;
using System.Collections.Generic;
namespace MDRPG
{
    public sealed class StagePlayer
    {
        public static Point viewPortPixelRect { get; private set; } = new Point(16 * 24, 9 * 24);
        public Point cameraPosition { get; set; } = Point.Zero;

        private List<StageItem> stageItems = new List<StageItem>();
        private List<StageItem> stageItemsToAdd = new List<StageItem>();
        private List<StageItem> stageItemsToRemove = new List<StageItem>();

        public readonly InputManager inputManager = null;
        public StageData stageData { get; private set; } = null;
        public StagePlayer(StageData stageData)
        {
            if (stageData is null)
            {
                throw new NullReferenceException();
            }
            this.stageData = stageData;
            inputManager = new InputManager(this);
            Regenerate();
        }

        public bool playerIsDead = false;
        private void Regenerate()
        {
            playerIsDead = false;

            for (int i = 0; i < stageItems.Count; i++)
            {
                RemoveStageItem(i);
            }

            if (stageData is not null)
            {
                foreach (TileData tileData in stageData.data)
                {
                    StageItem newItem = null;
                    if (tileData.itemName.ToLower() == "player")
                    {
                        newItem = new Player(this)
                        {
                            position = new Point(tileData.position.x * 16, tileData.position.y * 16),
                        };
                    }
                    else if (tileData.itemName.ToLower() == "ground")
                    {
                        newItem = new Ground(this)
                        {
                            position = new Point(tileData.position.x * 16, tileData.position.y * 16),
                        };
                    }
                    else if (tileData.itemName.ToLower() == "lava")
                    {
                        newItem = new Lava(this)
                        {
                            position = new Point(tileData.position.x * 16, tileData.position.y * 16),
                        };
                    }
                    else if (tileData.itemName.ToLower() == "nojump")
                    {
                        newItem = new NoJump(this)
                        {
                            position = new Point(tileData.position.x * 16, tileData.position.y * 16),
                        };
                    }
                    else if (tileData.itemName.ToLower() == "spring")
                    {
                        newItem = new Spring(this)
                        {
                            position = new Point(tileData.position.x * 16, tileData.position.y * 16),
                        };
                    }
                    if (newItem is not null)
                    {
                        AddStageItem(newItem);
                    }
                }
            }
        }
        public void Tick(InputPacket packet)
        {
            if (playerIsDead)
            {
                Regenerate();
            }

            foreach (StageItem stageItemToRemove in stageItemsToRemove)
            {
                stageItems.Remove(stageItemToRemove);
            }
            stageItemsToRemove = new List<StageItem>();

            foreach (StageItem newStageItem in stageItemsToAdd)
            {
                stageItems.Add(newStageItem);
            }
            stageItemsToAdd = new List<StageItem>();

            inputManager.UpdateInput(packet);

            List<Collider> loadedColliders = new List<Collider>();
            foreach (StageItem stageItem in stageItems)
            {
                if (stageItem.collider is not null)
                {
                    loadedColliders.Add(stageItem.collider);
                }
            }

            foreach (StageItem stageItem in stageItems)
            {
                if (stageItem.rigidbody is not null)
                {
                    stageItem.rigidbody.TickMovement(loadedColliders);
                }
            }

            foreach (StageItem stageItem in stageItems)
            {
                if (stageItem.collisionLogger is not null)
                {
                    stageItem.collisionLogger.LogCollisions(loadedColliders);
                }
            }

            foreach (StageItem stageItem in stageItems)
            {
                stageItem.Update();
            }
        }
        public Texture Render()
        {
            Texture frame = new Texture(viewPortPixelRect.x, viewPortPixelRect.y, new Color(255, 255, 155, 255));

            foreach (StageItem gameObject in stageItems)
            {
                if (gameObject.texture is not null)
                {
                    TextureHelper.Blitz(gameObject.texture, frame, new Point(gameObject.position.x - cameraPosition.x, gameObject.position.y - cameraPosition.y));
                }
            }

            return frame;
        }
        #region StageItem Management Methods
        public StageItem GetStageItem(int index)
        {
            if (stageItems is null)
            {
                stageItems = new List<StageItem>();
                return null;
            }
            if (index < 0 || index >= stageItems.Count)
            {
                throw new ArgumentException();
            }
            return stageItems[index];
        }
        public List<StageItem> GetStageItems()
        {
            return new List<StageItem>(stageItems);
        }
        public int GetStageItemCount()
        {
            if (stageItems is null)
            {
                stageItems = new List<StageItem>();
                return 0;
            }
            return stageItems.Count;
        }
        public void RemoveStageItem(int index)
        {
            if (stageItems is null)
            {
                stageItems = new List<StageItem>();
                return;
            }
            if (stageItemsToRemove is null)
            {
                stageItemsToRemove = new List<StageItem>();
            }
            if (index < 0 || index >= stageItems.Count)
            {
                throw new ArgumentException();
            }
            stageItemsToRemove.Add(stageItems[index]);
        }
        public void RemoveStageItem(StageItem target)
        {
            if (stageItemsToRemove is null)
            {
                stageItemsToRemove = new List<StageItem>();
            }
            if (target is null)
            {
                throw new NullReferenceException();
            }
            if (stageItems is null)
            {
                stageItems = new List<StageItem>();
                return;
            }
            stageItemsToRemove.Add(target);
        }
        public void AddStageItem(StageItem newStageItem)
        {
            if (stageItemsToAdd is null)
            {
                stageItemsToAdd = new List<StageItem>();
            }
            if (newStageItem is null)
            {
                throw new NullReferenceException();
            }
            stageItemsToAdd.Add(newStageItem);
        }
        #endregion
    }
}