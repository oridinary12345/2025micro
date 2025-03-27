using System;
using System.Collections;
using UnityEngine;

public class FileDownloader : MonoSingleton<FileDownloader>
{
	protected override void Init()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void DownloadCSV(string url, Action<CSVFile> onFileDownloaded, Action<string> onError)
	{
		Action<string> onFileDownloaded2 = delegate(string text)
		{
			CSVFile cSVFile = new CSVFile(text);
			if (cSVFile.IsValid)
			{
				if (onFileDownloaded != null)
				{
					onFileDownloaded(cSVFile);
				}
			}
			else if (onError != null)
			{
				onError("CSV file is downloaded but is invalid.");
			}
		};
		StartCoroutine(DownloadTextgCR(url, onFileDownloaded2, onError));
	}

	public void DownloadText(string url, Action<string> onFileDownloaded, Action<string> onError)
	{
		StartCoroutine(DownloadTextgCR(url, onFileDownloaded, onError));
	}

	private IEnumerator DownloadTextgCR(string url, Action<string> onFileDownloaded, Action<string> onError)
	{
		if (!string.IsNullOrEmpty(url))
		{
			WWW www = new WWW(url);
			while (!www.isDone)
			{
				yield return null;
			}
			if (!string.IsNullOrEmpty(www.error))
			{
				onError(www.error);
			}
			else
			{
				onFileDownloaded(www.text);
			}
		}
	}
}