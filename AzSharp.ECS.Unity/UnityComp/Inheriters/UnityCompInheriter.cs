using AzSharp.ECS.Shared.Components;
using AzSharp.IoC;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public interface IUnityCompInheriterInternal
{
    public void TryInherit(GameObject gameobject, uint entity_id);
}

public abstract class UnityCompInheriter<T> : IUnityCompInheriterInternal
    where T : new()
{
    public void TryInherit(GameObject gameobject, uint entity_id)
    {
        T component = new();
        if (LoadFromSceneObject(component, gameobject))
        {
            IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
            comp_manager.AddComponent<T>(component, entity_id);
        }
    }
    public abstract bool LoadFromSceneObject(T component, GameObject gameobject);
}

public static class UnityCompInheriter
{
    private static Dictionary<Type, IUnityCompInheriterInternal> InheritorsDict = new();
    private static void TryInherit<T>(GameObject gameobject, uint entity_id)
        where T : IUnityCompInheriterInternal, new()
    {
        Type type = typeof(T);
        if (!InheritorsDict.ContainsKey(type))
        {
            InheritorsDict[type] = new T();
        }
        IUnityCompInheriterInternal inheritor = InheritorsDict[type];
        inheritor.TryInherit(gameobject, entity_id);
    }
    public static void DoInheritance(GameObject gameobject, uint entity_id)
    {
        TryInherit<UCTransformInheriter>(gameobject, entity_id);

        TryInherit<UCAudioListenerInheriter>(gameobject, entity_id);
        TryInherit<UCBoxCollider2DInheriter>(gameobject, entity_id);
        TryInherit<UCButtonInheriter>(gameobject, entity_id);
        TryInherit<UCCameraInheriter>(gameobject, entity_id);
        TryInherit<UCCanvasInheriter>(gameobject, entity_id);
        TryInherit<UCCanvasRendererInheriter>(gameobject, entity_id);
        TryInherit<UCCanvasScalerInheriter>(gameobject, entity_id);
        TryInherit<UCEventSystemInheriter>(gameobject, entity_id);
        TryInherit<UCGraphicRaycasterInheriter>(gameobject, entity_id);
        TryInherit<UCHorizontalLayoutInheriter>(gameobject, entity_id);
        TryInherit<UCImageInheriter>(gameobject, entity_id);
        TryInherit<UCLightInheriter>(gameobject, entity_id);
        TryInherit<UCMeshFilterInheriter>(gameobject, entity_id);
        TryInherit<UCMeshRendererInheriter>(gameobject, entity_id);
        TryInherit<UCMouseEventsInheriter>(gameobject, entity_id);
        TryInherit<UCPhysics2DRaycasterInheriter>(gameobject, entity_id);
        TryInherit<UCSliderInheriter>(gameobject, entity_id);
        TryInherit<UCStandaloneInputModuleInheriter>(gameobject, entity_id);
        TryInherit<UCTextMeshProInheriter>(gameobject, entity_id);
        TryInherit<UCTextMeshProUGUIInheriter>(gameobject, entity_id);
        TryInherit<UCTMPInputFieldInheriter>(gameobject, entity_id);
    }
}
