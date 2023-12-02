﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Utils.IDPool;

public struct IDHandle
{
    public uint ID;
    public uint generation;
    public IDHandle(uint ID, uint generation)
    {
        this.ID = ID;
        this.generation = generation;
    }
}

public sealed class IDPoolGenerational
{
    private List<uint> idList = new List<uint>();
    private uint next_id = 0;
    private List<uint> recycled_ids = new();
    public uint GetNextID()
    {
        uint return_value;
        if (recycled_ids.Count != 0)
        {
            return_value = recycled_ids[0];
            recycled_ids.Remove(return_value);
        }
        else
        {
            return_value = next_id;
            idList.Add(0);
            next_id++;
        }
        return return_value;
    }
    public void FreeID(uint id_to_free)
    {
        idList[(int)id_to_free]++;
        recycled_ids.Add(id_to_free);
    }
    public void Reset()
    {
        next_id = 0;
        recycled_ids.Clear();
    }
    public IDHandle GetHandle(uint ID)
    {
        return new IDHandle(ID, idList[(int)ID]);
    }
    public bool ValidHandle(IDHandle handle)
    {
        uint generation = idList[(int)handle.ID];
        return generation == handle.generation;
    }
}
