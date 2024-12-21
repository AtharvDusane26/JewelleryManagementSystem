﻿using JewelleryManagementSystem.ModelUtilities;
using JewelleryManagementSystem.OrnamentManagement.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelleryManagementSystem.Settings.Model
{
    public sealed class MetalManager : CommonComponent
    {
        private List<IMetal> _availableMetals;
        private string _filePath = Path.Combine(DirectoryPath.MetalDirectory, "Metals.xml");

        private static MetalManager _instance;
        private MetalManager()
        {
            LoadDefaultMetals();
        }
        public static MetalManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MetalManager();
                return _instance;
            }
        }
        public List<IMetal> AvailableMetals => _availableMetals;
        public bool AddOrUpdateMetal(string metalName, WeightType weightType, float metalRate, WeightType weightTypeForMaking, float makingCharges)
        {
            IMetal metal = null;
            if (string.IsNullOrWhiteSpace(metalName))
                return false;
            if (_availableMetals != null && _availableMetals.Any(o => o.MetalName.ToLower().Trim() == metalName.ToLower().Trim()))
            {
                metal = _availableMetals.FirstOrDefault(o => o.MetalName.ToLower().Trim() == metalName.ToLower().Trim());
                metal.MetalName = metalName;
                metal.MetalRate = metalRate;
                metal.MakingCharges = makingCharges;
                metal.WeightTypeForRate = weightType;
                metal.WeightTypeForMaking = weightTypeForMaking;
            }
            else
            {
                metal = new Metal();
                metal.MetalName = metalName;
                metal.MetalRate = metalRate;
                metal.MakingCharges = makingCharges;
                metal.WeightTypeForRate = weightType;
                metal.WeightTypeForMaking = weightTypeForMaking;
                _availableMetals.Add(metal);
            }
            SaveMetalList();
            return true;
        }
        private void SaveMetalList()
        {
            try
            {
                if (_availableMetals != null)
                    DataManager.Save(_availableMetals, _filePath);
            }
            catch (Exception ex)
            {
                OnShowMessageBox?.Invoke($"Unable to save metals{ex.Message}");
            }

        }
        private void LoadDefaultMetals()
        {
            GetAvailableMetals();
            if (_availableMetals.Count == 0)
            {
                _availableMetals.Add(new Metal() { MetalName = "Gold 24 cr.", MetalRate = 7600, WeightTypeForRate = WeightType.InGram, MakingCharges = 1000, WeightTypeForMaking = WeightType.InGram });
                _availableMetals.Add(new Metal() { MetalName = "Gold 22 cr.", MetalRate = 7400, WeightTypeForRate = WeightType.InGram, MakingCharges = 800, WeightTypeForMaking = WeightType.InGram });
                _availableMetals.Add(new Metal() { MetalName = "Silver ", MetalRate = 90000, WeightTypeForRate = WeightType.InKiloGram, MakingCharges = 30, WeightTypeForMaking = WeightType.InGram });
                SaveMetalList();
            }
        }
        private void GetAvailableMetals()
        {
            try
            {
                var list = DataManager.Read<List<IMetal>>(_filePath);
                if (list == null)
                    _availableMetals = new List<IMetal>();
                _availableMetals = list;
            }
            catch (Exception ex)
            {
                _availableMetals = new List<IMetal>();
                OnShowMessageBox?.Invoke($"Unable to load metals{ex.Message}");
            }
        }

    }
}