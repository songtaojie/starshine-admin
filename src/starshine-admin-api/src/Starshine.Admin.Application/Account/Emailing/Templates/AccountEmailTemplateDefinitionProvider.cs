﻿using Starshine.Admin.Localization;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;

namespace Volo.Abp.Account.Emailing.Templates;

public class AccountEmailTemplateDefinitionProvider : TemplateDefinitionProvider
{
    public override void Define(ITemplateDefinitionContext context)
    {
        context.Add(
            new TemplateDefinition(
                AccountEmailTemplates.PasswordResetLink,
                displayName: LocalizableString.Create<AdminResource>($"TextTemplate:{AccountEmailTemplates.PasswordResetLink}"),
                layout: StandardEmailTemplates.Layout,
                localizationResource: typeof(AdminResource)
            ).WithVirtualFilePath("/Volo/Abp/Account/Emailing/Templates/PasswordResetLink.tpl", true)
        );
    }
}
