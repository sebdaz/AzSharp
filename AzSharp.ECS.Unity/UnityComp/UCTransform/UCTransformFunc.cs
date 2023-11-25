using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Shared.Entities;
using AzSharp.ECS.Unity.GameObjectManager;
using AzSharp.Info;
using AzSharp.IoC;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AzSharp.ECS.Unity.UnityComp;

public static class UCTransformFunc
{
    public static void ManifestGameObject(this UCTransform transform)
    {
        if (transform.gameObject == null)
        {
            transform.gameObject = new GameObject();
        }
    }
    public static void AssertSignGameObject(this UCTransform transform, uint ent_id)
    {
        if (ent_id == Entity.NULL_ENTITY)
        {
            throw new InvalidOperationException("UCTransform has null entity ID during manifesting and signing game object");
        }
        transform.ManifestGameObject();

        IGameObjectManager go_manager = IoCManager.Resolve<IGameObjectManager>();
        go_manager.SignGameObject(transform.GameObject, ent_id);
    }
    public static void DestroyGameObject(this UCTransform transform)
    {
        IGameObjectManager gomanager = IoCManager.Resolve<IGameObjectManager>();
        gomanager.DestroyGameObject(transform.GameObject);
        transform.gameObject = null;
    }
    public static uint GetParentEntityID(this UCTransform transform)
    {
        if (transform.gameObject == null)
        {
            throw new InvalidOperationException("GameObject null while getting parent ID");
        }
        Transform parent_transf = transform.gameObject.transform.parent;
        if (parent_transf == null)
        {
            return Entity.NULL_ENTITY;
        }
        IGameObjectManager gomanager = IoCManager.Resolve<IGameObjectManager>();
        return gomanager.GetEntityID(parent_transf.gameObject);
    }
    public static void SetParent(this UCTransform transform, uint parent, bool world_pos_stays = true)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        IGameObjectManager go_manager = IoCManager.Resolve<IGameObjectManager>();
        if (transform.gameObject == null)
        {
            InfoFunc.PrintInfo("UCTransform game object was null during setting a parent");
            return;
        }
        if (parent == Entity.NULL_ENTITY)
        {
            transform.gameObject.transform.SetParent(null, world_pos_stays);
        }
        else
        {
            Component<UCTransform>? target_transform = comp_manager.GetComponent<UCTransform>(parent);
            if (target_transform == null)
            {
                InfoFunc.PrintInfo("UCTransform's parent target doesn't have an UCTransform");
                return;
            }
            if (target_transform.comp.gameObject == null)
            {
                InfoFunc.PrintInfo("UCTransform's parent target doesn't have a GameObject within their UCTransform");
                return;
            }
            transform.gameObject.transform.SetParent(target_transform.comp.gameObject.transform, world_pos_stays);
        }
    }
    public static void UnsetAllChildren(this UCTransform transform, bool world_pos_stays = true)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        IGameObjectManager go_manager = IoCManager.Resolve<IGameObjectManager>();
        foreach (Transform child_transform in transform.GameObject.transform)
        {
            uint child_ent = go_manager.GetEntityID(child_transform.gameObject);
            Component<UCTransform> child_uctransform = comp_manager.AssumeGetComponent<UCTransform>(child_ent);
            child_uctransform.comp.SetParent(Entity.NULL_ENTITY, world_pos_stays);
        }
    }
    public static bool HasChild(this UCTransform transform, uint child)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        IGameObjectManager go_manager = IoCManager.Resolve<IGameObjectManager>();
        foreach (Transform child_transform in transform.GameObject.transform)
        {
            uint child_ent = go_manager.GetEntityID(child_transform.gameObject);
            if (child_ent == child)
            {
                return true;
            }
        }
        return false;
    }
    public static List<uint> GetChildrenEntities(this UCTransform transform)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        IGameObjectManager go_manager = IoCManager.Resolve<IGameObjectManager>();
        List<uint> children = new();
        foreach (Transform child_transform in transform.GameObject.transform)
        {
            uint child_ent = go_manager.GetEntityID(child_transform.gameObject);
            children.Add(child_ent);
        }
        return children;
    }
    public static Component<UCTransform> GetChildTransform(this Component<UCTransform> transform, string child_name)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        IGameObjectManager go_manager = IoCManager.Resolve<IGameObjectManager>();
        foreach (Transform child_transform in transform.comp.GameObject.transform)
        {
            if (child_transform.gameObject.name != child_name)
            {
                continue;
            }
            uint child_ent = go_manager.GetEntityID(child_transform.gameObject);
            return comp_manager.AssumeGetComponent<UCTransform>(child_ent);
        }
        throw new ArgumentException($"Couldn't find a child transform with name {child_name}");
    }
    public static Component<UCTransform> GetParentTransform(this Component<UCTransform> transform)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        IGameObjectManager go_manager = IoCManager.Resolve<IGameObjectManager>();
        GameObject parentgo = transform.comp.GameObject.transform.parent.gameObject;
        return comp_manager.AssumeGetComponent<UCTransform>(go_manager.GetEntityID(parentgo));
    }
    public static Component<UCTransform> GetParentRecursive(this Component<UCTransform> transform, int amount)
    {
        if (amount == 0)
        {
            return transform;
        }
        Component<UCTransform> currentTransform = transform;
        for (int i = 0; i < amount; i++)
        {
            currentTransform = currentTransform.GetParentTransform();
        }
        return currentTransform;
    }
    public static Component<UCTransform> GetChildRecursive(this Component<UCTransform> transform, string[] childNames)
    {
        if (childNames.Length == 0)
        {
            return transform;
        }
        Component<UCTransform> currentTransform = transform;
        foreach (string name in childNames)
        {
            currentTransform = currentTransform.GetChildTransform(name);
        }
        return currentTransform;
    }
    public static void DestroyAllChildren(this UCTransform transform)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        IEntityManager ent_manager = IoCManager.Resolve<IEntityManager>();
        IGameObjectManager go_manager = IoCManager.Resolve<IGameObjectManager>();
        foreach (Transform child_transform in transform.GameObject.transform)
        {
            uint child_ent = go_manager.GetEntityID(child_transform.gameObject);
            ent_manager.DestroyEntity(child_ent);
        }
    }
    public static bool Active(this Component<UCTransform> transform)
    {
        return transform.comp.GameObject.activeSelf;
    }
    public static void SetActive(this Component<UCTransform> transform, bool active)
    {
        transform.comp.GameObject.SetActive(active);
    }
    public static List<Component<UCTransform>> GetChildrenTransforms(this Component<UCTransform> transform)
    {
        List<Component<UCTransform>> list = new List<Component<UCTransform>>();
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        IGameObjectManager go_manager = IoCManager.Resolve<IGameObjectManager>();
        foreach (Transform child_transform in transform.comp.GameObject.transform)
        {
            uint child_ent = go_manager.GetEntityID(child_transform.gameObject);
            list.Add(comp_manager.AssumeGetComponent<UCTransform>(child_ent));
        }
        return list;
    }
}
