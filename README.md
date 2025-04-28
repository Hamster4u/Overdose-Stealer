## Overview

**Overdose Stealer** is a tool developed to demonstrate the process of extracting and decrypting Discord authentication tokens stored locally on a Windows system. This tool uses DPAPI and AES-GCM decryption techniques to recover the tokens from Discord’s installation files. The extracted tokens, along with system information like the username and IP address, are sent to a specified Discord webhook in a structured format.

This public version is intended for educational purposes, showcasing the core functionality of the tool. A more advanced and feature-rich private version is under development.

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

## Dependencies

- **.NET Framework 4.7.2+** or **.NET Core / .NET 5+**  
  - [Download .NET Framework](https://dotnet.microsoft.com/download)
  - [Download .NET Core / .NET 5+](https://dotnet.microsoft.com/download)
  
- **BouncyCastle.Crypto.dll**  
  - Available via NuGet: [BouncyCastle](https://www.nuget.org/packages/BouncyCastle/)

### Antivirus [scantime] Detection Status (as of 2025-04-27)
- **Adaware**: ✅ Clean
- **Alyac**: ✅ Clean
- **Amiti**: ✅ Clean
- **Arcabit**: ✅ Clean
- **Avast**: ✅ Clean
- **AVG**: ✅ Clean
- **Avira**: ✅ Clean
- **Bullguard**: ✅ Clean
- **Clamav**: ✅ Clean
- **Comodo**: ✅ Clean
- **Comodo Linux**: ✅ Clean
- **DrWeb**: ✅ Clean
- **Emsisoft**: ✅ Clean
- **Escan**: ✅ Clean
- **F-Prot**: ✅ Clean
- **F-Secure**: ✅ Clean
- **GData**: ✅ Clean
- **Ikarus**: ✅ Clean
- **Immunet**: ✅ Clean
- **Kaspersky**: ✅ Clean
- **MaxSecure**: ✅ Clean
- **McAfee**: ✅ Clean
- **Microsoft Defender**: ✅ Clean
- **Nano**: ✅ Clean
- **Nod32**: ✅ Clean
- **Norman**: ✅ Clean
- **QuickHeal**: ✅ Clean
- **SecureAge Apex**: ❌ Unknown (Detected)
- **Seqrite**: ✅ Clean
- **Sophos**: ✅ Clean
- **TrendMicro**: ✅ Clean
- **VBA32**: ✅ Clean
- **ViritExplorer**: ✅ Clean
- **VirusFighter**: ✅ Clean
- **Xvirus**: ✅ Clean
- **Zillya**: ✅ Clean
- **ZoneAlarm**: ✅ Clean
- **Zoner**: ✅ Clean

**Source**: [WebSec Scanner Result](https://websec.net/scanner/result/9fb3481f-fc26-473c-815b-cb0a3cb3bcfa)

### Antivirus [Runtime] Scan Results (as of 2025-04-27)

- **Amiti**: Undetected ✅
- **Arcabit**: Undetected ✅
- **Avast**: Undetected ✅ [screenshot.jpg](https://github.com/user-attachments/assets/ed7f6a5b-7520-45ff-8e87-abd26892de36)
- **AVG**: Undetected ✅
- **Bitdefender**: Undetected ✅ [screenshot.jpg](https://github.com/user-attachments/assets/f96eb16f-e212-41a0-83ef-e43ffd1683b7)
- **Crowdstrike**: Detected ❌
- **F-Secure**: Undetected ✅
- **IKARUS**: Undetected ✅
- **Kaspersky**: Undetected ✅ [screenshot.jpg](https://github.com/user-attachments/assets/e7ccd412-b09d-4184-8ea3-15e0d3bc0c5c)
- **Microsoft Defender**: Undetected ✅
- **Nod32**: Undetected ✅ [screenshot.jpg](https://github.com/user-attachments/assets/9538a0bd-d636-42b7-b209-d8476e604696)
- **Norton**: Undetected ✅
- **Threatdown**: Undetected ✅
- **Xvirus**: Undetected ✅

## Disclaimer

⚠️ **This project is for educational and security research purposes only.**  
It demonstrates techniques used in real-world malware but is not intended for malicious use. Using this code to collect data from systems you do not own or have explicit consent to access is illegal and unethical. **The author is not responsible for any misuse of this tool.**

Respect privacy and legal boundaries at all times.

![cd4dd9d794422a3d4b36a469d4ff6e1f](https://github.com/user-attachments/assets/69012334-6bf0-4865-a519-27ef0b26d0a2)
