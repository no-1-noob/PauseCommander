using PauseCommander.Logic;
using Zenject;

namespace PauseCommander.Installers
{
    internal class PauseSongInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PauseSongMgr>().AsSingle();
        }
    }
}
