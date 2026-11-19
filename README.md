# HelpDeskApp

> ⚠️ **Hinweis:** Dies ist ein Übungsprojekt. Beim ersten Start wird automatisch
> ein Test-Admin angelegt (Benutzer `admin`, Passwort `admin123`).
> Dieses Standardpasswort ist **nur zum Testen** gedacht und darf in einem
> echten Einsatz **nicht** verwendet werden.

## Über das Projekt

Ein Helpdesk-Ticketsystem als WPF-Desktop-Anwendung (C#, .NET Framework 4.7).
Entstanden als Lernprojekt zu den Creational Design Patterns (Singleton,
Factory Method, Abstract Factory, Builder, Prototype), später erweitert um MVVM,
Entity Framework 6 und eine Benutzerverwaltung mit Admin-Rolle.

## Funktionen

**Für alle Benutzer**
- Registrierung und Login (Passwörter mit BCrypt gehasht)
- Tickets anlegen (kritisch oder normal) mit Titel, Beschreibung, Ort und Anhängen
- Vorlagen auswählen und klonen
- Tagesbericht anzeigen und als PDF exportieren

**Zusätzlich für Admins**
- Tickets bearbeiten (Status, Zuweisung, Notizen mit Zeitstempel) und löschen
- Vorlagen anlegen, auch aus bestehenden Tickets
- Log exportieren und leeren
- Benutzerverwaltung (Admin-Rechte vergeben, Benutzer aktivieren/deaktivieren)

## Geplant / Offen

- [ ] Vorlagen löschen und bearbeiten
- [ ] Auswertungen und Statistiken
- [ ] E-Mail-Benachrichtigung (Code vorhanden, in der Oberfläche ausgeblendet)
- [ ] Zoom-Funktion für das Hauptfenster
- [ ] Oberfläche überarbeiten: übersichtlicher und fokussierter, mit Tabs (Tickets, Vorlagen, Log, Verwaltung)