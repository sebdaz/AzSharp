using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Shared.Entities;
using AzSharp.ECS.Unity.Mono;
using AzSharp.ECS.Unity.UnityComp;
using AzSharp.ECS.Unity.UnityComp.Inheriters;
using AzSharp.IoC;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AzSharp.ECS.Unity.GameObjectManager;

public class GameObjectManager : IGameObjectManager
{
    private Dictionary<GameObject, uint> TranslationDict = new();
    public GameObject CreateGameObject(uint entity_id, string? name = null)
    {
        GameObject gameObject = new GameObject(name);
        SignGameObject(gameObject, entity_id);
        return gameObject;
    }

    public void DestroyGameObject(GameObject game_object)
    {
        UnsignGameObject(game_object);
        UnityEngine.Object.Destroy(game_object);
    }

    public uint GetEntityID(GameObject game_object)
    {
        if (!TranslationDict.ContainsKey(game_object))
        {
            throw new ArgumentException("No entity ID to assigned game object");
        }
        return TranslationDict[game_object];
    }

    public void SignGameObject(GameObject gameObject, uint entity_id)
    {
        if (TranslationDict.ContainsKey(gameObject))
        {
            throw new InvalidOperationException("Game object already has a signed entity ID");
        }
        TranslationDict[gameObject] = entity_id;
    }

    public void UnsignGameObject(GameObject gameObject)
    {
        if (!TranslationDict.ContainsKey(gameObject))
        {
            throw new InvalidOperationException("Tried to unsign a game object that wasn't signed");
        }
        TranslationDict.Remove(gameObject);
    }

    public void InstantiantePrefabChildren(string path, uint parent_ent)
    {
        List<uint> ent_list = new();
        GameObject prefab_obj = Resources.Load<GameObject>(path);
        foreach (Transform child_transform in prefab_obj.transform)
        {
            GameObject child_gameobject = UnityEngine.Object.Instantiate(child_transform.gameObject);
            child_gameobject.name = child_gameobject.name.Replace("(Clone)", "");
            _InstantiateGameObject(child_gameobject, parent_ent, ent_list);
        }
        InitializeEntities(ent_list);
    }

    public uint InstantiatePrefab(string path, uint parent_ent, bool init = true)
    {
        List<uint> ent_list = new();
        GameObject prefab_obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(path));
        prefab_obj.name = prefab_obj.name.Replace("(Clone)", "");
        _InstantiateGameObject(prefab_obj, parent_ent, ent_list);
        if (init)
        {
            InitializeEntities(ent_list);
        }
        return GetEntityID(prefab_obj);
    }
    private void InitializeEntities(List<uint> ent_list)
    {
        IEntityManager ent_manager = IoCManager.Resolve<IEntityManager>();
        ent_manager.InitializeEntities(ent_list);
    }
    private void SetGameObjectParentEnt(GameObject gameobject, uint parent_ent)
    {
        if (parent_ent == Entity.NULL_ENTITY)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform>? transform = comp_manager.GetComponent<UCTransform>(GetEntityID(gameobject));
        if (transform == null)
        {
            return;
        }
        transform.comp.SetParent(parent_ent, false);
    }
    private void _InstantiateGameObject(GameObject gameObject, uint parent_ent, List<uint> ent_list)
    {
        //Recursively go through objects and their child objects starting with the `gameObject` and remember all of them
        List<GameObject> current_objects = new();
        List<GameObject> all_objects = new();
        current_objects.Add(gameObject);
        while (current_objects.Count > 0)
        {
            GameObject top_object = current_objects[current_objects.Count - 1];
            foreach (Transform child_transform in top_object.transform)
            {
                current_objects.Add(child_transform.gameObject);
            }
            all_objects.Add(top_object);
            current_objects.Remove(top_object);
        }
        foreach (GameObject iterated_object in all_objects)
        {
            ECSWrap(iterated_object, ent_list);
        }
        SetGameObjectParentEnt(gameObject, parent_ent);
    }
    private void ECSWrap(GameObject gameObject, List<uint> ent_list)
    {
        string extra_comp = string.Empty;
        string prototype_name = string.Empty;
        bool inherit_transform = true;

        ECSPrototype prototype_comp = gameObject.GetComponent<ECSPrototype>();
        if (prototype_comp != null)
        {
            extra_comp = prototype_comp.ExtraComponent;
            prototype_name = prototype_comp.PrototypeName;
            inherit_transform = prototype_comp.InheritTransform;
        }

        IEntityManager ent_manager = IoCManager.Resolve<IEntityManager>();
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();

        Entity ent = ent_manager.CreateEntity();
        uint entity_id = ent.ID;

        if (inherit_transform)
        {
            //Try and create the unity component wrappers here
            UnityCompInheriter.DoInheritance(gameObject, entity_id);
        }
        else
        {
            UnityEngine.Object.Destroy(gameObject);
        }

        if (prototype_name != null && prototype_name != "" && prototype_name != string.Empty)
        {
            ent_manager.CreatePrototype(prototype_name, entity_id, false);
        }
        if (extra_comp != null && extra_comp != "" && extra_comp != string.Empty)
        {
            string[] word_components = extra_comp.Split(';');
            foreach (string word_comp in word_components)
            {
                Type extra_comp_type = comp_manager.ComponentTypeFromName(word_comp);
                if (extra_comp_type == null)
                {
                    throw new ArgumentException("Extra Comp name didn't point to any component type");
                }
                if (!comp_manager.HasComponent(comp_manager.ComponentTypeFromName(word_comp), entity_id))
                {
                    comp_manager.AddComponent(extra_comp_type, entity_id);
                }
            }
        }

        ent_list.Add(entity_id);
    }

    public uint InstantiatePrototype(string proto_id, uint parent_ent)
    {
        IEntityManager ent_manager = IoCManager.Resolve<IEntityManager>();
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Entity ent = ent_manager.CreateEntityFromPrototype(proto_id);
        Component<UCTransform>? transform = comp_manager.GetComponent<UCTransform>(ent.ID);
        if (transform == null)
        {
            throw new ArgumentException("Prototype didn't instantiate with UCTransform to set parent");
        }
        transform.comp.SetParent(parent_ent, false);
        return ent.ID;
    }

    public uint InstantiatePrefabPrototype(string prefab_path, string proto_id, uint parent_ent)
    {
        IEntityManager ent_manager = IoCManager.Resolve<IEntityManager>();
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        uint ent = InstantiatePrefab(prefab_path, parent_ent, false);
        ent_manager.CreatePrototype(proto_id, ent, true);
        return ent;
    }
}
