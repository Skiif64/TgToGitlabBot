﻿using Bot.Core.Entities;
using Bot.Integration.Git;
using Bot.Integration.Git.GitCommands;
using LibGit2Sharp;
using System.Security.Cryptography;
using System.Text;

namespace Bot.Git.Tests;

public class AddFileCommandTests
{
	private const string REPOSITORY_PATH = "test-repository";
    

	public AddFileCommandTests()
	{
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
	}

	[SetUp]
	public void ClearDirectory()
	{
		if(Directory.Exists(REPOSITORY_PATH))
			Directory.Delete(REPOSITORY_PATH, true);
		Directory.CreateDirectory(REPOSITORY_PATH);
	}

	[TestCase("UTF-8.txt", "32158db1e9eb4a80c34f1f1ae11f5868ad80606c1a6e1ac4fbe95e6228da0c34")]
    [TestCase("WINDOWS-1251.txt", "35beb404565d5baab7bd964b9181ec3d9a730ec35320695b441eb6b5c889d7f4")]
    public void WhenAddFile_ThenSHA256HashShouldBeEquals(string filename, string expectedHash)
	{
		var stream = new MemoryStream(GetFileBytes($"Fixtures/{filename}"));		
        var sha256 = SHA256.Create();
		var commitInfo = new CommitInfo
		{
			Content = stream,
			Message = "message",
			FileName = filename
		};

		var optionsSection = new GitOptionsSection
		{
			LocalPath = REPOSITORY_PATH
		};

		var result = new AddFileCommand(commitInfo, optionsSection).Execute(null!);
		var filepath = Path.Combine(REPOSITORY_PATH, commitInfo.FileName);
		var actualhash = sha256.ComputeHash(GetFileBytes(filepath));
		var actualhashString = ByteArrayToHexString(actualhash);
		Assert.IsTrue(result);
		Assert.That(actualhashString, Is.EqualTo(expectedHash));
		
	}

	private Stream GetStreamFromFile(string filepath)
	{
		var memoryStream = new MemoryStream();
		var binaryWriter = new BinaryWriter(memoryStream);
		using var fileStream = new FileStream(filepath, FileMode.Open);
		using var binaryReader = new BinaryReader(fileStream);
		binaryWriter.Write(binaryReader.ReadBytes((int)fileStream.Length));
		return memoryStream;
	}

	private byte[] GetFileBytes(string filepath)
	{
		using var fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
		using var binaryReader = new BinaryReader(fileStream);
		return binaryReader.ReadBytes((int)fileStream.Length);
	}

	private string ByteArrayToHexString(byte[] bytes)
	{
		string output = string.Empty;
		foreach(var @byte in bytes)
		{
			output += $"{@byte:x2}";
		}
		return output;
	}
}