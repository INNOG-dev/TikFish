using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public enum EnumDifficulty
    {
        EASY,
        MEDIUM,
        HARD
    };

    public static GameLogic instance;

    public EnumDifficulty difficulty;


    private Dictionary<GameObject, Entity> entities = new Dictionary<GameObject, Entity>();

    private Dictionary<TiktokUser, Fish> players = new Dictionary<TiktokUser, Fish>();

    public List<Bonus> bonus = new List<Bonus>();

    private static EntityRegistry registry = new EntityRegistry();
    private static EffectRegistry effectRegistry = new EffectRegistry();

    private static readonly int maxBonusInWorld = 5;

    private static int id_counter = 0;

    private WebService webService;

    public GameObject LoadingGameobjectUI;

    public DialogBox dialogBox;

    [SerializeField]
    private MapData mapData;

    private PartyManager partyManager;

    private GiftManager giftManager;

    public void Awake()
    {
        instance = this;

        Models.registerModels();
        EntityType.registerEntities();
        Effects.registerEffects();

        webService = new WebService();

        partyManager = new PartyManager();

        giftManager = new GiftManager();

        GiftRegistry.registerGifts();
    }


    public void Start()
    {
        partyManager.startParty();

        InvokeRepeating("UpdateBonus", 0f, 1f);

        /*Fish fish = addPlayer(new TiktokUser("INeoxz", "41120"), EntityType.ENTITY_SKELETONSHARK);
        fish.setLocalPlayer();
        fish.addScore(200);*/
    }

    public void addBots()
    {
        int[] fishs = new int[] { EntityType.ENTITY_CLOWNFISH, EntityType.ENTITY_FISH, EntityType.ENTITY_SEAL, EntityType.ENTITY_SHARK, EntityType.ENTITY_SKELETONSHARK };
        for (int i = 0; i < Random.Range(10,100); i++)
        {
            Fish fish = addPlayer(new TiktokUser("Joueur-" + i, ""), fishs[Random.Range(0, fishs.Length)]);
            fish.addScore(Random.Range(110, 150));
        }
    }

    public void initializeLive(Streamer user)
    {
        webService.setStreamer(user);

        webService.startStream();
    }

    public Fish addPlayer(TiktokUser user, int type)
    {
        Fish entity = (Fish)GameLogic.instance.addEntity(type, mapData.getRandomMapPos());
        players.Add(user, entity);

        entity.setUsername(user.uniqueId);

        entity.setTagName(entity.getUsername());

        return entity;
    }

    public Entity addEntity(int entityId, Vector2 position)
    {
        return addEntityWithProperties(entityId, position, null);
    }

    public Entity addEntityWithProperties(int entityId, Vector2 position, params object[] parameters)
    {
        Entity entity = registry.newEntity(entityId, id_counter++, parameters);

        entities.Add(entity.GetGameObject(), entity);


        entity.setPosition(position);

        EntityDebugger debugger = entity.GetGameObject().AddComponent<EntityDebugger>();
        debugger.setDebuggingEntity(entity);

        return entity;
    }

    public void destroyEntity(Entity entity)
    {
        entities.Remove(entity.GetGameObject());

        entity.onDestroy();

        Destroy(entity.GetGameObject());
    }

    public void spawnBonus(int entityId, EffectProperty effect)
    {
        if (getPartyManager().getCurrentGameMode().deactiveBonus().Contains(entityId)) return;

        if (EntityType.isEntityOfType(entityId, typeof(Bonus)))
        {
            bonus.Add((Bonus)addEntityWithProperties(entityId, GameUtils.getRandomPositionWithinBounds(mapData.getMap()), effect));
        }
        else
        {
            Debug.LogError(entityId + " is not a Bonus entity");
        }

        Debug.Log("A bonus spawned");
    }

    public void Update()
    {
        partyManager.Update();

        foreach(Entity entity in entities.Values.ToList())
        {
            if (!entity.isAlive()) continue;

            entity.Update();
        }
    }

    public void OnDrawGizmos()
    {
        foreach (Entity entity in entities.Values.ToList())
        {
            if (!entity.isAlive()) continue;

            entity.OnDrawGizmos();
        }
    }

    public void UpdateBonus()
    {
        if (getPartyManager().getGameState() != GameState.STARTED) return;

        if (getPartyManager().getCurrentGameMode().deactivateAllBonus()) return;


        if (bonus.Count < maxBonusInWorld)
        {
            System.Random random = new System.Random();
            double randomValue = random.NextDouble();

            if (randomValue < 0.05f)
            {
                int randomInt = Random.Range(0, 2);

                if (randomInt == 0)
                    spawnBonus(EntityType.ENTITY_SHIELDBONUS, Effects.INVINCIBLE_EFFECT);
                else 
                    spawnBonus(EntityType.ENTITY_STARBONUS, Effects.STAR_EFFECT);
            }
        }
    }


    public Entity getEntity(GameObject gameObject)
    { 
        if(entities.ContainsKey(gameObject))
            return entities[gameObject];
        return null;
    }

    public Entity getEntityFromCollider(Collider2D collider)
    {
        if(collider.gameObject.tag == "Bonus")
        {
            return getEntity(collider.gameObject);
        }
        GameObject gameObject = collider.transform.parent.parent.parent.gameObject;

        return getEntity(gameObject);
    }

    public Fish getPlayer(TiktokUser user)
    {
        if (players.ContainsKey(user))
            return players[user];
        return null;
    }

    public static EntityRegistry getEntityRegistry()
    {
        return registry;
    }

    public static EffectRegistry getEffectRegistry()
    {
        return effectRegistry;
    }

    public void OnApplicationQuit()
    {
        webService.stopStream();
    }

    public WebService getWebService()
    {
        return webService;
    }

    public bool isInGame(TiktokUser user)
    {
        return players.ContainsKey(user);
    }

    public MapData getMapData()
    {
        return mapData;
    }

    public int getPlayerCount()
    {
        return players.Count;
    }

    public int getAlivePlayerCount()
    {
        int counter = 0;
        foreach (Fish fish in players.Values) if (fish.isAlive()) counter++;
        return counter;
    }

    public List<Fish> getPlayers()
    {
        return players.Values.ToList();
    }

    public PartyManager getPartyManager()
    {
        return partyManager;
    }

    public GiftManager getGiftManager()
    {
        return giftManager;
    }

    public void clearMap()
    {
        foreach (Entity entity in entities.Values.ToList())
        {
            destroyEntity(entity);
        }

        players.Clear();

        entities.Clear();
    }

    public EnumDifficulty getDifficulty()
    {
        return difficulty;
    }
}
