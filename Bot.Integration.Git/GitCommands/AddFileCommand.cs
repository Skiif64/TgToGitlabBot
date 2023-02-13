﻿using Bot.Core.Entities;
using LibGit2Sharp;
using System.Security.Principal;

namespace Bot.Integration.Git.GitCommands;

internal class AddFileCommand : IGitCommand
{    
    private readonly CommitInfo _commitInfo;
    private readonly GitOptionsSection _optionsSection;    

    public AddFileCommand(CommitInfo commitInfo, GitOptionsSection optionsSection)
    {
        _commitInfo = commitInfo;
        _optionsSection = optionsSection;       
    }
    public bool Execute(IRepository repository)
    {       
        if (_commitInfo.Content is null)
            throw new ArgumentNullException(nameof(_commitInfo.Content));
        string filepath;
        if (_optionsSection.FilePath is not null)
            filepath = Path.Combine(_optionsSection.FilePath, _commitInfo.FileName);
        else
            filepath = _commitInfo.FileName;        
        using (var fileStream = new FileStream(Path.Combine(_optionsSection.LocalPath, filepath), FileMode.OpenOrCreate, FileAccess.Write))
        {
            using var writer = new BinaryWriter(fileStream);
            using var reader = new BinaryReader(_commitInfo.Content);
            writer.Write(reader.ReadBytes((int)_commitInfo.Content.Length));
        }
        return true;
    }
}
