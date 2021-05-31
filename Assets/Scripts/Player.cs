using UnityEngine;

public class Player : Actor
{
    AdventureGame controls;

    private int experience;
    private Quest[] quests;
    private Weapon heldWeapon;
    private Armour equipedArmour;
    private Item[] consumables;
    private float potionCooldown;

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    public void Reset()
    {
        maxHealth = InitialHealth;
        currentHealth = maxHealth;

        UIController.Instance.PlayerHUD.UpdateHUD(this);
    }

    private void Awake()
    {
        controls = new AdventureGame();
        controls.Player.Move.performed += context => BeginMove(context.ReadValue<Vector2>());
    }

    public override void BeginMove(Vector2 _direction)
    {
        if (GameController.Instance.GameState == GameController.eGameState.PlayerTurn)
        {
            base.BeginMove(_direction);
        }
    }

    protected override void EndTurn()
    {
        ActorState = eActorState.Idle;
        if (GameController.Instance.GameState != GameController.eGameState.InTown)
        {
            GameController.Instance.GoToState(GameController.eGameState.MonsterTurn);
        }
    }


    public override void TakeDamage(int _amount)
    {
        base.TakeDamage(_amount);
        UIController.Instance.PlayerHUD.UpdateHUD(this);
    }

    public override void EnterTile(Vector2Int _tilePosition)
    {
        DungeonController.Instance.GetTile(TilePosition).MapObjects.Remove(this);

        TilePosition = targetPosition;
        Tile tile = DungeonController.Instance.GetTile(TilePosition);
        tile.MapObjects.Add(this);

        for (int i = 0; i < tile.MapObjects.Count; i++)
        {
            if (tile.MapObjects[i].GetType() == typeof(Door))
            {
                EnterDoor(tile.MapObjects[i] as Door);
            }
        }
    }

    void EnterDoor(Door _door)
    {
        if (_door == DungeonController.Instance.CurrentDungeon.DungeonExitDoor)
        {
            GameController.Instance.ExitDungeon();
        }
        else
        {
            DungeonController.Instance.CurrentFloor.gameObject.SetActive(false);
            DungeonController.Instance.CurrentFloor = _door.TargetDoor.Floor;
            DungeonController.Instance.CurrentFloor.gameObject.SetActive(true);

            DungeonController.Instance.CurrentRoom.gameObject.SetActive(false);
            DungeonController.Instance.CurrentRoom = _door.TargetDoor.Room;
            DungeonController.Instance.CurrentRoom.gameObject.SetActive(true);

            SetPosition(_door.TargetDoor.TilePosition);
        }
    }

    void OnLevelUp()
    {

    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}
