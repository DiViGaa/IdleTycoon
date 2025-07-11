using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Interface;
using Player;
using UnityEngine;

namespace NPC
{
    public enum LogisticTargetType
    {
        Storage,
        Factory,
        Market
    }

    [System.Serializable]
    public class ResourceTransportSetting
    {
        public ResourceType Resource;
        public LogisticTargetType TargetType;
        public float CarryAmount = 10f;
    }

    public class NPCSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject npcPrefab;
        [SerializeField] private Transform spawnPoint;

        [SerializeField] private Building sourceBuilding;

        [SerializeField] private List<ResourceTransportSetting> transportSettings = new();

        private readonly Dictionary<ResourceType, Coroutine> _coroutines = new();
        private IResourceProvider _provider;

        private void Start()
        {
            if (sourceBuilding == null)
            {
                return;
            }

            if (spawnPoint == null)
            {
                return;
            }

            _provider = sourceBuilding.GetComponent<IResourceProvider>();
            if (_provider == null)
            {
                return;
            }

            foreach (var setting in transportSettings)
            {
                if (_coroutines.ContainsKey(setting.Resource)) continue;

                Coroutine c = StartCoroutine(TransportRoutine(setting));
                _coroutines.Add(setting.Resource, c);
            }
        }

        private IEnumerator TransportRoutine(ResourceTransportSetting setting)
        {
            while (true)
            {
                Building targetBuilding = FindTargetBuilding(setting.TargetType);
                if (targetBuilding == null)
                {
                    yield return new WaitForSeconds(2f);
                    continue;
                }

                var receiver = targetBuilding.GetComponent<IResourceReceiver>();
                if (receiver == null)
                {
                    yield return new WaitForSeconds(2f);
                    continue;
                }

                if (_provider.GetAmount(setting.Resource) >= setting.CarryAmount)
                {
                    if (_provider.TryTakeResource(setting.Resource, setting.CarryAmount))
                    {
                        GameObject npcObj = Instantiate(
                            npcPrefab,
                            spawnPoint != null ? spawnPoint.position : transform.position,
                            Quaternion.identity
                        );
                    
                        npcObj.gameObject.transform.position = spawnPoint.position;
                    
                        LogisticsNPC npc = npcObj.GetComponent<LogisticsNPC>();

                        npc.Setup(
                            setting.Resource,
                            setting.CarryAmount,
                            sourceBuilding.transform.position,
                            targetBuilding.transform.position,
                            () => receiver.AddResource(setting.Resource, setting.CarryAmount)
                        );

                        yield return new WaitUntil(() => npc.IsReturned);

                        Destroy(npcObj);
                    }
                }

                yield return new WaitForSeconds(0.5f);
            }
        }


        private Building FindTargetBuilding(LogisticTargetType targetType)
        {
            var candidates = FindObjectsOfType<Building>().Where(b =>
            {
                switch (targetType)
                {
                    case LogisticTargetType.Storage:
                        return b.GetComponent<IStorageMarker>() != null;
                    case LogisticTargetType.Factory:
                        return b.GetComponent<FactoryBuilding>() != null;
                    case LogisticTargetType.Market:
                        return b.GetComponent<MarketBuilding>() != null;
                    default:
                        return false;
                }
            }).ToList();

            if (candidates.Count == 0) return null;

            return candidates.OrderBy(b => Vector3.Distance(transform.position, b.transform.position)).First();
        }
        
        public void AddTransportSettingIfMissing(ResourceTransportSetting newSetting)
        {
            bool exists = transportSettings.Any(ts =>
                ts.Resource == newSetting.Resource && ts.TargetType == newSetting.TargetType);

            if (!exists)
            {
                transportSettings.Add(newSetting);

                if (!_coroutines.ContainsKey(newSetting.Resource))
                {
                    Coroutine c = StartCoroutine(TransportRoutine(newSetting));
                    _coroutines.Add(newSetting.Resource, c);
                }
            }
        }

        private void OnDisable()
        {
            foreach (var coroutine in _coroutines.Values)
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);
            }
            _coroutines.Clear();
        }
    }
}