using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto;
using System.Threading.Tasks;
using System.Data.Common;

namespace AFIP_API.Tools.PfxGeneration
{
    public class PfxMaker
    {
        private DirectoryInfo? _mDirectoryInfo;
        public string? mCertPfxPath { get; private set; }
        public string? mCertPfxPassword { get; private set; }

		public async Task<string> CreatePFXFile(string pCrtPath, string pPrivateKeyPath, string pPfxFolderToSave, string pPfxPassword, string pPfxName) 
        {
            try
            {
                string lstrResult = "OK";
                _mDirectoryInfo = new DirectoryInfo(pPfxFolderToSave);

                List<FileInfo> lastPfxCreated = _mDirectoryInfo.GetFiles().Where((x) => x.Extension == ".pfx").OrderByDescending((t) => t.LastWriteTime).ToList();

                if (lastPfxCreated.Count > 0)
                {
                    mCertPfxPath = lastPfxCreated.First().FullName;
                    mCertPfxPassword = pPfxPassword;
                }
                else
                {
                    //lstrResult = await ExecuteCommandAsync("openssl.exe pkcs12 -in " + pCrtPath + " -inkey " + pPrivateKeyPath + " -export -out " + pPfxName + ".pfx -passout pass:" + pPfxPassword, pPfxFolderToSave, pPfxPassword);
                    lstrResult = await PFXBouncyCastleMeker(pPfxFolderToSave + @"\" +pPfxName + ".pfx", pPfxPassword, pCrtPath, pPrivateKeyPath);
				}                   
                return lstrResult;
            }
            catch (Exception ex) { throw new Exception($"{ex.Message}\n" + "Error in CreatePFXFile" + "\nStackTrace: " + ex.StackTrace); }
		}

		public string CheckExistisPfx(string pPfxPath) => File.Exists(pPfxPath) ? "OK" : "El pfx que se indico no existe en la carpeta";
        private async Task<string> PFXBouncyCastleMeker(string pPfxFolderToSave, string pPfxPassword, string pCrt, string pKey)
        {		
			string lstrResult = "OK";
			try
			{
				var subThread = new Task(() =>
                {
					Pkcs12Store store = new Pkcs12Store();
					X509CertificateParser parser = new X509CertificateParser();
					Org.BouncyCastle.X509.X509Certificate[] chain = { parser.ReadCertificate(File.ReadAllBytes(pCrt)) };

					Org.BouncyCastle.OpenSsl.PemReader pemReader = new Org.BouncyCastle.OpenSsl.PemReader(File.OpenText(pKey));
					AsymmetricCipherKeyPair keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();

					X509CertificateEntry[] entries = chain.Select(c => new X509CertificateEntry(c)).ToArray();

					store.SetKeyEntry("CVM", new AsymmetricKeyEntry(keyPair.Private), entries);

					using (FileStream fs = new FileStream(pPfxFolderToSave, FileMode.Create, FileAccess.Write))
					{
						store.Save(fs, pPfxPassword.ToCharArray(), new SecureRandom());
					}

					if (_mDirectoryInfo.GetFiles().Length == 0)
						lstrResult = "No hay archivos en la carpeta o no se pudo crear PFX";

					mCertPfxPath = _mDirectoryInfo.GetFiles().OrderByDescending((f) => f.LastWriteTime).First().FullName;
					mCertPfxPassword = pPfxPassword;
				});

				subThread.Start();

				await subThread;

				if (subThread.IsCompleted != true)
					lstrResult = "error thread";
			}
			catch (Exception ex) { throw new Exception($"{ex.Message}\n" + "Error in PFXBouncyCastleMeker" + "\nStackTrace: " + ex.StackTrace); }

			return lstrResult;
		}
        private async Task<string> ExecuteCommandAsync(string pCommand, string pPfxFolderToSave, string pPfxPassword)
        {
            string lstrResult = "OK";

            lstrResult = await ExecuteProcessAsync(pCommand, pPfxFolderToSave);

            if (lstrResult == "OK")
            {
                try
                {
                    if (_mDirectoryInfo.GetFiles().Length == 0)
                        lstrResult = "No hay archivos en la carpeta o no se pudo crear PFX";

                    mCertPfxPath = _mDirectoryInfo.GetFiles().OrderByDescending((f) => f.LastWriteTime).First().FullName;
                    mCertPfxPassword = pPfxPassword;
                }
                catch (Exception ex)
                {
                    lstrResult = ex.Message;
                }
            }
            return lstrResult;
        }
        private async Task<string> ExecuteProcessAsync(string pCommand, string pWorkingDirectory)
        {
            string lstrResult = "OK";
            try
            {
				var subThread = new Task(() =>
				{
					Process process = Process.Start(new ProcessStartInfo("cmd.exe", "/c " + pCommand)
					{
						CreateNoWindow = true,
						UseShellExecute = false,
						WorkingDirectory = pWorkingDirectory,
					});                    
                    process.WaitForExit();
					
                    //Thread.Sleep(2000);
                    
					process.Close();                    
				});

			    subThread.Start();

                await subThread;

                if (subThread.IsCompleted != true)
                    lstrResult = "error thread";                
			}
            catch (Exception ex)
            {
                lstrResult = ex.Message;
            }
            return lstrResult;
        }
    }
}
