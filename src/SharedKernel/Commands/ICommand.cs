﻿namespace SharedKernel.Commands;

public interface ICommand;

public interface ICommand<TResult> : ICommand;