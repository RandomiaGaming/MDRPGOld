using System.IO;
using System;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class SaveDataManager : MonoBehaviour
{
    public static List<MDRPG_Asset_Pack> LoadedAssetPacks = new List<MDRPG_Asset_Pack>();
    public static MDRPG_SaveFile LoadedSaveFile = new MDRPG_SaveFile();

    void Awake()
    {
        ReloadAllAssetPacks();
    }
    public static void ReloadAllAssetPacks()
    {
        LoadedAssetPacks = new List<MDRPG_Asset_Pack>();
        Directory.CreateDirectory(Application.persistentDataPath + "/Asset Packs");
        foreach (string PackPath in Directory.GetDirectories(Application.persistentDataPath + "/Asset Packs"))
        {
            LoadAssetPack(PackPath.Split(new char[] { '\\', '/' })[PackPath.Split(new char[] { '\\', '/' }).Length - 1]);
        }
    }
    public static void LoadAssetPack(string Name)
    {
        Directory.CreateDirectory(Application.persistentDataPath + "/Asset Packs");
        if (!Directory.Exists(Application.persistentDataPath + "/Asset Packs/" + Name))
        {
            Debug.LogError($"Asset pack at path '{Application.persistentDataPath + "/Asset Packs/" + Name}' could not be found!");
            return;
        }
        else
        {
            for (int i = 0; i < LoadedAssetPacks.Count; i++)
            {
                if (LoadedAssetPacks[i].Name == Name)
                {
                    LoadedAssetPacks.RemoveAt(i);
                }
            }
            List<Texture2D> LoadedTextures = new List<Texture2D>();
            if (Directory.Exists(Application.persistentDataPath + "/Asset Packs/" + Name + "/Textures"))
            {
                foreach (string TexturePath in Directory.GetFiles(Application.persistentDataPath + "/Asset Packs/" + Name + "/Textures"))
                {
                    Texture2D LoadedTexture = new Texture2D(1, 1);
                    ImageConversion.LoadImage(LoadedTexture, File.ReadAllBytes(TexturePath), false);
                    LoadedTextures.Add(LoadedTexture);
                }
            }
            else
            {
                Debug.LogError($"Textures directory not found within {Application.persistentDataPath + "/Asset Packs/" + Name}!");
                return;
            }
            List<MDRPG_Item> LoadedItems = new List<MDRPG_Item>();
            if (Directory.Exists(Application.persistentDataPath + "/Asset Packs/" + Name + "/Items"))
            {
                foreach (string ItemPath in Directory.GetFiles(Application.persistentDataPath + "/Asset Packs/" + Name + "/Items"))
                {
                    MDRPG_Item LoadedItem = (MDRPG_Item)ReadFromHD(ItemPath, typeof(MDRPG_Item));
                    if (LoadedItem != null)
                    {
                        LoadedItems.Add(LoadedItem);
                    }
                    else
                    {
                        Debug.LogError($"Json error loading item at path {Application.persistentDataPath + "/Asset Packs/" + Name + "/Items"}!");
                        return;
                    }
                }
            }
            else
            {
                Debug.LogError($"Items directory not found within {Application.persistentDataPath + "/Asset Packs/" + Name}!");
                return;
            }
            List<MDRPG_Object> LoadedObjects = new List<MDRPG_Object>();
            if (Directory.Exists(Application.persistentDataPath + "/Asset Packs/" + Name + "/Objects"))
            {
                foreach (string ObjectPath in Directory.GetFiles(Application.persistentDataPath + "/Asset Packs/" + Name + "/Objects"))
                {
                    MDRPG_Object LoadedObject = (MDRPG_Object)ReadFromHD(ObjectPath, typeof(MDRPG_Object));
                    if (LoadedObject != null)
                    {
                        LoadedObjects.Add(LoadedObject);
                    }
                    else
                    {
                        Debug.LogError($"Json error loading item at path {Application.persistentDataPath + "/Asset Packs/" + Name + "/Object"}!");
                        return;
                    }
                }
            }
            else
            {
                Debug.LogError($"Object directory not found within {Application.persistentDataPath + "/Asset Packs/" + Name}!");
                return;
            }
            List<MDRPG_Biome> LoadedBiomes = new List<MDRPG_Biome>();
            if (Directory.Exists(Application.persistentDataPath + "/Asset Packs/" + Name + "/Biomes"))
            {
                foreach (string BiomePath in Directory.GetFiles(Application.persistentDataPath + "/Asset Packs/" + Name + "/Biomes"))
                {
                    MDRPG_Biome LoadedBiome = (MDRPG_Biome)ReadFromHD(BiomePath, typeof(MDRPG_Biome));
                    if (LoadedBiome != null)
                    {
                        LoadedBiomes.Add(LoadedBiome);
                    }
                    else
                    {
                        Debug.LogError($"Json error loading item at path {Application.persistentDataPath + "/Asset Packs/" + Name + "/Biome"}!");
                        return;
                    }
                }
            }
            else
            {
                Debug.LogError($"Biome directory not found within {Application.persistentDataPath + "/Asset Packs/" + Name}!");
                return;
            }
            List<MDRPG_Demention> LoadedDementions = new List<MDRPG_Demention>();
            if (Directory.Exists(Application.persistentDataPath + "/Asset Packs/" + Name + "/Dementions"))
            {
                foreach (string DementionPath in Directory.GetFiles(Application.persistentDataPath + "/Asset Packs/" + Name + "/Dementions"))
                {
                    MDRPG_Demention LoadedDemention = (MDRPG_Demention)ReadFromHD(DementionPath, typeof(MDRPG_Demention));
                    if (LoadedDemention != null)
                    {
                        LoadedDementions.Add(LoadedDemention);
                    }
                    else
                    {
                        Debug.LogError($"Json error loading item at path {Application.persistentDataPath + "/Asset Packs/" + Name + "/Demention"}!");
                        return;
                    }
                }
            }
            else
            {
                Debug.LogError($"Demention directory not found within {Application.persistentDataPath + "/Asset Packs/" + Name}!");
                return;
            }
            MDRPG_Asset_Pack LoadedAssetPack = new MDRPG_Asset_Pack();
            LoadedAssetPack.Biomes = LoadedBiomes;
            LoadedAssetPack.Dementions = LoadedDementions;
            LoadedAssetPack.Items = LoadedItems;
            LoadedAssetPack.Name = Name;
            LoadedAssetPack.Objects = LoadedObjects;
            LoadedAssetPacks.Add(LoadedAssetPack);
        }
    }

    public static string SerializeToJson(object Original)
    {
        MemoryStream memoryStream = new MemoryStream();
        DataContractJsonSerializer JsonSerializer = new DataContractJsonSerializer(Original.GetType());
        JsonSerializer.WriteObject(memoryStream, Original);
        return Encoding.ASCII.GetString(memoryStream.ToArray());
    }
    public static object DeserializeFromJson(string Json, Type TypeOfOriginal)
    {
        try
        {
            byte[] JsonToBytes = Encoding.ASCII.GetBytes(Json);
            MemoryStream memoryStream = new MemoryStream(JsonToBytes);
            DataContractJsonSerializer ser1 = new DataContractJsonSerializer(TypeOfOriginal);
            object Output = ser1.ReadObject(memoryStream);
            return Output;
        }
        catch
        {
            return null;
        }
    }
    public static object ReadFromHD(string Path, Type TypeOfOriginal)
    {
        if (File.Exists(Path))
        {
            FileStream stream = new FileStream(Path, FileMode.Open);
            StreamReader streamReader = new StreamReader(stream);
            string fileJson = streamReader.ReadToEnd();
            streamReader.Dispose();
            stream.Close();
            return DeserializeFromJson(fileJson, TypeOfOriginal);
        }
        else
        {
            return null;
        }
    }
    public static void WriteToHD(object Original, string Path)
    {
        FileStream stream = new FileStream(Path, FileMode.Create);
        StreamWriter streamWriter = new StreamWriter(stream);
        streamWriter.Write(SerializeToJson(Original));
        streamWriter.Dispose();
        stream.Close();
    }
}