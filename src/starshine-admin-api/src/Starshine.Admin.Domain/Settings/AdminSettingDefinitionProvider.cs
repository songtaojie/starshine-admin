using Starshine.Admin.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Starshine.Admin.Settings;

public class AdminSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(AdminSettings.MySetting1));
        context.Add(
               new SettingDefinition(
                   AdminSettingNames.IsSelfRegistrationEnabled,
                   "true",
                   L("DisplayName:Abp.Account.IsSelfRegistrationEnabled"),
                   L("Description:Abp.Account.IsSelfRegistrationEnabled"), isVisibleToClients: true)
           );

        context.Add(
            new SettingDefinition(
                AdminSettingNames.EnableLocalLogin,
                "true",
                L("DisplayName:Abp.Account.EnableLocalLogin"),
                L("Description:Abp.Account.EnableLocalLogin"), isVisibleToClients: true)
        );
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AdminResource>(name);
    }
}
