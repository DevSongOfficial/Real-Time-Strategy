using System;
using UnityEngine;

public class UnitCapacitySlots
{
    private int maxCapacity;
    private int usedCapacity;

    public int FreeCapacity => maxCapacity - usedCapacity;

    private ResourceView view;

    public UnitCapacitySlots(int maxCapacity, ResourceView view)
    {
        this.maxCapacity = maxCapacity;

        this.view = view;
        view.UpdateCapacityText(usedCapacity, maxCapacity);
    }

    public void Occupy(int slots)
    {
        usedCapacity += slots;
        usedCapacity = Math.Clamp(usedCapacity, 0, maxCapacity);
        view.UpdateCapacityText(usedCapacity, maxCapacity);
    }

    public void Release(int slots)
    {
        usedCapacity -= slots;
        usedCapacity = Math.Clamp(usedCapacity, 0, maxCapacity);
        view.UpdateCapacityText(usedCapacity, maxCapacity);
    }
}