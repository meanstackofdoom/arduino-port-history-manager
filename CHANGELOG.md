# Changelog

All notable changes to this project will be documented in this file.

This project follows a pragmatic, real-world approach to versioning.
Features may evolve rapidly as SimHub integrations are explored.

---

## [0.1.0] – Initial Public Release
**2025-12-24**

### ✨ Added
- Initial release of **SimHub Arduino Manager**
- Web-based dashboard for managing SimHub Arduino devices
- Automatic detection of connected Arduino serial ports
- Persistent device registry stored in `ports.json`
- Per-device **Identify ID** assignment system
- Visual **Identify / Blink** action to map physical devices
- Bulk install function to auto-assign IDs to all devices
- Editable device metadata:
  - Name
  - Role (LED / Gauge / Buttons / Other)
  - Tags
- Clean, dark-mode UI designed for sim racing environments
- Flask-based backend with lightweight frontend

### 🧪 Experimental
- Keyboard-triggered SimHub Identify integration
- Early SimHub API probing for future telemetry & health features
- Dynamic device key handling based on USB identifiers

### 🛠️ Internal
- Modularized device logic into `port_manager.py`
- Flask routing split cleanly between actions and rendering
- Basic error handling for missing devices and malformed data
- Safe handling of empty or invalid update requests

### ⚠️ Known Limitations
- SimHub API access is experimental and may vary by version
- Device heartbeat / RX-TX health indicators not yet implemented
- USB identifiers may differ across systems and reconnects
- Not yet tested with large multi-rig setups

---

## Planned (Upcoming)

### 🚧 0.2.x – Device Health & Metadata
- Live device health / heartbeat indicators
- RX / TX counters from SimHub
- Firmware name & MCU type display
- Baud rate & LED count metadata
- Improved USB key normalization

### 🚧 0.3.x – Power User Features
- Device grouping & profiles
- Export / import configurations
- Multi-rig support
- Improved SimHub API integration (if available)
- Optional background polling service

---

## Notes

This project is:
- **Independent** of SimHub
- **Community-driven**
- **Experimental by design**

Feedback, issues, and pull requests are welcome.


