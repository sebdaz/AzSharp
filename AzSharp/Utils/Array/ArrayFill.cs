using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Utils.Array;

public static class ArrayFill
{
    public static void Fill<T>(this T[] arr, T value)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = value;
        }
    }
    public static void Fill<T>(this T[,] arr, T value)
    {
        for (int x = 0; x < arr.GetLength(0); x++)
        {
            for (int y = 0; y < arr.GetLength(1); y++)
            {
                arr[x, y] = value;
            }
        }
    }
    public static void Fill<T>(this T[,,] arr, T value)
    {
        for (int x = 0; x < arr.GetLength(0); x++)
        {
            for (int y = 0; y < arr.GetLength(1); y++)
            {
                for (int z = 0; z < arr.GetLength(2); z++)
                {

                    arr[x, y, z] = value;
                }
            }
        }
    }
}
