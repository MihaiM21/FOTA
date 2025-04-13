<p align=center>
<img src="FOTA/lib/logo.png" width="200">
</p>

<h1 align="center">Formula One Telemetry Analysis - FOTA</h1>

<p align=center>
<b>Powered by TurnOneOfficial -</b>
<a href="https://www.t1f1.com">t1f1.com</a>
</p>

> [!NOTE]
> This application is still in **active development** with no official release date.
> It is currently functional with a **basic user interface**.

---

## Table of Contents

- [About FOTA](#about-fota)
- [How to Install](#how-to-install)
- [How to Use](#how-to-use)
- [Future Plans](#future-plans)
- [Examples of Plots Generated with FOTA](#examples-of-plots-generated-with-fota)
- [Contact & Support](#contact--support)

---

## About FOTA

The **Formula One Telemetry Analysis** (FOTA) application is designed to process and visualize telemetry data from Formula One cars. This tool allows users to generate insightful plots and graphs that provide a deep understanding of performance metrics such as speed, throttle, braking, and sector times. FOTA is built with simplicity in mind but aims to become a comprehensive telemetry analysis suite in the future.

Planned future updates include:
- [ ] **Enhanced Graphing Options**: More customizable data visualizations.
- [ ] **Expanded Platform Support**: Developing dedicated apps for Android platforms with user-friendly GUIs.
- [ ] **Advanced Features**: Additional functionalities like comparative analysis between drivers or races, lap-by-lap breakdowns, and real-time data tracking (subject to availability).

Whether you're a Formula One enthusiast, a data analyst, or a racing engineer, FOTA offers a powerful way to dive into the rich data of the sport.

## How to Install

### Prerequisites

- [.NET 6.0 SDK or later](https://dotnet.microsoft.com/download/dotnet/6.0)
- A valid token for authenticating requests to the server. The token can be bought from [TurnOne](https://t1f1.com/product/telemetry-token/)
- If you want tokens for testing feel free to contact me at mihai@t1f1.com

## Installation Steps

### For regular users
1. Download the release version and install it (Release ETA ~ 01/05/2025)

### For Developers

1. Clone the repository:
   ```bash
   git clone https://github.com/MihaiM21/FOTA-DesktopApp
   ```

2. Navigate to the project directory:
   ```bash
   cd FOTA-DesktopApp
   ```

3. Open the solution in your preferred IDE (e.g., Visual Studio or JetBrains Rider).

4. Restore dependencies:
   ```bash
   dotnet restore
   ```

5. Build and run the application:
   ```bash
   dotnet run
   ```

---

## How to Use

1. **Launch the Application**:
   Open the FOTA application.

2. **Select the Parameters**:
    - **Year**: Select the specific season you want data from.
    - **Plot Type**: Depending on the type of analysis you want to perform, select a plot type. Some options will require specifying the **drivers** or **teams** involved.
    - **Event Type**: Choose from various Formula One event types:
        - `"R"` → **Race**
        - `"S"` → **Sprint Race**
        - `"Q"` → **Qualifying**
        - `"SQ"` → **Sprint Qualifying**
        - `"FP1"`, `"FP2"`, `"FP3"` → **Free Practice Sessions** 1, 2, or 3
    - **Token**: Provide your authentication token to validate the request with the server.

3. **Execute**:
   Once you’ve made your selections, click on **Generate**. The application will send a request to the server, validate the token, and generate the requested plot.

4. **View Results**:
    - The generated telemetry plot will be displayed in the application.
    - Use the **Download Image** button to save the plot locally as a `.png` file.

---

## Examples of Plots Generated with FOTA
<p align=center>
    <img src="FOTA/lib/Driver Analysis 2025 Bahrain Grand Prix VER Qualifying.png" width="800">
    <img src="FOTA/lib/2025 Bahrain Grand Prix Quali results.png" width="800">
    <img src="FOTA/lib/Top speed comparison 2025 Bahrain Grand Prix Qualifying .png" width="800">
    <img src="FOTA/lib/Bahrain Grand Prix Qualifying 2025 LEC vs VER.png" width="800">
    <img src="FOTA/lib/2025 Japanese Grand Prix Tyre strategy.png" width="800">
    <img src="FOTA/lib/2025 Japanese Grand Prix Team pace.png" width="800">
    <img src="FOTA/lib/2025 Japanese Grand Prix Position changes.png" width="800">
    <img src="FOTA/lib/2025 Japanese Grand Prix LEC vs VER Fastest Lap Comparison.png" width="800">
    <img src="FOTA/lib/2025 Japanese Grand Prix Laptimes distribution.png" width="800">
    <img src="FOTA/lib/2025 Bahrain Grand Prix Throttle comparison.png" width="800">
</p>

---

## Contact & Support

For questions or support, feel free to reach out:

- **Email**: mihai@t1f1.com
- **GitHub**: MihaiM21(https://github.com/MihaiM21)

<p align=center>
<b>Powered by TurnOneOfficial</b>
</p>
