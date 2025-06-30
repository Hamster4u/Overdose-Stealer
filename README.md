## Overview

**Overdose Stealer** is a tool developed to demonstrate the process of extracting and decrypting Discord authentication tokens stored locally on a Windows system. This tool uses DPAPI and AES-GCM decryption techniques to recover the tokens from Discord’s installation files. The extracted tokens, along with system information like the username and IP address, are sent to a specified Discord webhook in a structured format.

This public version is intended for educational purposes, showcasing the core functionality of the tool. A more advanced and feature-rich private version is under development.

> 🔄 **Update (2025-06-30)**:  
> 🛡️ New version includes changes to evade Microsoft Defender detection: ❌ Bypasses Program:Win32/Wacapew.C!ml


> 🔄 **Update (2025-04-30)**:  
> A new module has been added to detect and extract local wallet data from several popular cryptocurrency wallets, expanding the forensic capabilities of the tool.

## Key Features

🔑 **Token Extraction**  
   - Extracts Discord authentication tokens from `%AppData%\Roaming\discord`.

🔐 **Dual Token Support**  
   - Handles both AES-GCM encrypted tokens and plain text MFA tokens.

🔓 **Decryption Techniques**  
   - Uses DPAPI to decrypt the master key stored in the `Local State` file.  
   - Utilizes AES-GCM decryption via the BouncyCastle library to extract tokens.

🖥️ **System Info Collection**  
   - Collects basic system information such as the username and public IP address.

📤 **Webhook Reporting**  
   - Sends the extracted data as a structured Discord embed to a specified webhook URL.

💰 **Cryptowallet Data Detection** *(New)*  
   - Scans known local directories for the presence of wallet files from the following clients:
     - **Zcash**
     - **Armory**
     - **Bytecoin**
     - **Jaxx**
     - **Exodus**
     - **Ethereum**
     - **Electrum**
     - **AtomicWallet**
     - **Guarda**
     - **Coinomi**
   - Detects typical LevelDB, keystore, or wallet files for further manual analysis.

## Dependencies

- **.NET Framework 4.7.2+** or **.NET Core / .NET 5+**  
  - [Download .NET Framework](https://dotnet.microsoft.com/download)  
  - [Download .NET Core / .NET 5+](https://dotnet.microsoft.com/download)

- **BouncyCastle.Crypto.dll**  
  - Available via NuGet: [BouncyCastle](https://www.nuget.org/packages/BouncyCastle/)

## Antivirus Scan Results

### Scantime Detection Status *(as of 2025-06-30)*

| Engine         | Status  |
|----------------|---------|
| Adaware        | ✅ Clean |
| Alyac          | ✅ Clean |
| Amiti          | ✅ Clean |
| Arcabit        | ✅ Clean |
| Avast          | ✅ Clean |
| AVG            | ✅ Clean |
| Avira          | ✅ Clean |
| Bullguard      | ✅ Clean |
| Clamav         | ✅ Clean |
| Comodo         | ✅ Clean |
| DrWeb          | ✅ Clean |
| Emsisoft       | ✅ Clean |
| F-Secure       | ✅ Clean |
| GData          | ✅ Clean |
| Ikarus         | ✅ Clean |
| Kaspersky      | ✅ Clean |
| McAfee         | ✅ Clean |
| Microsoft Defender | ❌ Program:Win32/Wacapew.C!ml |
| Nod32          | ✅ Clean |
| Norton         | ✅ Clean |
| Sophos         | ✅ Clean |
| TrendMicro     | ✅ Clean |
| SecureAge Apex | ❌ Detected |
| Others         | ✅ Clean |

📌 **Source**: [WebSec Scanner Result](https://websec.net/scanner/result/d4fd26a5-ab89-4feb-9b61-29637a890be3)

### Runtime Detection *(as of 2025-04-27)*

| Engine         | Status  |
|----------------|---------|
| Avast          | ✅ Undetected [Screenshot](https://github.com/user-attachments/assets/ed7f6a5b-7520-45ff-8e87-abd26892de36) |
| Bitdefender    | ✅ Undetected [Screenshot](https://github.com/user-attachments/assets/f96eb16f-e212-41a0-83ef-e43ffd1683b7) |
| Kaspersky      | ✅ Undetected [Screenshot](https://github.com/user-attachments/assets/e7ccd412-b09d-4184-8ea3-15e0d3bc0c5c) |
| Nod32          | ✅ Undetected [Screenshot](https://github.com/user-attachments/assets/9538a0bd-d636-42b7-b209-d8476e604696) |
| Crowdstrike    | ❌ Detected |
| Others         | ✅ Undetected |

## Disclaimer

⚠️ **This project is provided strictly for educational and ethical security research purposes only.**  
It demonstrates techniques commonly used in real-world security threats and is intended for those studying digital forensics, malware analysis, and secure software design.

**Do not use this software on any system you do not own or have explicit written permission to test.**  
Unauthorized access, data exfiltration, or tampering is a criminal offense under international and local laws.

The authors and contributors are not responsible for any misuse or damages caused by this tool.

![Security research only](https://github.com/user-attachments/assets/69012334-6bf0-4865-a519-27ef0b26d0a2)
