using System;
using Managers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Installers
{
    public class SoundManagerInstaller : LifetimeScope
    {
        [SerializeField] private SoundManager _soundManager;
        
        protected override void Configure(IContainerBuilder builder)
        {
            if (_soundManager == null) throw new Exception("SoundManager is null");
            builder.RegisterComponent(_soundManager);
        }
    }
}