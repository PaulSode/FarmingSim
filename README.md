# Farming Game - README

## Description
Ce projet est un jeu de simulation agricole développé en Unity utilisant C#. Le joueur gère une ferme en cultivant des champs, achetant des véhicules, construisant des usines de transformation et gérant ses ressources financières.

## Fonctionnalités principales

### 🚜 Gestion des véhicules
- Achat de véhicules spécialisés (tracteurs, semoirs, moissonneuses, etc.)
- Système de disponibilité et de réservation des véhicules
- Véhicules requis pour chaque étape de culture

### 🌾 Système de culture
- Création et gestion de champs
- Cycle de culture complet : Labour → Semis → Fertilisation (optionnelle) → Récolte
- Différents types de cultures avec rendements variables
- Système de fertilisation pour doubler les rendements

### 🏭 Usines de transformation
- Construction d'usines pour transformer les matières premières
- Système d'intrants (cultures ou produits transformés)
- Multiplicateurs de production
- Gestion automatique avec responsables d'usine

### 💰 Gestion financière
- Système bancaire avec suivi des finances
- Achat/vente de véhicules et construction d'usines
- Vente automatique ou manuelle des produits

### 📦 Stockage (Silo)
- Stockage des cultures et produits transformés
- Limite de capacité de stockage
- Système de vente automatique configurable

## Architecture du code

### Managers principaux
- **Banque** : Gestion des finances du joueur
- **Ferme** : Création et gestion des champs
- **Garage** : Achat et gestion des véhicules
- **Silo** : Stockage des cultures et produits
- **Usineur** : Gestion des usines de transformation

### Classes de données
- **Culture** : Définit les cultures avec leurs rendements et véhicules requis
- **Vehicule** : Représente les véhicules avec prix, taille et disponibilité
- **Produit** : Produits transformés avec leur valeur marchande
- **Usine** : Usines de transformation avec leurs intrants et multiplicateurs

### Objets de jeu
- **Champs** : Gestion individuelle des parcelles avec état et progression
- **Usine** : Logique de production des usines
- **AdderLine** : Interface pour l'achat d'éléments

## Système de données

Le jeu utilise des fichiers JSON pour configurer :
- `Vehicules.json` : Liste des véhicules disponibles
- `Cultures.json` : Types de cultures et leurs propriétés
- `Produits.json` : Produits transformés et leurs prix
- `Usines.json` : Usines disponibles avec leurs recettes

## États des champs

Les champs suivent un cycle d'états :
1. **Récolté** : Prêt pour le labour
2. **Labouré** : En cours de labour
3. **LabouréFin** : Prêt pour le semis
4. **Semé** : En cours de croissance (fertilisation possible)
5. **Fertilisé** : Croissance avec bonus de rendement
6. **Prêt** : Prêt pour la récolte

## Interface utilisateur

- **Système d'onglets** : Navigation entre les différentes sections
- **UI de gestion** : Interfaces pour chaque aspect du jeu
- **Feedback visuel** : Barres de progression et état des éléments

## Fonctionnalités avancées

### Gestion des véhicules par étape
Chaque culture définit les véhicules nécessaires pour :
- Labour (Tracteur)
- Semis (Semeuse/Planteuse)
- Fertilisation (Fertilisateur)
- Récolte (Moissonneuse + Remorque)

### Système de lots
Possibilité de grouper des champs pour des opérations coordonnées. // Présent dans le code mais pas possible en jeu

### Automatisation
- Gestion automatique des usines avec responsables // Présent dans le code mais pas possible en jeu
- Vente automatique des produits // Présent dans le code mais pas possible en jeu
- Réservation automatique des véhicules 

## Installation et utilisation

Depuis l'édieur :
1. Ouvrir le projet dans Unity
2. Configurer les fichiers JSON dans le dossier StreamingAssets
3. Lancer le jeu depuis Unity
4. Commencer par acheter des véhicules (racteur, Remorque, Moissonneuse batteuse, Semeuse, Fertilisateur), créer des champs (Blé) et acheter une usine (Moulin à grain)

Depuis la livraison
1. Télécharger et extraire le ZIP
2. Lancer l'EXE
3. Commencer par acheter des véhicules (racteur, Remorque, Moissonneuse batteuse, Semeuse, Fertilisateur), créer des champs (Blé) et acheter une usine (Moulin à grain)

## Structure des fichiers

```
Assets/Scripts/
├── Data/              # Classes de données
├── Managers/          # Gestionnaires principaux
├── Objets/           # Objets de jeu
├── UI/               # Interface utilisateur
└── JsonHelper.cs     # Utilitaire pour JSON
```

## Notes techniques

- Pattern Singleton utilisé pour les managers
- Système d'événements pour la synchronisation des chargements
- Gestion des états avec des énumérations
- Interface utilisateur modulaire avec système d'onglets
