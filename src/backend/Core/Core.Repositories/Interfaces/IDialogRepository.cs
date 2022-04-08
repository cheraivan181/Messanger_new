﻿using Core.DbModels;

namespace Core.Repositories.Interfaces;

public interface IDialogRepository
{
    Task<(List<Dialog> dialogs, List<DialogRequest> dialogRequest)>
        GetUserDialogsAndDialogRequestsAsync(long userId, string predicate);
}