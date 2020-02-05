using System;
using System.Collections.Generic;
using System.Text;

using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;

namespace FirstApp.ViewModels.Helper
{
    class PermissionsHelper
    {
        public static async Task<PermissionStatus> GetPermission(Permission permissionType)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permissionType);
            if (status != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(permissionType);
                if (results.ContainsKey(permissionType))
                {
                    status = results[permissionType];
                }
            }

            return status;
        }
    }
}
