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

## Disclaimer

⚠️ **This project is for educational and security research purposes only.**  
It demonstrates techniques used in real-world malware but is not intended for malicious use. Using this code to collect data from systems you do not own or have explicit consent to access is illegal and unethical. **The author is not responsible for any misuse of this tool.**

Respect privacy and legal boundaries at all times.
