using Microsoft.Security.ApplicationId.PolicyManagement.PolicyEngine;
using Microsoft.Security.ApplicationId.PolicyManagement.PolicyModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.Security.ApplicationId.PolicyManagement
{
  public static class FileManager
  {
    public static FileInformation GetFileInformation(string filePath) => FileManager.GetFileInformation(filePath, true, true);

    public static FileInformation GetFileInformation(
      string filePath,
      bool collectPublisherInformation,
      bool collectHashInformation)
    {
      FileInformation fileInformation = File.Exists(filePath) ? new FileInformation(FileManager.NormalizeFilePath(filePath)) : throw new FileDoesNotExistException(filePath);
      if (FileManager.GetFileType(filePath) == AppLockerFileType.AppX)
        fileInformation.AppX = true;
      if (collectPublisherInformation)
      {
        FilePublisher filePublisher;
        try
        {
          filePublisher = FileManager.GetFilePublisher(filePath, fileInformation.AppX);
        }
        catch (CreateFilePublisherException ex)
        {
          filePublisher = (FilePublisher) null;
        }
        fileInformation.Publisher = filePublisher;
      }
      if (collectHashInformation)
      {
        if (!fileInformation.AppX)
        {
          FileHash fileHash;
          try
          {
            fileHash = FileManager.GetFileHash(filePath);
          }
          catch (CreateFileHashException ex)
          {
            fileHash = (FileHash) null;
          }
          fileInformation.Hash = fileHash;
        }
      }
      return fileInformation;
    }

    public static FilePublisher GetFilePublisher(string filePath) => FileManager.GetFilePublisher(filePath, false);

    public static FilePublisher GetFilePublisher(string filePath, bool isAppX)
    {
      if (!File.Exists(filePath))
        throw new FileDoesNotExistException(filePath);
      IAppIdPolicyHelper appIdPolicyHelper = (IAppIdPolicyHelper) new AppIdPolicyHelperClass();
      string pbstrPublisherName;
      string pbstrProductName;
      string pbstrBinaryName;
      ulong pulBinaryVersion;
      try
      {
        if (isAppX)
          appIdPolicyHelper.CalculateAppxFilePublisher(filePath, out pbstrPublisherName, out pbstrProductName, out pbstrBinaryName, out pulBinaryVersion);
        else
          appIdPolicyHelper.CalculateFilePublisher(filePath, out pbstrPublisherName, out pbstrProductName, out pbstrBinaryName, out pulBinaryVersion);
      }
      catch (COMException ex)
      {
        if (ex.ErrorCode == -2146958844)
          throw new FrameworkPackageException(filePath, (Exception) ex);
        throw new CreateFilePublisherException(filePath, (Exception) ex);
      }
      return new FilePublisher(pbstrPublisherName, pbstrProductName, pbstrBinaryName, new FileVersion(pulBinaryVersion));
    }

    public static FileHash GetFileHash(string filePath)
    {
      if (!File.Exists(filePath))
        throw new FileDoesNotExistException(filePath);
      IAppIdPolicyHelper appIdPolicyHelper = (IAppIdPolicyHelper) new AppIdPolicyHelperClass();
      byte[] fileHash1;
      try
      {
        fileHash1 = appIdPolicyHelper.CalculateFileHash(filePath);
      }
      catch (COMException ex)
      {
        throw new CreateFileHashException(filePath, (Exception) ex);
      }
      FileHash fileHash2 = new FileHash(fileHash1);
      FileInfo fileInfo = new FileInfo(filePath);
      fileHash2.SourceFileName = fileInfo.Name;
      fileHash2.SourceFileLength = fileInfo.Length;
      return fileHash2;
    }

    public static string NormalizeFilePath(string filePath)
    {
      if (!File.Exists(filePath))
        throw new FileDoesNotExistException(filePath);
      IAppIdPolicyHelper appIdPolicyHelper = (IAppIdPolicyHelper) new AppIdPolicyHelperClass();
      try
      {
        return appIdPolicyHelper.NormalizeFilePath(filePath);
      }
      catch (COMException ex)
      {
        throw new NormalizeFilePathException(filePath, (Exception) ex);
      }
    }

    public static AppLockerFileType GetFileType(string filePath)
    {
      if (filePath == null)
        throw new ArgumentNullException(nameof (filePath));
      IAppIdPolicyHelper appIdPolicyHelper = (IAppIdPolicyHelper) new AppIdPolicyHelperClass();
      FILE_TYPE fileType;
      try
      {
        fileType = appIdPolicyHelper.GetFileType(filePath);
      }
      catch (COMException ex)
      {
        throw new GetFileTypeException(filePath, (Exception) ex);
      }
      return FileManager.TranslateFileType(fileType);
    }

    public static bool IsFileTypeSupported(string filePath) => FileManager.GetFileType(filePath) != 0;

    public static void VerifyFileTypeSupported(string filePath)
    {
      if (!FileManager.IsFileTypeSupported(filePath))
        throw new UnsupportedFileTypeException(filePath);
    }

    public static AppLockerFileType GetFileType(FileInformation fileInformation) => FileManager.GetFileType(FileManager.GetFilePath(fileInformation));

    public static bool IsFileTypeSupported(FileInformation fileInformation) => FileManager.IsFileTypeSupported(FileManager.GetFilePath(fileInformation));

    public static void VerifyFileTypeSupported(FileInformation fileInformation) => FileManager.VerifyFileTypeSupported(FileManager.GetFilePath(fileInformation));

    public static ICollection<string> SearchFiles(
      string directory,
      ICollection<AppLockerFileType> filesTypes,
      bool recurse,
      out ICollection<string> errorPaths)
    {
      if (directory == null)
        throw new ArgumentNullException(nameof (directory));
      if (filesTypes == null)
        throw new ArgumentNullException(nameof (filesTypes));
      List<string> fileExtensions = new List<string>();
      foreach (AppLockerFileType filesType in (IEnumerable<AppLockerFileType>) filesTypes)
      {
        if (filesType != AppLockerFileType.NotSupported)
          fileExtensions.AddRange((IEnumerable<string>) FileManager.GetFileExtensions(filesType));
      }
      return FileManager.SearchFiles(directory, (ICollection<string>) fileExtensions, recurse, out errorPaths);
    }

    private static ICollection<string> SearchFiles(
      string directory,
      ICollection<string> fileExtensions,
      bool recurse,
      out ICollection<string> errorPaths)
    {
      List<string> stringList = new List<string>();
      errorPaths = (ICollection<string>) new List<string>();
      Stack<string> stringStack = new Stack<string>();
      stringStack.Push(directory);
      while (stringStack.Count > 0)
      {
        string path = stringStack.Pop();
        try
        {
          foreach (string fileExtension in (IEnumerable<string>) fileExtensions)
          {
            string str = "." + fileExtension;
            string searchPattern = "*" + str;
            foreach (string file in (IEnumerable<string>) Directory.GetFiles(path, searchPattern))
            {
              if (file.EndsWith(str, StringComparison.OrdinalIgnoreCase))
                stringList.Add(file);
            }
          }
          if (recurse)
          {
            foreach (string directory1 in (IEnumerable<string>) Directory.GetDirectories(path))
              stringStack.Push(directory1);
          }
        }
        catch (Exception ex)
        {
          if (errorPaths != null)
            errorPaths.Add(path);
        }
      }
      return (ICollection<string>) stringList;
    }

    public static ICollection<string> GetFileExtensions(AppLockerFileType fileType)
    {
      if (fileType == AppLockerFileType.NotSupported)
        throw new ArgumentOutOfRangeException(nameof (fileType));
      IAppIdPolicyHelper appIdPolicyHelper = (IAppIdPolicyHelper) new AppIdPolicyHelperClass();
      FILE_TYPE eFileType = FileManager.TranslateFileType(fileType);
      string empty = string.Empty;
      string fileExtensions;
      try
      {
        fileExtensions = appIdPolicyHelper.GetFileExtensions(eFileType);
      }
      catch (COMException ex)
      {
        throw new GetFileExtensionsException((Exception) ex);
      }
      return (ICollection<string>) fileExtensions.Split(',');
    }

    public static string GetFileRuleCollection(AppLockerFileType fileType)
    {
      if (fileType == AppLockerFileType.NotSupported)
        throw new ArgumentOutOfRangeException(nameof (fileType));
      IAppIdPolicyHelper appIdPolicyHelper = (IAppIdPolicyHelper) new AppIdPolicyHelperClass();
      FILE_TYPE eFileType = FileManager.TranslateFileType(fileType);
      string empty = string.Empty;
      try
      {
        return appIdPolicyHelper.GetFileRuleCollection(eFileType);
      }
      catch (COMException ex)
      {
        throw new GetFileExtensionsException((Exception) ex);
      }
    }

    private static string GetFilePath(FileInformation fileInformation)
    {
      if (fileInformation == (FileInformation) null)
        throw new ArgumentNullException(nameof (fileInformation));
      return fileInformation.Path.Path;
    }

    private static AppLockerFileType TranslateFileType(FILE_TYPE engineFileType)
    {
      AppLockerFileType appLockerFileType = AppLockerFileType.NotSupported;
      switch (engineFileType)
      {
        case FILE_TYPE.FILE_TYPE_EXE:
          appLockerFileType = AppLockerFileType.Exe;
          break;
        case FILE_TYPE.FILE_TYPE_DLL:
          appLockerFileType = AppLockerFileType.Dll;
          break;
        case FILE_TYPE.FILE_TYPE_WINDOWS_INSTALLER:
          appLockerFileType = AppLockerFileType.WindowsInstaller;
          break;
        case FILE_TYPE.FILE_TYPE_SCRIPT:
          appLockerFileType = AppLockerFileType.Script;
          break;
        case FILE_TYPE.FILE_TYPE_APPX:
          appLockerFileType = AppLockerFileType.AppX;
          break;
      }
      return appLockerFileType;
    }

    private static FILE_TYPE TranslateFileType(AppLockerFileType fileType)
    {
      FILE_TYPE fileType1 = FILE_TYPE.FILE_TYPE_NOT_SUPPORTED;
      switch (fileType)
      {
        case AppLockerFileType.Exe:
          fileType1 = FILE_TYPE.FILE_TYPE_EXE;
          break;
        case AppLockerFileType.Dll:
          fileType1 = FILE_TYPE.FILE_TYPE_DLL;
          break;
        case AppLockerFileType.WindowsInstaller:
          fileType1 = FILE_TYPE.FILE_TYPE_WINDOWS_INSTALLER;
          break;
        case AppLockerFileType.Script:
          fileType1 = FILE_TYPE.FILE_TYPE_SCRIPT;
          break;
        case AppLockerFileType.AppX:
          fileType1 = FILE_TYPE.FILE_TYPE_APPX;
          break;
      }
      return fileType1;
    }

    public static string EncodeFilePublisherInformation(
      string publisherInformation,
      bool ignoreWildCharacters)
    {
      if (publisherInformation == null)
        throw new ArgumentNullException(nameof (publisherInformation));
      string empty = string.Empty;
      IAppIdPolicyHelper appIdPolicyHelper = (IAppIdPolicyHelper) new AppIdPolicyHelperClass();
      try
      {
        return appIdPolicyHelper.EncodeFilePublisherInformation(publisherInformation, ignoreWildCharacters ? 1 : 0);
      }
      catch (COMException ex)
      {
        throw new EncodeFilePublisherInformationException(publisherInformation, (Exception) ex);
      }
    }

    public static string DecodeFilePublisherInformation(string publisherInformation)
    {
      if (publisherInformation == null)
        throw new ArgumentNullException(nameof (publisherInformation));
      string empty = string.Empty;
      IAppIdPolicyHelper appIdPolicyHelper = (IAppIdPolicyHelper) new AppIdPolicyHelperClass();
      try
      {
        return appIdPolicyHelper.DecodeFilePublisherInformation(publisherInformation);
      }
      catch (COMException ex)
      {
        throw new DecodeFilePublisherInformationException(publisherInformation, (Exception) ex);
      }
    }
  }
}
