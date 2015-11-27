using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace RegisterAssembly
{




    [RunInstaller(true)]

    public partial class RegasmInstaller : Installer
    {

        public RegasmInstaller()
        {

            InitializeComponent();

        }

        public override void Install(IDictionary stateSaver)
        {

            base.Install(stateSaver);

            Regasm(true);

        }

        public override void Rollback(IDictionary savedState)
        {

            base.Rollback(savedState);

            Regasm(false);

        }

        public override void Uninstall(IDictionary savedState)
        {

            base.Rollback(savedState);

            Regasm(false);

        }

        private void Regasm(bool register)
        {

            string file = base.Context.Parameters["Assembly"];

            if (string.IsNullOrEmpty(file))

                throw new InstallException("Assembly not defined");

            if (!File.Exists(file))

                return;

            RegistrationServices regsrv = new RegistrationServices();

            Assembly assembly = Assembly.LoadFrom(file);

            if (register)
            {

                regsrv.RegisterAssembly(assembly, AssemblyRegistrationFlags.SetCodeBase);

            }

            else
            {

                try
                {

                    regsrv.UnregisterAssembly(assembly);

                }

                catch
                {

                    //Exceptions are ignored: even if the unregistering failes

                    //it should notprevent the user from uninstalling

                }

            }

        }

    }


}
