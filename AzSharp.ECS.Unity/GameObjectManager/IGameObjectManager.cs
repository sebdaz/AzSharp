using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AzSharp.ECS.Unity.GameObjectManager;

public interface IGameObjectManager
{
    public void SignGameObject(GameObject gameObject, uint entity_id);
    public void UnsignGameObject(GameObject gameObject);
    public GameObject CreateGameObject(uint entity_id, string? name = null);
    public void DestroyGameObject(GameObject game_object);
    public uint GetEntityID(GameObject game_object);
    public uint InstantiatePrefab(string path, uint parent_ent, bool init = true);
    public void InstantiantePrefabChildren(string path, uint parent_ent);
    public void InstantiatePrototype(string proto_id, uint parent_ent);
    public void InstantiatePrefabPrototype(string prefab_path, string proto_id, uint parent_ent);
}
