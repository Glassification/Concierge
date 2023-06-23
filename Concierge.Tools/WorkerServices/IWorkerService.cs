// <copyright file="IWorkerService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Tools.WorkerServices
{
    public interface IWorkerService
    {
        void StartWorker(string message);

        void StopWorker();
    }
}
