using Core.ApiResponses.Dialog;
using Core.DialogServices.Domain;

namespace Core.Mapping.Interfaces;

public interface IDialogMapper
{
    CreateDialogResponse Map(CreateDialogRequestResult createDialogResult);
    GetDialogResponse Map(GetDialogResult getDialogResult);
}