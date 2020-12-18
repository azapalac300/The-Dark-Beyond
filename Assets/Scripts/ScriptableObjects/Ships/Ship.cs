﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ship : MonoBehaviour
{
    //Ship is a container that manages ship data
    public ShipData currentShip;

    private Dictionary<ShipModuleType, ShipModule> ModuleDict;

    public List<ShipModule> modules;

    // Start is called before the first frame update
    void Start()
    {
        Starmap.JumpEvent += JumpTo;
        ModuleDict = new Dictionary<ShipModuleType, ShipModule>();

        for(int i = 0; i < modules.Count; i++)
        {
            ModuleDict.Add(modules[i].Type, modules[i]);

        }

        PlayerInput.PrimaryFired += () => FirePrimary();

        PlayerInput.SecondaryFired += () => FireSecondary();
    }



    // Update is called once per frame
    void Update()
    {
        UpdateModules();
    }

    public float HP_ratio()
    {
        if(HasModule<Hull>()){
            Hull hull = GetModule<Hull>();

            return (float)hull.CurrHP / (float)hull.maxHP;
        }

        return -1;
    }

    public float Shields_ratio()
    {
        if (HasModule<Shields>())
        {
            Shields shields = GetModule<Shields>();
            return (float)shields.CurrShields / (float)shields.maxShields;
        }
        return -1;
    }

    public float JumpFuel_ratio()
    {
        if (HasModule<JumpDrive>())
        {
            JumpDrive jumpDrive = GetModule<JumpDrive>();
            return jumpDrive.CurrFuel / jumpDrive.fuelCapacity;
        }
        return -1;
    }

    public void UpdateModules()
    {
        foreach(ShipModule module in ModuleDict.Values)
        {
            module.UpdateModule();
        }
    }
   
    public void JumpTo(Vector3 destination)
    {
        if (HasModule<JumpDrive>())
        {
            GetModule<JumpDrive>().JumpTo(destination, transform);
        }
    }

    public void FirePrimary()
    {
        if (HasModule(ShipModuleType.PrimaryWeapon))
        {
            ((Weapon)GetModule(ShipModuleType.PrimaryWeapon)).Fire(gameObject);

        }
    }

    public void FireSecondary()
    {
        if (HasModule(ShipModuleType.SecondaryWeapon))
        {
            ((Weapon)GetModule(ShipModuleType.SecondaryWeapon)).Fire(gameObject);
        }
    }

    //Et cetera



    public bool HasModule(ShipModuleType type)
    {
        return ModuleDict.ContainsKey(type);
    }

    public ShipModule GetModule(ShipModuleType type)
    {
        return ModuleDict[type];
    }

    public bool HasModule<M>() where M : ShipModule
    {
        return ModuleDict.ContainsKey(GetModuleType<M>());
    }


    public M GetModule<M>() where M : ShipModule
    {

        return (M)ModuleDict[GetModuleType<M>()];
    }


    public ShipModuleType GetModuleType<M> () where M: ShipModule
    {
       
       return (ShipModuleType)Enum.Parse(typeof(ShipModuleType), typeof(M).Name);
    }

}