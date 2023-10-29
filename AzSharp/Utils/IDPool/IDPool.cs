using System;
using System.Collections.Generic;

namespace AzSharp.Utils.IDPool;

public class IDPool
{
    private uint next_id = 0;
    private List<uint> recycled_ids = new();
    public uint GetNextID()
    {
        uint return_value;
        if(recycled_ids.Count != 0)
        {
            return_value = recycled_ids[0];
            recycled_ids.Remove(return_value);
        }
        else
        {
            return_value = next_id;
            next_id++;
        }
        return return_value;
    }
    public void FreeID(uint id_to_free)
    {
        recycled_ids.Add(id_to_free);
    }
    public void Reset()
    {
        next_id = 0;
        recycled_ids.Clear();
    }
}
