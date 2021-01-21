﻿using System;
using System.IO;
using System.Reflection;
namespace MDRPG
{
    public sealed class AssetCodecInfo
    {
        public readonly string[] managedExtensions = new string[0];
        public readonly MethodInfo codecMethod = null;
        public AssetCodecInfo(RegisterAssetCodecAttribute assetCodecAttribute, MethodInfo codecMethod)
        {
            if (assetCodecAttribute is null)
            {
                throw new NullReferenceException();
            }
            managedExtensions = assetCodecAttribute.managedExtensions;
            if (codecMethod is null)
            {
                throw new NullReferenceException();
            }
            if (!codecMethod.IsStatic || !codecMethod.IsPublic || !typeof(AssetBase).IsAssignableFrom(codecMethod.ReturnType))
            {
                throw new ArgumentException();
            }
            ParameterInfo[] codecMethodParameters = codecMethod.GetParameters();
            if (codecMethodParameters is null || codecMethodParameters.Length != 2)
            {
                throw new ArgumentException();
            }
            if (codecMethodParameters[0].IsOut || codecMethodParameters[0].ParameterType != typeof(Stream))
            {
                throw new ArgumentException();
            }
            if (codecMethodParameters[1].IsOut || codecMethodParameters[1].ParameterType != typeof(string))
            {
                throw new ArgumentException();
            }
            this.codecMethod = codecMethod;
        }
        public AssetBase LoadAsset(Stream stream, string name)
        {
            return (AssetBase)codecMethod.Invoke(null, new object[] { stream, name });
        }
    }
}
