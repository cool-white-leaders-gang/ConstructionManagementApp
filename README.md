# ConstructionManagementApp
Pewnie! Poniżej masz gotową sekcję do pliku `README.md`, z instrukcją krok po kroku, jak uruchomić aplikację **ConstructionManagementApp**.

---

## 🚀 Jak uruchomić projekt ConstructionManagementApp

### 📋 Wymagania:
- [XAMPP](https://www.apachefriends.org) (Apache + MySQL)
- Visual Studio 2022 z obsługą .NET
- Git + Git Bash

---

### ✅ Krok po kroku:

1. **Zainstaluj XAMPP**  
   Pobierz i zainstaluj XAMPP ze strony:  
   👉 [https://www.apachefriends.org](https://www.apachefriends.org)

2. **Zainstaluj Visual Studio 2022**  
   Podczas instalacji zaznacz komponent **“.NET Desktop Development”**.

3. **Sklonuj repozytorium**  
   Otwórz Git Bash i wpisz:
   ```bash
   git clone https://github.com/cool-white-leaders-gang/ConstructionManagementApp.git
   ```

4. **Uruchom XAMPP**  
   - Otwórz **XAMPP Control Panel**
   - Wciśnij **Start** obok:
     - `Apache`
     - `MySQL`

5. **Przejdź do phpMyAdmin**  
   Wpisz w przeglądarce:
   ```
   http://localhost/phpmyadmin
   ```

6. **Utwórz nową bazę danych**
   - Kliknij **Nowa**
   - Wprowadź nazwę:
     ```
     construction_management
     ```

7. **Importuj plik SQL**
   - Przejdź do zakładki **Import**
   - Wybierz plik `construction_management.sql` z repozytorium
   - Kliknij **Wykonaj**

8. **Otwórz projekt w Visual Studio**
   - Otwórz plik `ConstructionManagementApp.sln` lub `Program.cs`
   - Upewnij się, że połączenie z bazą danych działa
   - Uruchom aplikację (`F5` lub klikając przycisk **Start**)
