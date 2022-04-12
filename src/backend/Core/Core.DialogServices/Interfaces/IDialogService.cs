﻿using Core.DialogServices.Domain;

namespace Core.DialogServices.Interfaces;

public interface IDialogService
{
    Task<CreateDialogRequestResult> CreateDialogRequestAsync(Guid ownerUserId,
        Guid requestUserId,
        string ownerSessionId);
}