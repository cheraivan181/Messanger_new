using Core.ApiResponses.Dialog;
using Core.DialogServices.Domain;
using Core.Mapping.Interfaces;

namespace Core.Mapping.Impl;

public class DialogMapper : IDialogMapper
{
    public CreateDialogResponse Map(CreateDialogRequestResult createDialogResult)
    {
        var result = new CreateDialogResponse();
        result.Key = createDialogResult.Key;
        result.DialogId = createDialogResult.DialogId;
        result.DialogRequestId = createDialogResult.DialogRequestId;

        return result;
    }

    public GetDialogResponse Map(GetDialogResult getDialogResult)
    {
        var result = new GetDialogResponse();
        result.Dialogs = getDialogResult.Dialogs;
        result.Count = getDialogResult.Count;

        return result;
    }
}